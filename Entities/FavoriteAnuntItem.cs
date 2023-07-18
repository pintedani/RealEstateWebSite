using Imobiliare.Entities;

namespace Imobiliare.Entities
{
    public class FavoriteAnuntItem : Entity
    {
        public Imobil Imobil { get; set; } = default!;

        public string? FavoritesID { get; set; }
    }
}
