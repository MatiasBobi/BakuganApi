using BakuganApi.Enums;
using BakuganApi.models;
using BakuganAPI.Models;

namespace BakuganApi.models.DTO.bakugansDTO
{
    public class BakuganDetailDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public EClasificacion Tipo { get; set; }

        public decimal Precio { get; set; }

        public List<BakuganSkillDetailDTO> Habilidades { get; set; } = new List<BakuganSkillDetailDTO>();
        public EBakuganCategoria? Category { get; set; }
    }
}
