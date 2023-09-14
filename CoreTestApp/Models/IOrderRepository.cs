using Imobiliare.Entities;

namespace Imobiliare.UI.Models
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
    }
}
