using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViajesMvcNetCore.Models
{
    [Table("SEGUIDORES")]
    public class Seguidor
    {
        [Key]
        [Column("ID_SEGUIDOR")]
        public int IdSeguidor { get; set; }

        [Column("ID_USUARIO_SEGUIDOR")]
        public int IdUsuarioSeguidor { get; set; }

        [Column("ID_USUARIO_SEGUIDO")]
        public int IdUsuarioSeguido { get; set; }

        [Column("FECHA_SEGUIMIENTO")]
        public DateTime FechaSeguimiento { get; set; }

        // Claves foráneas (opcional si ya tienes las relaciones configuradas en el DbContext)
        [ForeignKey("IdUsuarioSeguidor")]
        public Usuario UsuarioSeguidor { get; set; } // Relación con el usuario seguidor

        [ForeignKey("IdUsuarioSeguido")]
        public Usuario UsuarioSeguido { get; set; } // Relación con el usuario seguido
    }
}