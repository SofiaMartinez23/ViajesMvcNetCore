using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViajesMvcNetCore.Data;
using ViajesMvcNetCore.Models;
using ViajesMvcNetCore.Repositories;

namespace ViajesMvcNetCore.Controllers
{
    public class UsuariosController : Controller
    {
        private RepositoryUsuarios repo;
        private readonly ViajesContext context;


        public UsuariosController(ViajesContext context, RepositoryUsuarios repo)
        {
            this.context = context;
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var idUsuarioSesion = HttpContext.Session.GetInt32("IdUsuario");

            var usuariosCompletos = await context.UsuarioCompletoViewModels
                .Where(u => u.IdUsuario != idUsuarioSesion)
                .ToListAsync();

            return View(usuariosCompletos);
        }

        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index");
            }

            List<Usuario> usuariosEncontrados = await this.repo.FindUsuarioByNameAsync(searchTerm);
            return View("Index", usuariosEncontrados);
        }

        public async Task<IActionResult> PerfilUser(int idusuario)
        {
            // Obtener el usuario de la base de datos
            var usuario = await this.context.UsuariosLogin.FirstOrDefaultAsync(u => u.IdUsuario == idusuario);

            if (usuario == null)
            {
                return NotFound();
            }

            HttpContext.Session.SetString("AvatarUrlUsuario", usuario.AvatarUrl);

            // Pasar el modelo a la vista
            return View(usuario);
        }

        public async Task<IActionResult> _Lugares(int idUsuario)
        {

            List<Lugar> lugares = await this.repo.GetLugaresPorUsuarioAsync(idUsuario);
            return PartialView("_Lugares", lugares);

        }

        [HttpPost]
        public async Task<IActionResult> Seguir(int idSeguido)
        {
            var idSeguidor = HttpContext.Session.GetInt32("IdUsuario");

            if (idSeguidor == null || idSeguido == 0 || idSeguidor == idSeguido)
            {
                return RedirectToAction("Index"); // Si no es válido, redirigir al inicio
            }

            var seguidor = new Seguidor
            {
                IdUsuarioSeguidor = idSeguidor.Value,
                IdUsuarioSeguido = idSeguido,
                FechaSeguimiento = DateTime.Now
            };

            await repo.AddSeguidorAsync(seguidor);

            return RedirectToAction("PerfilUser", new { idusuario = idSeguido });
        }

    }
}
