using Imobiliare.Entities;

namespace Imobiliare.UI.Models
{
    public class FavoriteAnuntItem : Entity
    {
        public Imobil Imobil { get; set; } = default!;

        public string? FavoritesID { get; set; }
    }
}
