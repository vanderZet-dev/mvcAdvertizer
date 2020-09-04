using Microsoft.AspNetCore.Mvc.Rendering;
using MvcAdvertizer.Data.Models;

namespace MvcAdvertizer.ViewModels
{
    public class CreateAdvertViewModel
    {
        public SelectList UserSelectList { get; set; }       

        public Advert Advert { get; set; }          
    }
}
