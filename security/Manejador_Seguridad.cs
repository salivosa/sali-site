using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Security
{
    public class Manejador_Seguridad
    {
        private static SecurityEntities db_seguridad { get; set; }
        public Manejador_Seguridad()
        {
            db_seguridad = new SecurityEntities();
        }

        public bool confirmar_logueo(string user, string password)
        {
            string pass_encriptado = encriptar_pass(password);
            var check = db_seguridad.users.Any(x => x.username == user && x.password == pass_encriptado && x.active == true);
            return check;
        }

        private string encriptar_pass(string entrada)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(entrada);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public IQueryable<user> datos_usuario(string user)
        {
            var datos = db_seguridad.users.Where(x => x.username == user);
            return datos;
        }

        public Dictionary<string, Tuple<string, string, string, string>> obtener_modulos(string username)
        {
            var listado = db_seguridad.permissions.Where(x => x.user.username == username && x.status == true).ToDictionary(o => o.module.module_name, w => new Tuple<string, string, string, string>(w.module.description, w.module.route, w.module.icon, w.user.name + " " + w.user.surname));
            return listado;
        }

        public void actualizar_tiempo_logging(string user)
        {
            var user_instance = db_seguridad.users.Find(datos_usuario(user).FirstOrDefault().ID_user);
            user_instance.last_logging = DateTime.Now;
            db_seguridad.SaveChanges();
        }

    }
}
