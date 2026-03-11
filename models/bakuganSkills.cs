using BakuganApi.Enums;

namespace BakuganAPI.Models;

public class BakuganSkillModel
{
    public int Id { get; set; } 

    public string Nombre { get; set; } = string.Empty;

    public int Dano { get; set; }

    // Clave FK

    public int BakuganId { get; set; }

    // Navegacion hacia Bakugan
    public BakuganModel? Bakugan { get; set; }

    public EIntensidadDano SkillDano { get; set; }

}