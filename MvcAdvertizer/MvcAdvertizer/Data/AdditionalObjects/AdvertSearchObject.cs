using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel;

namespace MvcAdvertizer.Data.AdditionalObjects
{
    public class AdvertSearchObject
    {   
        [BindRequired]
        [DisplayName("Пользователь")]
        public Guid? UserId { get; set; }

        [BindRequired]
        public string SearchStringQuery { get; set; }
                
        private DateTime dateStartSearch = DateTime.Now.AddDays(-100);        
        public DateTime DateStartSearch { get => dateStartSearch; set { if (value != null) dateStartSearch = value; } }
        public string DateStartSearchString { get => DateStartSearch.ToString("yyyy-MM-dd"); }

        private DateTime dateEndSearch = DateTime.Now.AddDays(1);
        public DateTime DateEndSearch { get => dateEndSearch; set { if (value != null) dateEndSearch = value; } }
        public string DateEndSearchString { get => DateEndSearch.ToString("yyyy-MM-dd"); }

    }
}
