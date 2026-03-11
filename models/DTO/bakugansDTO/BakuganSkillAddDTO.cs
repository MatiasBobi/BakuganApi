namespace BakuganApi.models.DTO.bakugansDTO
{
    using BakuganApi.Enums;

    public class BakuganSkillAddDTO
    {
        public required string Nombre { get; set; }

        public required int Dano { get; set; }

        public required EIntensidadDano SkillDano { get; set; }
    }
}
