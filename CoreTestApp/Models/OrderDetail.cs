using System.IO.Pipelines;

namespace Imobiliare.UI.Models
{
    public class OrderDetail : Entity
    {
        public int OrderId { get; set; }
        public int ImobilId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public Imobil Imobil { get; set; } = default!;
        public Order Order { get; set; } = default!;
    }
}
