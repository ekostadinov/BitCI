﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Mvc;
using BitCI.Context;
using BitCI.Models;
using BitCI.Models.BuildSteps;

namespace BitCI.Controllers
{
    [Authorize] 
    public class BuildsController : Controller
    {
        private ServerContext db = new ServerContext();

        // GET: WorkDirectory
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        // GET: Builds
        public ActionResult Index()
        {
            Response.AddHeader("Refresh", "5");

            var builds = db.Builds.Include(b => b.Project).OrderBy(b => b.Id).ToList();
            var last10Builds = builds.Skip(Math.Max(0, builds.Count - 11)).ToList();

            return View(last10Builds);
        }

        // GET: Builds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Build build = db.Builds.Find(id);
            if (build == null)
            {
                return HttpNotFound();
            }

            if (System.IO.File.Exists(build.Log))
            {
                string logText = String.Empty;

                try
                {
                    object locker = new Object();
                    lock (locker)
                    {
                        logText = System.IO.File.ReadAllText(build.Log);
                    }
                }
                catch (IOException ioe)
                {
                    // currently this is not a multithreading application and the Log resource is locked by
                    // all steps for both write and read
                    var builds = db.Builds.Include(b => b.Project);                
                    build.Status = Build.BuildStatus.Running;
                    db.SaveChanges();
                    return View("Index", builds.ToList());
                }

                ViewBag.LogFileContent = System.IO.File.ReadAllLines(build.Log);
                Response.AddHeader("Refresh", "5");
            }

            return View(build);
        }

        // GET: Builds/Create
        public ActionResult Create()
        {
            ViewBag.Message = "Build created!";
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            return View();
        }

