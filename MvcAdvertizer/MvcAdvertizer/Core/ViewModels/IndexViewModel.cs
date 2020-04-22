using MvcAdvertizer.Config;
using MvcAdvertizer.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MvcAdvertizer.Core.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Advert> Adverts { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }

        public SelectList Users { get; set; }
        public string Number { get; set; }
    }
}
