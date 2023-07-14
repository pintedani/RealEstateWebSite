namespace Imobiliare.UI.Models
{
    public interface IFavorite
    {
        void AddToFavorite(Imobil imobil);

        int RemoveFromFavorite(Imobil imobil);

        List<FavoriteAnuntItem> GetFavoriteAnuntItems();

        void ClearFavorite();
    }
}
