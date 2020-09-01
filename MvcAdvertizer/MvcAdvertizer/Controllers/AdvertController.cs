using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MvcAdvertizer.Controllers
{
    public class AdvertController : Controller
    {
        public IActionResult Create() {
            return View();
        }
    }
}
