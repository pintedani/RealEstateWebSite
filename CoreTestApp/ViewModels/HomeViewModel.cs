using CoreTestApp.Models;

namespace Imobiliare.UI.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Imobil> Anunturi { get; }

        public HomeViewModel(IEnumerable<Imobil> anunturi) 
        {
            Anunturi = anunturi;
        }
    }
}
