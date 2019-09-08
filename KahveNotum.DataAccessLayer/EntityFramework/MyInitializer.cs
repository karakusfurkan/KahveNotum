using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using KahveNotum.Entities;

namespace KahveNotum.DataAccessLayer.EntityFramework
{
   public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            //Adding admin
            KahveNotumUser admin = new KahveNotumUser()
            {
                Name = "Furkan",
                SurName = "Karakuş",
                Email = "karakus.afurkan@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "karakusfurkan",
                Password = "123123",
                ProfileImageFileName = "coffee.jpg",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "karakusfurkan"




            };

            KahveNotumUser standartUser = new KahveNotumUser()
            {
                Name = "Furkan",
                SurName = "Karakuş",
                Email = "karakus.afurkan@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "karakusfurkandeneme",
                Password = "123123",
                ProfileImageFileName = "coffee.jpg",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUsername = "karakusfurkan"

            };

            context.KahveNotumUsers.Add(admin);
            context.KahveNotumUsers.Add(standartUser);

            for (int i = 0; i < 8; i++)
            {
                KahveNotumUser fakeUser = new KahveNotumUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    SurName = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",
                    ProfileImageFileName = "coffee.jpg",
                    Password = "123",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"

                };
                context.KahveNotumUsers.Add(fakeUser);

            }
            //User list
            

            context.SaveChanges();

            List<KahveNotumUser> userlist = context.KahveNotumUsers.ToList();

            //Adding fake categories for test
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {

                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "karakusfurkan"



                };
                context.Categories.Add(cat);
                //Adding notes for all categories
                for (int k = 0; k < FakeData.NumberData.GetNumber(5,9); k++)
                {
                    KahveNotumUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(6, 24)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        Category = cat,
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username

                    };

                    cat.Notes.Add(note);
                    //Adding comments for all notes

                    for (int j = 0; j < FakeData.NumberData.GetNumber(2,6); j++)
                    {
                        KahveNotumUser comment_owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = comment_owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = comment_owner.Username





                        };

                        note.Comments.Add(comment);


                    }


                    
                    //Adding fake likes
                    for (int m = 0; m < note.LikeCount; m++)
                    {

                        Liked liked = new Liked()
                        {
                            LikedUser = userlist[m]


                        };
                        note.Likes.Add(liked);


                    }
                    

                }


            }

            context.SaveChanges();



        }




    }
}
