using KahveNotum.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KahveNotum.Entities;
using KahveNotum.WebApp.Models;

namespace KahveNotum.WebApp.init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            KahveNotumUser user = CurrentSession.User;

            if (user != null)
            {
                return user.Username;

            }
            else
            {
                return "system";
            }

        }
    }
}