using Microsoft.AspNetCore.Mvc;
using ViajesMvcNetCore.Models;
using ViajesMvcNetCore.Repositories;

namespace ViajesMvcNetCore.ViewComponents
{
    public class MenuSeguidosViewComponentViewComponent: ViewComponent
    {
        private RepositoryHome repo;

        public MenuSeguidosViewComponentViewComponent(RepositoryHome repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync(int idusuario)
        {
            List<UsuarioSeguidoPerfil> usuario = await this.repo.GetSeguidoresUsuarioAsync(idusuario);
            return View();
        }
    }
}
