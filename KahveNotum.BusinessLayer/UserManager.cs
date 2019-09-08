using KahveNotum.BusinessLayer.Abstract;
using KahveNotum.BusinessLayer.Results;
using KahveNotum.Common.Helpers;
using KahveNotum.DataAccessLayer.EntityFramework;
using KahveNotum.Entities;
using KahveNotum.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KahveNotum.BusinessLayer
{
    public class UserManager : ManagerBase<KahveNotumUser>
    {
       

        public BusinessLayerResult<KahveNotumUser> RegisterUser(RegisterViewModel data)
        {
            //Username exist control
            //Eposta exist control
            //Register
            //Activation post
           KahveNotumUser user =  Find(x => x.Username == data.Username || x.Email == data.EMail);
                BusinessLayerResult<KahveNotumUser> res = new BusinessLayerResult<KahveNotumUser>();
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                if (user.Email == data.EMail)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.EmailAlreadyExists, "E-mail zaten kayıtlı.");
                }
            }
            else
            {
                int dbResult = base.Insert(new KahveNotumUser()
                {
                    Username = data.Username,
                    Email = data.EMail,
                    ProfileImageFileName = "coffee.jpg",
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false,


                });

                if (dbResult > 0)
                {
                    res.Result = Find(x => x.Email == data.EMail && x.Username == data.Username);

                    //TODO: Activate mail
                    //layerResult.Result.ActivateGuid
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";


                    MailHelper.SendMail(body,res.Result.Email,"KahveNotum Hesap Aktifleştirme");



                }
            }

            return res;



        }

        public BusinessLayerResult<KahveNotumUser> GetUserByID(int id)
        {
            BusinessLayerResult<KahveNotumUser> res = new BusinessLayerResult<KahveNotumUser>();
            res.Result = Find(x => x.ID == id);

            if (res.Result == null)
            {
                res.AddError(Entities.Messages.ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı.");
            }
            return res;
        }

        public BusinessLayerResult<KahveNotumUser> LoginUser(LoginViewModel data)
        {
            // Login Control
            // Account activate control

            BusinessLayerResult<KahveNotumUser> res = new BusinessLayerResult<KahveNotumUser>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);

            
            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                    res.AddError(Entities.Messages.ErrorMessageCode.CheckYourEmail, "Lütfen e-posta adresinizi kontrol ediniz.");
                }



            }
            else
            {
                res.AddError(Entities.Messages.ErrorMessageCode.UsernameorPassWrong, "Kullanıcı adı ya da şifre uyuşmuyor.");
            }
            return res;



        }

        public BusinessLayerResult<KahveNotumUser> UpdateProfile(KahveNotumUser data)
        {
            KahveNotumUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<KahveNotumUser> res = new BusinessLayerResult<KahveNotumUser>();

            if (db_user != null && db_user.ID != data.ID)
            {


                if (db_user.Username == data.Username)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");

                }
                if (db_user.Email == data.Email)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");


                }
                return res;

            }

            res.Result = Find(x => x.ID == data.ID);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.SurName = data.SurName;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;



            if (string.IsNullOrEmpty(data.ProfileImageFileName) == false)
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;


            }
            else
            {
                res.Result.ProfileImageFileName = "/images/coffee.jpg";
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(Entities.Messages.ErrorMessageCode.UserCouldNotUpdated, "Profil güncellenmesinde hata oluştu.")
;           }

            return res;


        }

        public BusinessLayerResult<KahveNotumUser> RemoveUserByID(int ID)
        {
            BusinessLayerResult<KahveNotumUser> res = new BusinessLayerResult<KahveNotumUser>();
            KahveNotumUser user = Find(x => x.ID == ID);


            if (user != null)
            {
                if (Delete(user) == 0 )
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.UserCouldNotRemove, "Kullanıcı Silinemedi.");
                    return res;

                }

            }
            else
            {
                res.AddError(Entities.Messages.ErrorMessageCode.UserCouldNotFind, "Kullanıcı Bulunamadı.");



            }
            return res;


        }

        public BusinessLayerResult<KahveNotumUser> ActivateUser(Guid activateID)
        {

            BusinessLayerResult<KahveNotumUser> res = new BusinessLayerResult<KahveNotumUser>();
            res.Result = Find(x => x.ActivateGuid == activateID);
            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif.");
                    return res;

                }

                res.Result.IsActive = true;
                Update(res.Result);



            }
            else
            {
                res.AddError(Entities.Messages.ErrorMessageCode.ActivateIDDoesNotExist, "Aktifleştirilecek kullanıcı bulunamadı.");
            }



            return res;


        }


        public new BusinessLayerResult<KahveNotumUser> Insert(KahveNotumUser data)
        {
            //Method hiding
            KahveNotumUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<KahveNotumUser> res = new BusinessLayerResult<KahveNotumUser>();

            res.Result = data;


            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.EmailAlreadyExists, "E-mail zaten kayıtlı.");
                }
            }
            else
            {
                res.Result.ProfileImageFileName = "coffee.jpg";
                res.Result.ActivateGuid = Guid.NewGuid();

                if (base.Insert(res.Result) == 0 )
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.UserCouldNotInserted, "Kullanıcı Eklenemedi.");
                }


            
            }

            return res;





        }


        public new BusinessLayerResult<KahveNotumUser> Update(KahveNotumUser data)
        {
            KahveNotumUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<KahveNotumUser> res = new BusinessLayerResult<KahveNotumUser>();

            if (db_user != null && db_user.ID != data.ID)
            {


                if (db_user.Username == data.Username)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");

                }
                if (db_user.Email == data.Email)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");


                }
                return res;

            }

            res.Result = Find(x => x.ID == data.ID);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.SurName = data.SurName;
            res.Result.Password = data.Password;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;
            res.Result.Username = data.Username;


            if (base.Update(res.Result) == 0)
            {
                res.AddError(Entities.Messages.ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenmesinde hata oluştu.")
;
            }

            return res;

        }

    }
}
