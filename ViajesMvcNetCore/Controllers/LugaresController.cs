using Microsoft.AspNetCore.Mvc;
using ViajesMvcNetCore.Models;
using ViajesMvcNetCore.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ViajesMvcNetCore.Data;

namespace ViajesMvcNetCore.Controllers
{
    public class LugaresController : Controller
    {
        private RepositoryLugar repo;
        private readonly ViajesContext context;


        public LugaresController(ViajesContext context, RepositoryLugar repo)
        {
            this.context = context;
            this.repo = repo;
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
        public async Task<IActionResult> Create(string nombre, string descripcion, string ubicacion, string categoria, DateTime horario, string imagen, string tipo)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (!idUsuario.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            await this.repo.InsertLugarAsync(nombre, descripcion, ubicacion, categoria, horario, imagen, tipo, idUsuario.Value);

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

        public async Task<IActionResult> EditComentario(int idComentario)
        {
            // Obtener el comentario por id
            var comentario = await this.context.Comentarios.FindAsync(idComentario);

            if (comentario == null)
            {
                return NotFound();
            }

            // Solo lo mostramos en la vista si es válido
            return View(comentario);
        }

        [HttpPost]
        public async Task<IActionResult> EditComentario(Comentario comentario)
        {

                comentario.FechaComentario = DateTime.Now;

                await repo.UpdateComentarioAsync(comentario.IdComentario, comentario.IdLugar,
                    comentario.IdUsuario, comentario.Comentarios, comentario.FechaComentario);

                if (HttpContext.Session.GetInt32("IdUsuario") != null)
                {
                    return RedirectToAction("Perfil", new { id = HttpContext.Session.GetInt32("IdUsuario") });
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            

            return View(comentario);
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
