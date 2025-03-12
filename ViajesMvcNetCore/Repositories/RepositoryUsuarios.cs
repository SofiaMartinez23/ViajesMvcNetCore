using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ViajesMvcNetCore.Data;
using ViajesMvcNetCore.Models;

namespace ViajesMvcNetCore.Repositories
{
    public class RepositoryUsuarios
    {
        private ViajesContext context;

        public RepositoryUsuarios(ViajesContext context)
        {
            this.context = context;
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            string sql = "SELECT * FROM USUARIOS";
            var consulta = this.context.Usuarios.FromSqlRaw(sql);
            return await consulta.ToListAsync();

        }

        public async Task<List<Usuario>> FindUsuarioByNameAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return new List<Usuario>();
            }

            string lowerSearchTerm = searchTerm.ToLower();

            var usuariosEncontrados = await this.context.Usuarios
                .Where(usuario => usuario.Nombre.ToLower().Contains(lowerSearchTerm))
                .ToListAsync();

            return usuariosEncontrados;
        }
        public async Task<List<Lugar>> GetLugaresPorUsuarioAsync(int idUsuario)
        {
            var lugares = new List<Lugar>();
            string sql = "EXEC SP_GET_LUGARES_POR_USUARIO @id_usuario";

            lugares = await this.context.Lugares.FromSqlRaw(sql,
                new SqlParameter("@id_usuario", idUsuario))
                .ToListAsync();


            return lugares;
        }
        public async Task AddSeguidorAsync(Seguidor seguidor)
        {
            
            string sql = "EXEC SP_ADD_SEGUIR @idusuarioseguidor, @idusuarioseguido, @fechaseguimiento";

            await context.Database.ExecuteSqlRawAsync(sql,
                new SqlParameter("@idusuarioseguidor", seguidor.IdUsuarioSeguidor),
                new SqlParameter("@idusuarioseguido", seguidor.IdUsuarioSeguido),
                new SqlParameter("@fechaseguimiento", seguidor.FechaSeguimiento)
            );
        }
        public async Task<bool> ExisteSeguidorAsync(int idUsuarioSeguidor, int idUsuarioSeguido)
        {
            return await this.context.UsuarioSeguidoPerfiles
                .AnyAsync(s => s.IdUsuarioSeguidor == idUsuarioSeguidor && s.IdUsuarioSeguido == idUsuarioSeguido);
        }


    }
}
