using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KahveNotum.Common.Helpers
{
    public class ConfigHelper
    {
        public static T Get<T>(String key)
        {
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));

        }

      
    }
}
