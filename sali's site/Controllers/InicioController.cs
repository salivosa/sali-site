using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Security;
using sali_site.Controllers;
using sali_site.Models;

namespace sali_site.Controllers
{
    public class InicioController : Controller
    {
        private static Manejador_Seguridad seguridad { get; set; }
        private static string usuario_logeado { get; set; }
        public ActionResult Selección()
        {
            seguridad = new Manejador_Seguridad();

            return View();
        }
        
        public JsonResult login(string user, string password)
        {
            var check = seguridad.confirmar_logueo(user, password);

            if (check)
            {
                usuario_logeado = user;
                seguridad.actualizar_tiempo_logging(usuario_logeado);
            }

            return Json(check, JsonRequestBehavior.AllowGet);
        }

        public JsonResult log_off()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(new AuthenticationProperties { IsPersistent = false });

            return Json("/Inicio/Selección", JsonRequestBehavior.AllowGet);
        }

        public JsonResult modulos_disponibles()
        {
            if (usuario_logeado == null)
                usuario_logeado = User.Identity.Name;

            var datos_usuario = seguridad.datos_usuario(usuario_logeado);
            
            var nombre_completo = datos_usuario.Select(x => x.name + " " + x.surname).FirstOrDefault();
            var fecha_registo = datos_usuario.FirstOrDefault().registration_date.ToString("dd/MM/yyyy");
            var icon = datos_usuario.FirstOrDefault().icon;

            var modulos = seguridad.obtener_modulos(usuario_logeado);

            var ident = new ClaimsIdentity(new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, modulos.Select(x=>x.Value.Item4).FirstOrDefault()),
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                    new Claim(ClaimTypes.Name,usuario_logeado),
                    }, DefaultAuthenticationTypes.ApplicationCookie);

            foreach (var modulo in modulos)
                ident.AddClaim(new Claim(ClaimTypes.Role, modulo.Key));

            if (User.Identity.IsAuthenticated)
                HttpContext.GetOwinContext().Authentication.SignOut(new AuthenticationProperties { IsPersistent = false });

            HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);

            var datos_json = modulos.Select(x => new module_model() { module_name = x.Key, module_description = x.Value.Item1, module_route = x.Value.Item2, module_icon = x.Value.Item3 }).ToList();

            return Json(new { success = true, datos_modulo = datos_json, usuario = usuario_logeado, fullname = nombre_completo, registration_date = fecha_registo, user_icon = icon }, JsonRequestBehavior.AllowGet);
        }
    }
}