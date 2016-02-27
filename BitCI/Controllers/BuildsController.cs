using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BitCI.Context;
using BitCI.Models;
using BitCI.Models.BuildSteps;

namespace BitCI.Controllers
{
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

            var builds = db.Builds.Include(b => b.Project);
            string msbuildNoErrors = "0 Error(s)";
            string nunitNoErrors = "Errors: 0,";
            int latestBuilds = 10;
            int curentRunningbBuildDbCounter = 1;
            int runingBuildLogLength = 30;

            for (int buildCounter = builds.Count() - latestBuilds; buildCounter <= builds.Count() + curentRunningbBuildDbCounter; buildCounter++)
            {
                var build = builds.First(b => b.Id.Equals(buildCounter));
                string logText = string.Empty;

                try
                {
                    object locker = new Object();
                    lock (locker)
                    {
                        logText = System.IO.File.ReadAllText(build.Log);
                    }
                }
                catch (FileNotFoundException fnfe)
                {
                    // ignore, since we already updated the build.Status and cleaned the work directory
                }

                if (!build.Status.Equals(Build.BuildStatus.Passed) && !build.Status.Equals(Build.BuildStatus.Failed))
                {

                    if (logText.Length > runingBuildLogLength)
                    {
                        bool isBuildSuccessful = logText.Contains(msbuildNoErrors) && logText.Contains(nunitNoErrors);
                        if (isBuildSuccessful)
                        {
                            build.Status = Build.BuildStatus.Passed;
                        }
                        else
                        {
                            build.Status = Build.BuildStatus.Failed;
                        }   
                    }
                }
            }

            db.SaveChanges();
            return View(builds.ToList());
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
                ViewBag.LogFileContent = System.IO.File.ReadAllLines(build.Log);
                Response.AddHeader("Refresh", "5");
            }

            return View(build);
        }

        // GET: Builds/Create
        public ActionResult Create()
        {
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
            //todo: add user accounts
            build.TriggeredBy = "evgeni";
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
               
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
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

        // GET: Builds/Delete/5
        public ActionResult Delete(int? id)
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
            return View(build);
        }

        // POST: Builds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Build build = db.Builds.Find(id);
            db.Builds.Remove(build);
            db.SaveChanges();
            return RedirectToAction("Index");
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
