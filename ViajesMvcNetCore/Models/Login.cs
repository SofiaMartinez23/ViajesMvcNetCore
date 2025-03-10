using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ViajesMvcNetCore.Models
{
    [Table("LOG_IN")]
    public class Login
    {
        [Key]
        [Column("ID_USUARIO")]
        public string IdUsuario { get; set; }
        [Column("CORREO")]
        public string Email { get; set; }
        [Column("CLAVE")]
        public string Clave { get; set; }
    }
}
