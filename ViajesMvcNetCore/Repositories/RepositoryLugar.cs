using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ViajesMvcNetCore.Data;
using ViajesMvcNetCore.Models;

namespace ViajesMvcNetCore.Repositories
{
    public class RepositoryLugar
    {
        private ViajesContext context;
        
        public RepositoryLugar(ViajesContext context)
        {
            this.context = context;
        }

        public async Task<List<Lugar>> GetLugaresAsync()
        {
            string sql = "SELECT * FROM LUGARES";
            var consulta = this.context.Lugares.FromSqlRaw(sql);
            return await consulta.ToListAsync();

        }

        public async Task<List<LugarFavorito>> GetFavoritosLugarAsync(int idUsuario)
        {
            string sql = "EXEC SP_FAVORITOS_BY_USUARIO @idusuario";

            var favoritos = await this.context.LugaresFavoritos
                .FromSqlRaw(sql, new SqlParameter("@idusuario", idUsuario))
                .ToListAsync();

            return favoritos;
        }

        public async Task<Lugar> FindLugarAsync(int idLugar)
        {
            var consulta = from datos in this.context.Lugares
                           where datos.IdLugar == idLugar
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task InsertLugarAsync(string nombre, string descripcion, string ubicacion, string categoria, DateTime horario, string imagen, string tipo, int idUsuario)
        {
            string sql = "EXEC SP_INSERT_LUGARES @nombre, @descripcion, @ubicacion, @categoria, @horario, @imagen, @tipo, @id_usuario";

            // Ejecuta el procedimiento almacenado de manera asíncrona
            await this.context.Database.ExecuteSqlRawAsync(sql,
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@descripcion", descripcion),
                new SqlParameter("@ubicacion", ubicacion),
                new SqlParameter("@categoria", categoria),
                new SqlParameter("@horario", horario),
                new SqlParameter("@imagen", imagen),
                new SqlParameter("@tipo", tipo),
                new SqlParameter("@id_usuario", idUsuario)  
            );
        }
        
        public async Task<List<Lugar>> FindLugarByNameAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return new List<Lugar>();
            }

            string lowerSearchTerm = searchTerm.ToLower();


            var lugaresEncontrados = await this.context.Lugares
                .Where(lugar => lugar.Nombre.ToLower().Contains(lowerSearchTerm))
                .ToListAsync();

            return lugaresEncontrados;
        }
        
        public async Task<List<Comentario>> GetComentariosLugarAsync(int idLugar)
        {
            string sql = "EXEC SP_GET_COMENTARIOS_LUGAR @idLugar";

            var consulta = this.context.Comentarios
                .FromSqlRaw(sql, new SqlParameter("@idLugar", idLugar));

            return await consulta.ToListAsync();
        }
        
        public async Task AddComentarioAsync(int idLugar, int idUsuario, string comentario)
        {
            string sql = "EXEC SP_INSERT_COMENTARIO @idLugar, @idUsuario, @comentario";

            await this.context.Database.ExecuteSqlRawAsync(sql,
                new SqlParameter("@idLugar", idLugar),
                new SqlParameter("@idUsuario", idUsuario),
                new SqlParameter("@comentario", comentario)
            );
        }
        
        public async Task AddFavoritoAsync(int idUsuario, int idLugar, DateTime fecha, string imagen, string nombre, string descripcion, string ubicacion, string tipo)
        {  
            string sql = "EXEC SP_AGREGAR_FAVORITO @idusuario, @idlugar, @fecha, @imagen, @nombre, @descripcion, @ubicacion, @tipo";

            var result = await this.context.Database.ExecuteSqlRawAsync(sql,
                new SqlParameter("@idusuario", idUsuario),
                new SqlParameter("@idlugar", idLugar),
                new SqlParameter("@fecha", fecha),
                new SqlParameter("@imagen", imagen),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@descripcion", descripcion),
                new SqlParameter("@ubicacion", ubicacion),
                new SqlParameter("@tipo", tipo)
            );
        }

        public async Task<bool> ExisteFavoritoAsync(int idUsuario, int idLugar)
        {
            // Comprobar si el lugar ya está en los favoritos del usuario
            var favorito = await this.context.LugaresFavoritos
                .FirstOrDefaultAsync(f => f.IdUsuario == idUsuario && f.IdLugar == idLugar);

            return favorito != null;  // Si ya existe, retorna true
        }


        public async Task DeleteComentarioAsync(int idComentario)
        {
            string sql = "DELETE FROM COMENTARIOS WHERE ID_COMENTARIO = @idcomentario";

            await this.context.Database.ExecuteSqlRawAsync(sql,
                new SqlParameter("@idcomentario", idComentario)
            );
        }


        public async Task UpdateComentarioAsync(int idComentario, int idLugar, int idUsuario, string comentario, DateTime fechaComentario)
        {
            string sql = "EXEC SP_UPDATE_COMENTARIO @id_comentario, @id_lugar, @id_usuario, @comentario, @fecha_comentario";

            await context.Database.ExecuteSqlRawAsync(sql,
                new SqlParameter("@id_comentario", idComentario),
                new SqlParameter("@id_lugar", idLugar),
                new SqlParameter("@id_usuario", idUsuario),
                new SqlParameter("@comentario", comentario),
                new SqlParameter("@fecha_comentario", fechaComentario)
            );
        }

        public async Task<List<Lugar>> GetLugaresPorTipoAsync(string tipo)
        {
            var lugares = await this.context.Lugares
                .Where(l => l.Tipo.ToLower() == tipo.ToLower())
                .ToListAsync();

            return lugares;
        }

    }
}
