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
    public class PostBuildEmailStepsController : Controller
    {
        private ServerContext db = new ServerContext();

        // GET: PostBuildEmailSteps
        public ActionResult Index()
        {
            var emailSteps = db.EmailSteps.Include(p => p.Build);
            return View(emailSteps.ToList());
        }

        // GET: PostBuildEmailSteps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostBuildEmailStep postBuildEmailStep = db.EmailSteps.Find(id);
            if (postBuildEmailStep == null)
            {
                return HttpNotFound();
            }
            return View(postBuildEmailStep);
        }

        // GET: PostBuildEmailSteps/Create
        public ActionResult Create()
        {
            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace");
            return View();
        }

        // POST: PostBuildEmailSteps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BuildId,Status,Value")] PostBuildEmailStep postBuildEmailStep)
        {
            if (ModelState.IsValid)
            {
                db.EmailSteps.Add(postBuildEmailStep);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace", postBuildEmailStep.BuildId);
            return View(postBuildEmailStep);
        }

        // GET: PostBuildEmailSteps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostBuildEmailStep postBuildEmailStep = db.EmailSteps.Find(id);
            if (postBuildEmailStep == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace", postBuildEmailStep.BuildId);
            return View(postBuildEmailStep);
        }

        // POST: PostBuildEmailSteps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BuildId,Status,Value")] PostBuildEmailStep postBuildEmailStep)
        {
            if (ModelState.IsValid)
            {
                db.Entry(postBuildEmailStep).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuildId = new SelectList(db.Builds, "Id", "Workspace", postBuildEmailStep.BuildId);
            return View(postBuildEmailStep);
        }

        // GET: PostBuildEmailSteps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostBuildEmailStep postBuildEmailStep = db.EmailSteps.Find(id);
            if (postBuildEmailStep == null)
            {
                return HttpNotFound();
            }
            return View(postBuildEmailStep);
        }

        // POST: PostBuildEmailSteps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostBuildEmailStep postBuildEmailStep = db.EmailSteps.Find(id);
            db.EmailSteps.Remove(postBuildEmailStep);
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
