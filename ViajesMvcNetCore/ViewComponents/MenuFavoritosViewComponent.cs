using Microsoft.AspNetCore.Mvc;
using ViajesMvcNetCore.Models;
using ViajesMvcNetCore.Repositories;

namespace ViajesMvcNetCore.ViewComponents
{
    public class MenuFavoritosViewComponent: ViewComponent
    {
        private RepositoryHome repo;

        public MenuFavoritosViewComponent(RepositoryHome repo) 
        {  
            this.repo = repo; 
        }

        public async Task<IViewComponentResult> InvokeAsync(int idusuario)
        {
            List<LugarFavorito> favoritos = await this.repo.GetFavoritosLugarAsync(idusuario);
            return View();
        }
    }
}
