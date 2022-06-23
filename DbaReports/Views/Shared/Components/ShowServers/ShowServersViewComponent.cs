using DbaRC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

public class ShowServersViewComponent : ViewComponent
{
    private readonly SettingsContext db;

    public ShowServersViewComponent(SettingsContext context) => db = context;
    //public ShowServersViewComponent(SettingsContext context)
    //{
    //    db = context;
    //}

    public IViewComponentResult Invoke()
    {
        var items = db.Setting
            .ToList();
        return View("Default", items);
    }
}