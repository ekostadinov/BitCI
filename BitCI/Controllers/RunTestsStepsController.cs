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
    public class RunTestsStepsController : Controller
    {
        private ServerContext db = new ServerContext();

        // GET: RunTestsSteps
        public ActionResult Index()
        {
            var runTestsSteps = db.RunTestsSteps.Include(r => r.Build);
            return View(runTestsSteps.ToList());
        }

        // GET: RunTestsSteps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RunTestsStep runTestsStep = db.RunTestsSteps.Find(id);
            if (runTestsStep == null)
            {
                return HttpNotFound();
            }
            return View(runTestsStep);
        }

        // GET: RunTestsSteps/Create
        public ActionResult Create()
        {
            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace");
            return View();
        }

        // POST: RunTestsSteps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BuildId,Status,Value")] RunTestsStep runTestsStep)
        {
            if (ModelState.IsValid)
            {
                db.RunTestsSteps.Add(runTestsStep);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace", runTestsStep.BuildId);
            return View(runTestsStep);
        }

        // GET: RunTestsSteps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RunTestsStep runTestsStep = db.RunTestsSteps.Find(id);
            if (runTestsStep == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace", runTestsStep.BuildId);
            return View(runTestsStep);
        }

        // POST: RunTestsSteps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BuildId,Status,Value")] RunTestsStep runTestsStep)
        {
            if (ModelState.IsValid)
            {
                db.Entry(runTestsStep).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace", runTestsStep.BuildId);
            return View(runTestsStep);
        }

        // GET: RunTestsSteps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RunTestsStep runTestsStep = db.RunTestsSteps.Find(id);
            if (runTestsStep == null)
            {
                return HttpNotFound();
            }
            return View(runTestsStep);
        }

        // POST: RunTestsSteps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RunTestsStep runTestsStep = db.RunTestsSteps.Find(id);
            db.RunTestsSteps.Remove(runTestsStep);
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