        // POST: Builds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId")] Build build, string vcs, string buildStep, string run, string mail, string trigger)
        {
            build.Status = Build.BuildStatus.Pending;
            build.Workspace = AssemblyDirectory + "\\..\\..\\work\\" + Guid.NewGuid().ToString().Substring(0, 8);
            if (!Directory.Exists(build.Workspace))
            {
                Directory.CreateDirectory(build.Workspace);
            }
            build.Workspace = build.Workspace.Replace("BitCI\\bin\\..\\..\\", String.Empty);
            build.Duration = "00:00:00";
            build.TriggeredBy = User.Identity.Name;
            build.Log = build.Workspace + "\\log.txt";


            if (!System.IO.File.Exists(build.Log))
            {
                using (FileStream fs = System.IO.File.Create(build.Log))
                {
                    string title = "------ Build Log ------";
                    fs.Write(System.Text.ASCIIEncoding.Default.GetBytes(title), 0, title.Length);
                }
            }
            build.StartDate = DateTime.Now;
            build.Steps = new List<IBuildStep>();
            db.VCSteps.Add(new VersionControlStep()
            {
                Build = build,
                BuildId = build.Id,
                Id = 0,
                Status = StepStatus.Idle,
                Value = vcs
            });

            db.BuildSteps.Add(new BuildStep()
            {
                Build = build,
                BuildId = build.Id,
                Id = 0,
                Status = StepStatus.Idle,
                Value = buildStep
            });

            db.RunTestsSteps.Add(new RunTestsStep()
            {
                Build = build,
                BuildId = build.Id,
                Id = 0,
                Status = StepStatus.Idle,
                Value = run
            });

            db.EmailSteps.Add(new PostBuildEmailStep()
            {
                Build = build,
                BuildId = build.Id,
                Id = 0,
                Status = StepStatus.Idle,
                Value = mail
            });

            db.TriggerSteps.Add(new TriggerStep()
            {
                Build = build,
                BuildId = build.Id,
                Id = 0,
                Status = StepStatus.Idle,
                Value = trigger
            });

            if (ModelState.IsValid)
            {
                db.Builds.Add(build);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // in case we get invalid state
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", build.ProjectId);
            return View(build);
        }

        // GET: Builds/Edit/5
        public ActionResult Run(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Build build = db.Builds.Find(id);
            if (build == null)
            {
                return HttpNotFound();
            }
            var project = db.Projects.FirstOrDefault(p => p.Id.Equals(build.ProjectId));
            ViewBag.ProjectName = project.Name;
            ViewBag.StartDateNow = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            return View(build);
        }

        // POST: Builds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Run([Bind(Include = "Id,ProjectId,Status,Workspace,Duration,TriggeredBy,Log,StartDate")] Build build)
        {
            var dbBuild = db.Builds.FirstOrDefault(b => b.Id.Equals(build.Id));
            
            if (ModelState.IsValid)
            {
                dbBuild.Status = Build.BuildStatus.Running;
                build.Status = Build.BuildStatus.Running;
                // needed for UX real-time monitoring
                db.SaveChanges();

                Stopwatch timer = new Stopwatch();
                timer.Start();
                
                ExecuteAllBuildSteps(dbBuild.Id);
                dbBuild.StartDate = DateTime.Now;
                
                timer.Stop();
                dbBuild.Duration = String.Empty;
                string zeroPrefix = "0";
                
                if (timer.Elapsed.Hours < 10)
                {
                    dbBuild.Duration += zeroPrefix;
                }
                dbBuild.Duration += timer.Elapsed.Hours.ToString() + ":";

                if (timer.Elapsed.Minutes < 10)
                {
                    dbBuild.Duration += zeroPrefix;
                }
                dbBuild.Duration += timer.Elapsed.Minutes.ToString() + ":";

                if (timer.Elapsed.Seconds < 10 )
                {
                    dbBuild.Duration += zeroPrefix;
                }
                dbBuild.Duration += timer.Elapsed.Seconds.ToString();

                build.TriggeredBy = User.Identity.Name;

                try
                {
                    string msbuildNoErrors = "0 Error(s)";
                    string nunitNoErrors = "Errors: 0,";
                    string logContent = System.IO.File.ReadAllText(dbBuild.Log);

                    if (logContent.Contains(msbuildNoErrors) && logContent.Contains(nunitNoErrors))
                    {
                        dbBuild.Status = Build.BuildStatus.Passed;
                        build.Status = Build.BuildStatus.Passed;
                    }
                    else
                    {
                        dbBuild.Status = Build.BuildStatus.Failed;
                        build.Status = Build.BuildStatus.Failed;
                    }
                }
                catch (IOException)
                {
                    throw;
                }


                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            // in case of invalid state
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", build.ProjectId);
            return View(build);
        }

        private void ExecuteAllBuildSteps(int buildId)
        {
            // this step is first since it affects the entire work-flow
            for (int step = 0; step < db.TriggerSteps.Count(); step++)
            {
                if (db.VCSteps.ToArray()[step].BuildId.Equals(buildId))
                {
                    db.TriggerSteps.ToArray()[step].Execute();
                    break;
                }
            }
            
            for (int step = 0; step < db.VCSteps.Count(); step++)
            {
                if (db.VCSteps.ToArray()[step].BuildId.Equals(buildId))
                {
                    db.VCSteps.ToArray()[step].Execute();
                    break;
                }
            }

            for (int step = 0; step < db.BuildSteps.Count(); step++)
            {
                if (db.VCSteps.ToArray()[step].BuildId.Equals(buildId))
                {
                    db.BuildSteps.ToArray()[step].Execute();
                    break;
                }
            }

            for (int step = 0; step < db.RunTestsSteps.Count(); step++)
            {
                if (db.VCSteps.ToArray()[step].BuildId.Equals(buildId))
                {
                    db.RunTestsSteps.ToArray()[step].Execute();
                    break;
                }
            }

            for (int step = 0; step < db.EmailSteps.Count(); step++)
            {
                if (db.VCSteps.ToArray()[step].BuildId.Equals(buildId))
                {
                    db.EmailSteps.ToArray()[step].Execute();
                    break;
                }
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
