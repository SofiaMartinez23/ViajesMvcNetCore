using Microsoft.EntityFrameworkCore;
using System;
using ViajesMvcNetCore.Models;


namespace ViajesMvcNetCore.Data
{
    public class ViajesContext : DbContext
    {
        public ViajesContext(DbContextOptions<ViajesContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Lugar> Lugares { get; set; }
        public DbSet<UsuarioLogin> UsuariosLogin { get; set; }
        public DbSet<LugarFavorito> LugaresFavoritos { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Chat> Chats { get; set; }


    }
}