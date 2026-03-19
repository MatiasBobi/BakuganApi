using BakuganAPI.Models;
using BakuganApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace BakuganApi.models.DTO.bakugansDTO
{
    public class BakuganModelCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public required string Nombre { get; set; }
        [Required(ErrorMessage = "El tipo es obligatorio.")]
        public EClasificacion Tipo { get; set; }

        [Required(ErrorMessage = "El precio del bakugan es obligatorio.")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "La categoria del bakugan es obligatorio.")]
        public required EBakuganCategoria Category { get; set; }
    }
}
