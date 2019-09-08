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
using KahveNotum.WebApp.Models;
using KahveNotum.WebApp.Filters;

namespace KahveNotum.WebApp.Controllers
{
    [Exc]
    public class NoteController : Controller
    {



        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private LikedManager likedManager = new LikedManager();

        // GET: Note
        public ActionResult Index()
        {



            var notes = noteManager.ListQueryable().Include("Category").Include("Owner")
                .Where(x => x.Owner.ID == CurrentSession.User.ID)
                .OrderByDescending(x => x.ModifiedOn);
            return View(notes.ToList());
        }

       public ActionResult MyLikedNotes()
        {
            var notes = likedManager.ListQueryable().Include("LikedUser")
                .Where(x => x.LikedUser.ID == CurrentSession.User.ID)
                .Select(x => x.Note).Include("Category")
                .OrderByDescending(x => x.ModifiedOn);

            return View("MyLikedNotes", notes.ToList());

        }


        // GET: Note/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.ID == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Note/Create
        [Auth]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(CacheHelper.GetCategoriesFromCache(), "ID", "Title");
            return View();
        }

        // POST: Note/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Create(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                note.Owner = CurrentSession.User;
                noteManager.Insert(note);
                return RedirectToAction("Index");
            }
            
            ViewBag.CategoryID = new SelectList(CacheHelper.GetCategoriesFromCache(), "ID", "Title", note.CategoryID);
            return View(note);
        }

        // GET: Note/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.ID == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(CacheHelper.GetCategoriesFromCache(), "ID", "Title", note.CategoryID);
            return View(note);
        }

        // POST: Note/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Edit(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                Note db_note = noteManager.Find(x => x.ID == note.ID);
                db_note.IsDraft = note.IsDraft;
                db_note.CategoryID = note.CategoryID;
                db_note.Text = note.Text;
                db_note.Title = note.Title;
                noteManager.Update(db_note);

                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(CacheHelper.GetCategoriesFromCache(), "ID", "Title", note.CategoryID);
            return View(note);
        }

        // GET: Note/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.ID == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Note/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = noteManager.Find(x => x.ID == id);
            noteManager.Delete(note);
            return RedirectToAction("Index");
        }

       [HttpPost]
       [Auth]
        public ActionResult GetLiked(int[] ids)
        {
            List<int> likedNoteIds = likedManager.List(x => x.LikedUser.ID == CurrentSession.User.ID && ids.Contains(x.Note.ID)).Select(x => x.Note.ID).ToList();

            return Json(new{ result = likedNoteIds });


        }


        public ActionResult SetLikeState(int noteid, bool liked)
        {
            
            int res = 0;
            Liked like = likedManager.Find(x => x.Note.ID == noteid && x.LikedUser.ID == CurrentSession.User.ID);
            Note note = noteManager.Find(x => x.ID == noteid);

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (like != null && liked == false)
            {
                res = likedManager.Delete(like);


            }
            else if (like == null && liked == true)
            {
               res = likedManager.Insert(new Liked()
                {
                    LikedUser = CurrentSession.User,
                    Note = note,
                });
            }

            if (res > 0)
            {
                if (liked)
                {
                    note.LikeCount++;

                }
                else
                {
                    note.LikeCount--;

                }
                res = noteManager.Update(note);
                return Json(new { hasError = false, errorMessage = string.Empty,result = note.LikeCount});
            }
            return Json(new { hasError = true, errorMesage = "Beğenme işlemi gerçekleştirilemedi.", result = note.LikeCount });

        }


    }
}
