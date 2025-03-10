using Microsoft.AspNetCore.Mvc;
using ViajesMvcNetCore.Repositories;
using ViajesMvcNetCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ViajesMvcNetCore.ViewComponents
{
    public class MenuLugaresViewComponent : ViewComponent
    {
        private readonly RepositoryHome repo;

        public MenuLugaresViewComponent(RepositoryHome repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync(int idUsuario)
        {
            List<Lugar> lugares = await this.repo.GetLugaresPorUsuarioAsync(idUsuario);
            return View(lugares);
        }
    }
}
