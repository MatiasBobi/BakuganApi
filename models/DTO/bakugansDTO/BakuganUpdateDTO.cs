using BakuganApi.Enums;
using BakuganAPI.Models;

namespace BakuganApi.models.DTO.bakugansDTO
{
    public class BakuganUpdateDTO
    {
        public string? Nombre { get; set; }
        public EClasificacion? Tipo { get; set; }

        public decimal? Precio { get; set; }

    }
}
