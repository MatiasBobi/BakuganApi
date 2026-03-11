using BakuganAPI.Models;

namespace BakuganApi.models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }

        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        public required string TipoUsuario { get; set; }

        // Relacion N-N : Un usuario puede tener muchos bakugans

        public List<BakuganModel> Bakugans { get; set; } = new List<BakuganModel>();


    }
}
