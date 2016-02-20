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
    public class VersionControlStepsController : Controller
    {
        private ServerContext db = new ServerContext();

        // GET: VersionControlSteps
        public ActionResult Index()
        {
            var vCSteps = db.VCSteps.Include(v => v.Build);
            return View(vCSteps.ToList());
        }

        // GET: VersionControlSteps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VersionControlStep versionControlStep = db.VCSteps.Find(id);
            if (versionControlStep == null)
            {
                return HttpNotFound();
            }
            return View(versionControlStep);
        }

        // GET: VersionControlSteps/Create
        public ActionResult Create()
        {
            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace");
            return View();
        }

        // POST: VersionControlSteps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BuildId,Status,Value")] VersionControlStep versionControlStep)
        {
            if (ModelState.IsValid)
            {
                db.VCSteps.Add(versionControlStep);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace", versionControlStep.BuildId);
            return View(versionControlStep);
        }

        // GET: VersionControlSteps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VersionControlStep versionControlStep = db.VCSteps.Find(id);
            if (versionControlStep == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace", versionControlStep.BuildId);
            return View(versionControlStep);
        }

        // POST: VersionControlSteps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BuildId,Status,Value")] VersionControlStep versionControlStep)
        {
            if (ModelState.IsValid)
            {
                db.Entry(versionControlStep).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace", versionControlStep.BuildId);
            return View(versionControlStep);
        }

        // GET: VersionControlSteps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VersionControlStep versionControlStep = db.VCSteps.Find(id);
            if (versionControlStep == null)
            {
                return HttpNotFound();
            }
            return View(versionControlStep);
        }

        // POST: VersionControlSteps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VersionControlStep versionControlStep = db.VCSteps.Find(id);
            db.VCSteps.Remove(versionControlStep);
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
