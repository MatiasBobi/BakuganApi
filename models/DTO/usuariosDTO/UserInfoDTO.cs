using BakuganApi.models.DTO.bakugansDTO;

namespace BakuganApi.models.DTO.usuariosDTO
{
    public class UserInfoDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Email { get; set; }

        public string? TipoUsuario { get; set; }

        public BakuganDetailDTO? Bakugans { get; set; } = new();
    }
}
