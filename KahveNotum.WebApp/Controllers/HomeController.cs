using KahveNotum.BusinessLayer;
using KahveNotum.BusinessLayer.Results;
using KahveNotum.Entities;
using KahveNotum.Entities.Messages;
using KahveNotum.Entities.ValueObjects;
using KahveNotum.WebApp.Filters;
using KahveNotum.WebApp.Models;
using KahveNotum.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KahveNotum.WebApp.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        CommentManager commentManager = new CommentManager();
        // GET: Home
        public ActionResult Index()
        {

            //if (TempData["mm"]!= null)
            //{
            //    return View(TempData["mm"] as List<Note>);
            //}
            NoteManager nm = new NoteManager();
            return View(nm.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
            //return View(nm.GetAllQuearyble().OrderByDescending(x => x.ModifiedOn).ToList());

            
        }


        public ActionResult ByCategory(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            CategoryManager cm = new CategoryManager();
            Category cat = cm.Find(x=> x.ID ==id.Value);

            if (cat == null)
            {
                return HttpNotFound();
                //return RedirectToAction("Index", "Home");
            }

            return View("Index",cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }


        public ActionResult MostLiked()
        {

            NoteManager nm = new NoteManager();
            return View("Index",nm.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());

        }
        [Auth]
        public ActionResult ShowProfile()
        {

            UserManager um = new UserManager();
            BusinessLayerResult<KahveNotumUser> res = um.GetUserByID(CurrentSession.User.ID);

            if (res.Errors.Count >0)
            {
                //TODO: Show an error page to user....
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);


            }



            return View(res.Result);

        }

        
        [Auth]
        public ActionResult EditProfile()
        {
          
            UserManager um = new UserManager();
            BusinessLayerResult<KahveNotumUser> res = um.GetUserByID(CurrentSession.User.ID);

            if (res.Errors.Count > 0)
            {
                //TODO: Show an error page to user....
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);


            }


            return View(res.Result);
        }

        [HttpPost]
        [Auth]
        public ActionResult EditProfile(KahveNotumUser model, HttpPostedFileBase ProfileImage)
        {

            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                if(ProfileImage != null &&
                     (ProfileImage.ContentType == "image/jpeg" ||
                     ProfileImage.ContentType == "image/jpg" ||
                     ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.ID}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFileName = filename;
                }
                UserManager eum = new UserManager();
                BusinessLayerResult<KahveNotumUser> res = eum.UpdateProfile(model);

                if (res.Errors.Count() > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil güncellenmesinde hata oluştu.",
                        RedirectingUrl = "../../Home/EditProfile"
                    };
                    return View("Error", errorNotifyObj);

                }

                CurrentSession.Set<KahveNotumUser>("login", res.Result);
                return RedirectToAction("ShowProfile");


            }

            return View(model);

        }

        
        [Auth]
        public ActionResult DeleteProfile()
        {

            
            UserManager um = new UserManager();
            BusinessLayerResult<KahveNotumUser> res = um.RemoveUserByID(CurrentSession.User.ID);

            if (res.Errors.Count >0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi.",
                    RedirectingUrl = "../../Home/ShowProfile"
                };

                return View("Error", errorNotifyObj);


            }
            Session.Clear();

            return RedirectToAction("Index");

        }


        public ActionResult Login()
        {
            return View();

        }





        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

            //Login control and redirect
            UserManager eum = new UserManager();
            BusinessLayerResult<KahveNotumUser> res = eum.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == Entities.Messages.ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "E-Posta Gönder";

                    }


                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));


                    return View(model);
                }

                CurrentSession.Set<KahveNotumUser>("login", res.Result);
                return RedirectToAction("Index");

            }



            return View(model);

        }


        public ActionResult Register()
        {
            return View();

        }


        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            //Username exist control
            //Eposta exist control
            //Register
            //Activation post
            if (ModelState.IsValid)
            {
                UserManager eum = new UserManager();
                  BusinessLayerResult<KahveNotumUser> res = eum.RegisterUser(model);

                if (res.Errors.Count >0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                //try
                //{
                //   user = eum.RegisterUser(model);
                //}
                //catch (Exception ex)
                //{

                //    ModelState.AddModelError("", ex.Message);
                //}

                //if (user == null)
                //{
                //    return View(model);
                //}


                return RedirectToAction("RegisterOK");


            }



            return View();

        }


        public ActionResult RegisterOK()
        {
            return View();

        }


        public ActionResult UserActivate(Guid id)
        {
            UserManager eum = new UserManager();
            BusinessLayerResult<KahveNotumUser> res = eum.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                TempData["errors"] = res.Errors;
                return RedirectToAction("UserActivateCancel");

            }


            return RedirectToAction("UserActivateOK");

        }

        public ActionResult UserActivateOK()
        {

            return View();

        }

        public ActionResult UserActivateCancel()
        {
            if (TempData["errors"] != null)
            {
                List<ErrorMessageObj> errors = TempData["errors"] as List<ErrorMessageObj>;
            }


            return View();

        }

        public ActionResult Logout()
        {


            Session.Clear();

            return RedirectToAction("Index");

        }

        public ActionResult Administration()
        {

            return View();

        }

        public ActionResult ShowNote(int? id)
        {
            NoteManager noteManager = new NoteManager();

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
        [HttpPost]
        public ActionResult ShowNote(int? id, string text)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = commentManager.Find(x => x.ID == id);
            if (comment == null)
            {
                return new HttpNotFoundResult();
            }

            comment.Text = text;
            if (commentManager.Update(comment)>0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ShowNote2(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = commentManager.Find(x => x.ID == id);
            if (comment == null)
            {
                return new HttpNotFoundResult();
            }

            
            if (commentManager.Delete(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ShowNote3(Comment comment,int? noteID)
        {

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                if (noteID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                NoteManager noteManager = new NoteManager();
                Note note = noteManager.Find(x => x.ID == noteID);


                if (note == null)
                {
                    return new HttpNotFoundResult();
                }

                comment.Note = note;
                comment.Owner = CurrentSession.User;
                if (commentManager.Insert(comment) > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);

                }

            }
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);


        }

        //public ActionResult ShowComments(int? id)
        //{

        //    UserManager userManager = new UserManager();
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    return View();


        //}

        public ActionResult About()
        {
            return View();

        }
        

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult HasError()
        {
            return View();

        }

    }
}