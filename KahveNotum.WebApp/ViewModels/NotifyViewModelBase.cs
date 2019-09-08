﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KahveNotum.WebApp.ViewModels
{
    public class NotifyViewModelBase<T>
    {
        public List<T> Items { get; set; }
        public string Header{ get; set; }
        public string Title { get; set; }
        public bool IsRedirecting { get; set; }
        public string RedirectingUrl { get; set; }
        public int RedirectingTimeOut { get; set; }

        public NotifyViewModelBase()
        {
            Header = "Yönlendiriliyorsunuz.";
            Title = "Geçersiz işlem";
            IsRedirecting = true;
            RedirectingUrl = "../../Home/Index";
            RedirectingTimeOut = 10000;


        }




    }
}