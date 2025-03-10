using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using System.Drawing;
using ViajesMvcNetCore.Models;
using ViajesMvcNetCore.Data;
using Microsoft.Data.SqlClient;
using ViajesMvcNetCore.Repositories;

namespace AvatarDinamicoPersonalizado.Controllers
{
    public class HomeController : Controller
    {
        private readonly ViajesContext context;
        private readonly RepositoryHome repo;

        public HomeController(ViajesContext context, RepositoryHome repo)
        {
            this.context = context;
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        private void GuardarSesion(UsuarioCompletoViewModel usuario)
        {
            HttpContext.Session.SetString("NombreUsuario", usuario.Nombre);
            HttpContext.Session.SetString("CorreoUsuario", usuario.CorreoLogin);
            HttpContext.Session.SetString("ClaveUsuario", usuario.Clave);
            HttpContext.Session.SetString("ConfirmarClaveUsuario", usuario.ConfirmarClave);
            HttpContext.Session.SetString("PreferenciaViajeUsuario", usuario.PreferenciaViaje);
            HttpContext.Session.SetString("AvatarUrlUsuario", usuario.AvatarUrl);
            HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);
            HttpContext.Session.SetInt32("EdadUsuario", usuario.Edad);
            HttpContext.Session.SetString("NacionalidadUsuario", usuario.Nacionalidad);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            try
            {
                if (!string.IsNullOrEmpty(login.Clave))
                {
                    var usuario = await context.UsuarioCompletoViewModels.FirstOrDefaultAsync(u => u.CorreoLogin == login.Email && u.Clave == login.Clave);

                    if (usuario != null)
                    {
                        if (usuario.IdUsuario != 0)
                        {
                            GuardarSesion(usuario);
                            return RedirectToAction("Perfil", new { id = usuario.IdUsuario });
                        }
                        else
                        {
                            ViewBag.Error = "IdUsuario no encontrado en la base de datos.";
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Correo o contraseña incorrectos.";
                    }
                }
                else
                {
                    ViewBag.Error = "La contraseña no puede estar vacía.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error durante el inicio de sesión: " + ex.Message;
                Console.WriteLine(ex.ToString());
            }

            return View(login);
        }

        private string GetIniciales(string nombre)
        {
            string[] palabras = nombre.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string iniciales = "";

            foreach (var palabra in palabras)
            {
                iniciales += char.ToUpper(palabra[0]);
                if (iniciales.Length == 2) break;
            }

            return iniciales;
        }

        private byte[] GenerarAvatar(string iniciales, string colorHex)
        {
            int ancho = 150, alto = 150;

            Bitmap bitmap = new Bitmap(ancho, alto);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Color color = ColorTranslator.FromHtml(colorHex);

                g.Clear(color);

                Font fuente = new Font("Arial", 50, FontStyle.Bold);

                Brush textoBlanco = Brushes.White;

                SizeF tamano = g.MeasureString(iniciales, fuente);

                float x = (ancho - tamano.Width) / 2;
                float y = (alto - tamano.Height) / 2;

                g.DrawString(iniciales, fuente, textoBlanco, x, y);
            }

            using MemoryStream ms = new MemoryStream();

            bitmap.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        public IActionResult CrearCuenta()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearCuenta(UsuarioLogin usuario)
        {
            HttpContext.Session.Clear();

            if (ModelState.IsValid)
            {
                usuario.Nombre = usuario.Nombre.ToUpper();

                string iniciales = GetIniciales(usuario.Nombre);
                byte[] imagenAvatar = GenerarAvatar(iniciales, usuario.ColorAvatar);
                string carpetaAvatar = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/avatars");

                if (!Directory.Exists(carpetaAvatar))
                {
                    Directory.CreateDirectory(carpetaAvatar);
                }

                string nombreAvatar = $"{Guid.NewGuid()}.png";
                string nombreArchivo = Path.Combine(carpetaAvatar, nombreAvatar);

                System.IO.File.WriteAllBytes(nombreArchivo, imagenAvatar);
                usuario.AvatarUrl = $"/avatars/{nombreAvatar}";

                try
                {
                    await context.Database.ExecuteSqlRawAsync("EXEC SP_INSERT_USER @nombre, @correo, @clave, @confirmarclave, @preferenciasdeviaje, @coloravatar, @avatarurl",
                        new SqlParameter("@nombre", usuario.Nombre),
                        new SqlParameter("@correo", usuario.Correo),
                        new SqlParameter("@clave", usuario.Clave),
                        new SqlParameter("@confirmarclave", usuario.ConfirmarClave),
                        new SqlParameter("@preferenciasdeviaje", usuario.PreferenciaViaje),
                        new SqlParameter("@coloravatar", usuario.ColorAvatar),
                        new SqlParameter("@avatarurl", usuario.AvatarUrl));
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Ocurrió un error al crear la cuenta: " + ex.Message;
                    return View(usuario);
                }

                var usuarioCreado = await context.UsuarioCompletoViewModels.FirstOrDefaultAsync(u => u.CorreoLogin == usuario.Correo);

                if (usuarioCreado == null)
                {
                    ViewBag.Error = "No se pudo obtener el ID del usuario creado.";
                    return View(usuario);
                }

                GuardarSesion(usuarioCreado);
                return RedirectToAction("Perfil", new { id = usuarioCreado.IdUsuario });
            }
            return View(usuario);
        }


        public async Task<IActionResult> Perfil(int id)
        {
            var usuario = await this.context.UsuariosLogin.FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            var avatarUrl = HttpContext.Session.GetString("AvatarUrlUsuario");

            if (!string.IsNullOrEmpty(nombreUsuario))
            {
                usuario.Nombre = nombreUsuario;
                usuario.AvatarUrl = avatarUrl;
            }


            return View(usuario);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("NombreUsuario");
            HttpContext.Session.Remove("CorreoUsuario");
            HttpContext.Session.Remove("ClaveUsuario");
            HttpContext.Session.Remove("ConfirmarClaveUsuario");
            HttpContext.Session.Remove("PreferenciaViajeUsuario");
            HttpContext.Session.Remove("AvatarUrl");
            HttpContext.Session.Remove("IdUsuario");

            return RedirectToAction("Login", "Home");
        }

        public async Task<IActionResult> _Lugares(int idUsuario)
        {
           
            List<Lugar> lugares = await this.repo.GetLugaresPorUsuarioAsync(idUsuario);
            return PartialView("_Lugares", lugares);
        
        }

        public async Task<IActionResult> _Favoritos(int idUsuario)
        {

            List<LugarFavorito> favoritos = await this.repo.GetFavoritosLugarAsync(idUsuario);
            return PartialView("_Favoritos", favoritos);

        }

        public async Task<IActionResult> EditLugar(int idLugar)
        {
            var lugar = await context.Lugares.FindAsync(idLugar);
            if (lugar == null)
            {
                return NotFound();
            }
            return View(lugar);
        }

        [HttpPost]
        public async Task<IActionResult> EditLugar(Lugar lugar)
        {      
            await repo.UpdateLugarAsync(lugar.IdLugar, lugar.Nombre, 
                lugar.Descripcion, lugar.Ubicacion, lugar.Categoria, 
                lugar.Horario, lugar.Imagen, lugar.Tipo);

            if (HttpContext.Session.GetInt32("IdUsuario") != null)
            {
                return RedirectToAction("Perfil", new { id = HttpContext.Session.GetInt32("IdUsuario") });
            }
            else
            {
                return RedirectToAction("Login", "Home"); 
            }
        }

        public async Task<IActionResult> EditarPerfil(int idUsuario)
        {
            var usuario = await context.UsuarioCompletoViewModels.FindAsync(idUsuario);  
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> EditarPerfil(UsuarioCompletoViewModel usuario)
        {

                try
                {
                    await repo.UpdatePerfilAsync(
                        usuario.IdUsuario,
                        usuario.Nombre,
                        usuario.CorreoLogin,
                        usuario.Clave,
                        usuario.ConfirmarClave,
                        usuario.PreferenciaViaje,
                        usuario.ColorAvatar,
                        usuario.AvatarUrl,
                        usuario.Edad,
                        usuario.Nacionalidad
                    );

                    GuardarSesion(usuario);

                    return RedirectToAction("Perfil", new { id = usuario.IdUsuario });
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Ocurrió un error al actualizar el perfil: " + ex.Message;
                    return View(usuario);
                }
            
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int idLugar)
        {
            await repo.DeleteLugarAsync(idLugar);
            return RedirectToAction("_Lugares");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFavorito(int idlugar)
        {
            int? idusuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idusuario.HasValue)
            {
                await this.repo.DeleteFavoritoAsync(idusuario.Value, idlugar);
            }
            return RedirectToAction("_Favoritos");

        }

        public async Task<IActionResult> _Seguidos(int idusuario)
        {

            List<UsuarioSeguidoPerfil> seguido = await this.repo.GetSeguidoresUsuarioAsync(idusuario);
            return PartialView("_Seguidos", seguido);
        }
    }
}