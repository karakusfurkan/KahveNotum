using KahveNotum.BusinessLayer.Abstract;
using KahveNotum.DataAccessLayer.EntityFramework;
using KahveNotum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KahveNotum.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        public override int Delete(Category category)
        {

            NoteManager noteManager = new NoteManager();
//TODO:            //LikedManager likedManager = new LikedManager();
            //CommentManager commentManager = new CommentManager();


            foreach (Note note in category.Notes.ToList())
            {

                foreach (Liked like in note.Likes.ToList())
                {
                    //TODO:    likedManager.Delete(like);
                }
                foreach (Comment comment in note.Comments.ToList())
                {
                    //TODO:  commentManager.Delete(comment);
                }



                noteManager.Delete(note);

            }



            return base.Delete(category);
        }




    }
}
