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
    public class TriggerStepsController : Controller
    {
        private ServerContext db = new ServerContext();

        // GET: TriggerSteps
        public ActionResult Index()
        {
            var triggerSteps = db.TriggerSteps.Include(t => t.Build);
            return View(triggerSteps.ToList());
        }

        // GET: TriggerSteps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TriggerStep triggerStep = db.TriggerSteps.Find(id);
            if (triggerStep == null)
            {
                return HttpNotFound();
            }
            return View(triggerStep);
        }

        // GET: TriggerSteps/Create
        public ActionResult Create()
        {
            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace");
            return View();
        }

        // POST: TriggerSteps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BuildId,Status,Value")] TriggerStep triggerStep)
        {
            if (ModelState.IsValid)
            {
                db.TriggerSteps.Add(triggerStep);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace", triggerStep.BuildId);
            return View(triggerStep);
        }

        // GET: TriggerSteps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TriggerStep triggerStep = db.TriggerSteps.Find(id);
            if (triggerStep == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace", triggerStep.BuildId);
            return View(triggerStep);
        }

        // POST: TriggerSteps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BuildId,Status,Value")] TriggerStep triggerStep)
        {
            if (ModelState.IsValid)
            {
                db.Entry(triggerStep).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace", triggerStep.BuildId);
            return View(triggerStep);
        }

        // GET: TriggerSteps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TriggerStep triggerStep = db.TriggerSteps.Find(id);
            if (triggerStep == null)
            {
                return HttpNotFound();
            }
            return View(triggerStep);
        }

        // POST: TriggerSteps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TriggerStep triggerStep = db.TriggerSteps.Find(id);
            db.TriggerSteps.Remove(triggerStep);
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
