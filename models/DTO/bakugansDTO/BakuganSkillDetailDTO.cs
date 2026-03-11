using BakuganApi.Enums;

namespace BakuganApi.models.DTO.bakugansDTO
{
    public class BakuganSkillDetailDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public int BakuganId { get; set; }
        public int Dano { get; set; }

        public EIntensidadDano SkillDano { get; set; }
    }
}
