using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KahveNotum.Entities;
using KahveNotum.Core.DataAccess;
using KahveNotum.BusinessLayer;
using KahveNotum.WebApp.Models;
using KahveNotum.WebApp.Filters;

namespace KahveNotum.WebApp.Controllers
{   [Auth]
    [AuthAdmin]
    [Exc]
    public class CategoryController : Controller
    {
        CategoryManager categoryManage = new CategoryManager();


        // GET: Category
        [Auth]
        public ActionResult Index()
        {
            return View(categoryManage.List());
        }

        // GET: Category/Details/5
        [Auth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManage.Find(x => x.ID == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Category/Create
        [Auth]
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Create(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                categoryManage.Insert(category);
                CacheHelper.RemoveCategoriesFromCache();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManage.Find(x => x.ID == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,CreatedOn,ModifiedOn,ModifiedUsername")] Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                // TODO: 
                Category cat = categoryManage.Find(x => x.ID == category.ID);
                cat.Title = category.Title;
                cat.Description = category.Description;

                categoryManage.Update(cat);
                CacheHelper.RemoveCategoriesFromCache();


                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Category/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManage.Find(x => x.ID == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = categoryManage.Find(x => x.ID == id);
            categoryManage.Delete(category);
            CacheHelper.RemoveCategoriesFromCache();
            return RedirectToAction("Index");
        }

    }
}
