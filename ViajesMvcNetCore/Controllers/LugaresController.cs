using Microsoft.AspNetCore.Mvc;
using ViajesMvcNetCore.Models;
using ViajesMvcNetCore.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ViajesMvcNetCore.Data;
using MvcNetCoreUtilidades.Helpers;

namespace ViajesMvcNetCore.Controllers
{
    public class LugaresController : Controller
    {
        private RepositoryLugar repo;
        private readonly ViajesContext context;
        private readonly HelperPathProvider helperPath;



        public LugaresController(ViajesContext context, RepositoryLugar repo, HelperPathProvider helperPath)
        {
            this.context = context;
            this.repo = repo;
            this.helperPath = helperPath;
        }

        public async Task<IActionResult> Index(string tipo)
        {
            List<Lugar> lugares;
            if (string.IsNullOrEmpty(tipo))
            {
                lugares = await this.repo.GetLugaresAsync();
            }
            else
            {
                lugares = await this.repo.GetLugaresPorTipoAsync(tipo);
            }

            return View(lugares);
        }


        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index");
            }

            List<Lugar> lugaresEncontrados = await this.repo.FindLugarByNameAsync(searchTerm);
            return View("Index", lugaresEncontrados);
        }

        public async Task<IActionResult> Details(int idlugar)
        {
            Lugar lugar = await this.repo.FindLugarAsync(idlugar);
            if (lugar == null)
            {
                return NotFound();
            }

            List<Comentario> comentarios = await this.repo.GetComentariosLugarAsync(idlugar);
            ViewBag.Comentarios = comentarios;
            return View(lugar);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string nombre, string descripcion, string ubicacion, string categoria, DateTime horario, string tipo, IFormFile fichero)
        {
            // Comprobar si el usuario está autenticado
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (!idUsuario.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            // Crear variable para almacenar la URL de la imagen
            string imagenUrl = null;

            // Si se ha subido un archivo, proceder con la carga de la imagen
            if (fichero != null && fichero.Length > 0)
            {
                // Nombre del archivo
                string fileName = fichero.FileName;

                // Ruta donde se almacenará el archivo (en el servidor)
                string path = this.helperPath.MapPath(fileName, Folders.Uploads);

                // URL para acceder al archivo una vez cargado (en el navegador)
                imagenUrl = this.helperPath.MapUrlPath(fileName, Folders.Uploads);

                // Guardar el archivo en el servidor
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    await fichero.CopyToAsync(stream);
                }
            }

            // Insertar el lugar en la base de datos
            await this.repo.InsertLugarAsync(nombre, descripcion, ubicacion, categoria, horario, imagenUrl, tipo, idUsuario.Value);

            // Redirigir al usuario a la vista de lista de lugares (Index)
            return RedirectToAction("Index");
        }




        public async Task<IActionResult> Comentarios(int id)
        {
            var comentarios = await this.repo.GetComentariosLugarAsync(id);
            ViewBag.LugarId = id;
            return View(comentarios);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(int idlugar, string comentario)
        {
            // Obtener el IdUsuario de la sesión
            int? idusuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idusuario.HasValue)
            {
                await this.repo.AddComentarioAsync(idlugar, idusuario.Value, comentario);
                return RedirectToAction("Details", new { idlugar = idlugar });
            }
            else
            {
                // Si el usuario no está autenticado, redirige a la página de inicio de sesión.
                return RedirectToAction("Home", "Login");
            }
        }

        public async Task<IActionResult> _Comentarios(int idlugar)
        {
            List<Comentario> comentarios = await this.repo.GetComentariosLugarAsync(idlugar);
            return PartialView("_Comentarios", comentarios);
        }

        
        [HttpPost]
        public async Task<IActionResult> DeleteComentario(int idComentario, int idLugar)
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario.HasValue)
            {
                var comentarios = await this.repo.GetComentariosLugarAsync(idLugar);
                var comentario = comentarios.Find(com => com.IdComentario == idComentario);

                if (comentario != null && comentario.IdUsuario == idUsuario.Value)
                {
                    await this.repo.DeleteComentarioAsync(idComentario);
                }
                else
                {
                    return Forbid();
                }
            }

            return RedirectToAction("Details", new { idlugar = idLugar });
        }
        [HttpPost]
        public async Task<IActionResult> AddFavorito(int idlugar)
        {
            // Obtener el IdUsuario de la sesión
            int? idusuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idusuario.HasValue)
            {
                // Verificar si el lugar ya está en los favoritos del usuario
                var favoritoExistente = await repo.ExisteFavoritoAsync(idusuario.Value, idlugar);

                if (favoritoExistente)
                {
                    // Si ya está en favoritos, mostrar un mensaje de error
                    ViewData["FavoritoError"] = "Este lugar ya está en tus favoritos.";
                    return RedirectToAction("Index");
                }

                // Obtener el lugar con el idlugar proporcionado
                var lugar = await repo.FindLugarAsync(idlugar);

                if (lugar != null)
                {
                    // Llamar al método para agregar el lugar a los favoritos
                    await this.repo.AddFavoritoAsync(
                        idusuario.Value,
                        idlugar,
                        DateTime.Now,
                        lugar.Imagen,
                        lugar.Nombre,
                        lugar.Descripcion,
                        lugar.Ubicacion,
                        lugar.Tipo
                    );

                    // Usar ViewData para enviar un mensaje de éxito
                    ViewData["FavoritoSuccess"] = "¡Lugar guardado como favorito!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["FavoritoError"] = "Lugar no encontrado.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                // Si el usuario no está autenticado, redirige a la página de inicio de sesión
                ViewData["FavoritoError"] = "Debes iniciar sesión para guardar este lugar como favorito.";
                return RedirectToAction("Login", "Account");
            }
        }

    }
}
