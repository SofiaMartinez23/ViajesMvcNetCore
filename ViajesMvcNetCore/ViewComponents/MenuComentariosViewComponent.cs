using Microsoft.AspNetCore.Mvc;
using ViajesMvcNetCore.Models;
using ViajesMvcNetCore.Repositories;

namespace ViajesMvcNetCore.ViewComponents
{
    public class MenuComentariosViewComponent: ViewComponent
    {
        private RepositoryLugar repo;

        public MenuComentariosViewComponent(RepositoryLugar repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync(int idcomentario)
        {
            List<Comentario> comentarios = await this.repo.GetComentariosLugarAsync(idcomentario);
            return View();
        }
    }
}
