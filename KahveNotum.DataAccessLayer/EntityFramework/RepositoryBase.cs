using KahveNotum.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KahveNotum.DataAccessLayer.EntityFramework
{
    public class RepositoryBase
    {
        protected static DatabaseContext context;
        private static object _lockSync = new object();

        protected RepositoryBase()
        {
            CreateContext();
            //Sadece miras alan newleyebilir.
        }


        private static void CreateContext()
        {
            if (context == null)
            {

                lock (_lockSync)
                {
                    if (context == null)
                    {
                        context = new DatabaseContext();
                    }
                    
                }

            }
            
        }




    }
}
