using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sali_site.Controllers
{
    public class CompartidoController : Controller
    {
        [Authorize(Roles = "galeria_capturas")]
        public ActionResult Capturas()
        {
            return View();
        }

        [Authorize(Roles = "listado")]
        public ActionResult Listado()
        {
            return View();
        }
    }
}