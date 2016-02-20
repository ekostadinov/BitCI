using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BitCI.Context;
using BitCI.Models.BuildSteps;

namespace BitCI.Controllers
{
    public class BuildStepsController : Controller
    {
        private ServerContext db = new ServerContext();

        // GET: BuildSteps
        public ActionResult Index()
        {
            var buildSteps = db.BuildSteps.Include(b => b.Build);
            return View(buildSteps.ToList());
        }

        // GET: BuildSteps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildStep buildStep = db.BuildSteps.Find(id);
            if (buildStep == null)
            {
                return HttpNotFound();
            }
            return View(buildStep);
        }

        // GET: BuildSteps/Create
        public ActionResult Create()
        {
            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace");
            return View();
        }

        // POST: BuildSteps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BuildId,Status,Value")] BuildStep buildStep)
        {
            if (ModelState.IsValid)
            {
                db.BuildSteps.Add(buildStep);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace", buildStep.BuildId);
            return View(buildStep);
        }

        // GET: BuildSteps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildStep buildStep = db.BuildSteps.Find(id);
            if (buildStep == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace", buildStep.BuildId);
            return View(buildStep);
        }

        // POST: BuildSteps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BuildId,Status,Value")] BuildStep buildStep)
        {
            if (ModelState.IsValid)
            {
                db.Entry(buildStep).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace", buildStep.BuildId);
            return View(buildStep);
        }

        // GET: BuildSteps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildStep buildStep = db.BuildSteps.Find(id);
            if (buildStep == null)
            {
                return HttpNotFound();
            }
            return View(buildStep);
        }

        // POST: BuildSteps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BuildStep buildStep = db.BuildSteps.Find(id);
            db.BuildSteps.Remove(buildStep);
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
