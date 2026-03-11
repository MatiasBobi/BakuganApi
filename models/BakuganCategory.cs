using BakuganAPI.Models;

namespace BakuganApi.models
{
    public class BakuganCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Relacion N - N Bakugans - Categories
        public List<BakuganModel> Bakugans { get; set; } = new List<BakuganModel>();
    }
}
