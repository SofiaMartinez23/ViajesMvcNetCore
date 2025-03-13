using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ViajesMvcNetCore.Data;
using ViajesMvcNetCore.Models;

namespace ViajesMvcNetCore.Repositories
{
    public class RepositoryHome
    {
        private ViajesContext context;

        public RepositoryHome(ViajesContext context)
        {
            this.context = context;
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

        public async Task UpdateLugarAsync(int idLugar, string nombre, string descripcion, string ubicacion, string categoria, DateTime horario, string imagen, string tipo)
        {
            string sql = "EXEC SP_UPDATE_LUGAR @id_lugar, @nombre, @descripcion, @ubicacion, @categoria, @horario, @imagen, @tipo";
            await context.Database.ExecuteSqlRawAsync(sql,
                new SqlParameter("@id_lugar", idLugar),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@descripcion", descripcion),
                new SqlParameter("@ubicacion", ubicacion),
                new SqlParameter("@categoria", categoria),
                new SqlParameter("@horario", horario),
                new SqlParameter("@imagen", imagen),
                new SqlParameter("@tipo", tipo));
        }

        public async Task DeleteLugarAsync(int idLugar)
        {
            string sql = "EXEC SP_DELETE_LUGAR @idlugar";
            await context.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@idlugar", idLugar));
        }

        public async Task UpdatePerfilAsync(int idUsuario, string nombre, string correo, string clave, string confirmarClave, string preferenciaViaje, string colorAvatar, string avatarUrl, int edad, string nacionalidad)
        {
            string sql = "EXEC SP_UPDATE_PERFIL @id_usuario, @nombre, @correo, @clave, @confirmarclave, @preferenciaviaje, @coloravatar, @avatarurl, @edad, @nacionalidad";
            await context.Database.ExecuteSqlRawAsync(sql,
                new SqlParameter("@id_usuario", idUsuario),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@correo", correo),
                new SqlParameter("@clave", clave),
                new SqlParameter("@confirmarclave", confirmarClave),
                new SqlParameter("@preferenciaviaje", preferenciaViaje),
                new SqlParameter("@coloravatar", colorAvatar),
                new SqlParameter("@avatarurl", avatarUrl),
                new SqlParameter("@edad", edad),
                new SqlParameter("@nacionalidad", nacionalidad)
            );
        }

        public async Task<List<LugarFavorito>> GetFavoritosLugarAsync(int idUsuario)
        {
            string sql = "EXEC SP_FAVORITOS_BY_USUARIO @idusuario";

            var favoritos = await this.context.LugaresFavoritos
                .FromSqlRaw(sql, new SqlParameter("@idusuario", idUsuario))
                .ToListAsync();

            return favoritos;
        }

        public async Task DeleteFavoritoAsync(int idUsuario, int idLugar)
        {
            string sql = "EXEC SP_DELETE_FAVORITO @idusuario, @idlugar";

            var result = await this.context.Database.ExecuteSqlRawAsync(sql,
                new SqlParameter("@idusuario", idUsuario),
                new SqlParameter("@idlugar", idLugar)
            );

        }
        public async Task DeleteSeguidorAsync(int idUsuarioSeguidor, int idUsuarioSeguido)
        {
            string sql = "EXEC SP_DELETE_SEGUIDOR @idusuarioseguidor, @idusuarioseguido";

            await context.Database.ExecuteSqlRawAsync(sql,
                new SqlParameter("@idusuarioseguidor", idUsuarioSeguidor),
                new SqlParameter("@idusuarioseguido", idUsuarioSeguido)
            );
        }
        public async Task<List<UsuarioSeguidoPerfil>> GetSeguidoresUsuarioAsync(int idusuario)
        {
            string sql = "EXEC SP_SEGUIDORES_BY_USUARIO @idusuario";

            List<UsuarioSeguidoPerfil>  user = await this.context.UsuarioSeguidoPerfiles
                .FromSqlRaw(sql, new SqlParameter("@idusuario", idusuario))
                .ToListAsync();

            return user;
        }

    }
}
