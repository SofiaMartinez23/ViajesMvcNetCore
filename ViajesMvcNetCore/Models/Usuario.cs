using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ViajesMvcNetCore.Models
{

    [Table("USUARIOS")]
    public class Usuario
    {
        [Key]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; }

        [Column("EDAD")]
        public int Edad { get; set; }

        [Column("NACIONALIDAD")]
        public string Nacionalidad { get; set; }

        [Column("PREFERENCIASDEVIAJE")]
        public string PreferenciaViaje { get; set; }

        [Column("IMAGEN")]
        public string Imagen { get; set; }

        [Column("FECHADEREGISTRO")]
        public DateTime FechaRegistro { get; set; }
    }
}
