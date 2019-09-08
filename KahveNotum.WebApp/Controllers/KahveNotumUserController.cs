using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KahveNotum.Entities;
using KahveNotum.BusinessLayer;
using KahveNotum.BusinessLayer.Results;
using KahveNotum.WebApp.Filters;

namespace KahveNotum.WebApp.Controllers
{
    [AuthAdmin]
    [Exc]
    public class KahveNotumUserController : Controller
    {
        private UserManager userManager = new UserManager();


        // GET: KahveNotumUser
        public ActionResult Index()
        {
            return View(userManager.List());
        }

        // GET: KahveNotumUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KahveNotumUser kahveNotumUser = userManager.Find(x => x.ID == id.Value);
            if (kahveNotumUser == null)
            {
                return HttpNotFound();
            }
            return View(kahveNotumUser);
        }

        // GET: KahveNotumUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KahveNotumUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( KahveNotumUser kahveNotumUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                BusinessLayerResult<KahveNotumUser> res = userManager.Insert(kahveNotumUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(kahveNotumUser);
                }

               
                return RedirectToAction("Index");
            }

            return View(kahveNotumUser);
        }

        // GET: KahveNotumUser/Edit/5
        public ActionResult Edit(int? id)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KahveNotumUser kahveNotumUser = userManager.Find(x => x.ID == id.Value);
            if (kahveNotumUser == null)
            {
                return HttpNotFound();
            }
            return View(kahveNotumUser);
        }

        // POST: KahveNotumUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( KahveNotumUser kahveNotumUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                BusinessLayerResult<KahveNotumUser> res = userManager.Update(kahveNotumUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(kahveNotumUser);
                }

                return RedirectToAction("Index");
            }
            return View(kahveNotumUser);
        }

        // GET: KahveNotumUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KahveNotumUser kahveNotumUser = userManager.Find(x => x.ID == id.Value);
            if (kahveNotumUser == null)
            {
                return HttpNotFound();
            }
            return View(kahveNotumUser);
        }

        // POST: KahveNotumUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KahveNotumUser kahveNotumUser = userManager.Find(x => x.ID == id);
            userManager.Delete(kahveNotumUser);
            return RedirectToAction("Index");
        }

        
    }
}
