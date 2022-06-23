using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace DbaReportsWebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly SettingsContext db;

        public HomeController(SettingsContext context) => db = context;

        public IActionResult Index()
        {
            var items = db.Setting
           .ToList();
            return View(items);
        }
    }
}