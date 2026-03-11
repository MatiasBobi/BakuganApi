using BakuganApi.Enums;

namespace BakuganApi.models.DTO.bakugansDTO
{
    public class BakuganSkillUpdateDTO
    {
        public string? Nombre { get; set; }
        public int? Dano { get; set; }

        public EIntensidadDano? SkillDano { get; set; }
    }
}
