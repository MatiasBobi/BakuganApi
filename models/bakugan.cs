using BakuganApi.Enums;
using BakuganApi.models;
using System.ComponentModel.DataAnnotations;

namespace BakuganAPI.Models;

public class BakuganModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public required string Nombre { get; set; }
    [Required(ErrorMessage = "El tipo es obligatorio.")]
    public EClasificacion Tipo { get; set; }
    [Required(ErrorMessage = "El precio del bakugan es obligatorio.")]
    public decimal Precio { get; set; } = decimal.Zero;


    // 1-N hacia Skills de cada bakugan, un bakugan puede tener muchas habilidades.
    public List<BakuganSkillModel> Habilidades { get; set; } = new List<BakuganSkillModel>();

    // Relacion N - N Bakugans - Categories
    public List<BakuganCategory> Categories { get; set; } = new List<BakuganCategory>();

    // Relacion N - N : Muchos bakugans pueden pertenecer varios usuarios y viceversa.
    public List<Usuario> Usuarios { get; set; } = new List<Usuario>();


}