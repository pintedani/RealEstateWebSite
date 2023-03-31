using CoreTestApp.Models;

namespace CoreTestApp.ViewModels
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
