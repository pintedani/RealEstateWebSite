using System.Linq;
using Imobiliare.Repositories.Interfaces;
using System;
using System.Globalization;

using Imobiliare.Entities;
using Imobiliare.ServiceLayer.Interfaces;

using Imobiliare.UI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Logging;

namespace Imobiliare.UI.Controllers
{
    [Authorize(Roles = "Admin")]
  public class RapoarteController : Controller
  {
    private const int DEFAULT_PAGE_SIZE = 20;

    private DateTimeFormatInfo dateFormat = new DateTimeFormatInfo { ShortDatePattern = "dd'/'MM'/'yyyy" };

    private readonly IUnitOfWork unitOfWork;

    private static readonly ILog log = LogManager.GetLogger(typeof(RapoarteController));

    public RapoarteController(IUnitOfWork unitOfWork)
    {
      this.unitOfWork = unitOfWork;
    }

    // GET: /Rapoarte/

    public ActionResult Index()
    {
      RapoarteData rapoarteData = new RapoarteData();
      var administrators = this.unitOfWork.UsersRepository.GetUserProfiles().Where(x => x.Role == Role.Administrator).ToList();
      rapoarteData.Administratori = administrators;

      var imobilFilter2 = new ImobilFilter { PageSize = 60, Page = 1 };
      imobilFilter2.StareAprobare = StareAprobare.Aprobat;
      imobilFilter2.PerioadaStart = DateTime.Now.AddDays(-7);
      imobilFilter2.FiltareDupaDataAdaugareInitiala = false;
      int totalNumberOfImobils2;
      var anunturi = this.unitOfWork.AnunturiRepository.GetAllImobil(imobilFilter2, out totalNumberOfImobils2);
      foreach (var anunt in anunturi)
      {
        if (anunt.DataAdaugareInitiala.AddDays(7) > anunt.DataAdaugare)
        {
          rapoarteData.AnunturiNoiAdaugate.Add(anunt);
        }
        else
        {
          rapoarteData.AnunturiReactualizate.Add(anunt);
        }
      }

      var referenceDate = DateTime.Now.AddDays(-7);
      rapoarteData.UseriCreati = this.unitOfWork.UsersRepository.Find(x => x.UserCreateDate > referenceDate);
      rapoarteData.Stiri = this.unitOfWork.StiriRepository.Find(x => x.DateCreated > referenceDate && x.Active);

      return this.View(rapoarteData);
    }

    public ActionResult Anunturi()
    {
      RapoarteAnunturiDetalii rapoarteAnunturiDetalii = new RapoarteAnunturiDetalii();
      var imobilFilter2 = new ImobilFilter { PageSize = 60, Page = 1 };
      imobilFilter2.StareAprobare = StareAprobare.Aprobat;
      imobilFilter2.PerioadaStart = DateTime.Now.AddDays(-7);
      imobilFilter2.FiltareDupaDataAdaugareInitiala = false;
      int totalNumberOfImobils2;
      var anunturi = this.unitOfWork.AnunturiRepository.GetAllImobil(imobilFilter2, out totalNumberOfImobils2);
      foreach (var anunt in anunturi)
      {
        if (anunt.DataAdaugareInitiala.AddDays(7) > anunt.DataAdaugare)
        {
          rapoarteAnunturiDetalii.AnunturiNoiAdaugate.Add(anunt);
        }
        else
        {
          rapoarteAnunturiDetalii.AnunturiReactualizate.Add(anunt);
        }
      }

      var referenceDate = DateTime.Now.AddDays(-7);
      rapoarteAnunturiDetalii.UseriCreati = this.unitOfWork.UsersRepository.Find(x => x.UserCreateDate > referenceDate);
      return this.View(rapoarteAnunturiDetalii);
    }

    public ActionResult Stiri()
    {
      RapoarteStiriDetalii rapoarteStiriDetalii = new RapoarteStiriDetalii();

      var referenceDate = DateTime.Now.AddDays(-7);
      rapoarteStiriDetalii.Stiri = this.unitOfWork.StiriRepository.Find(x => x.DateCreated > referenceDate && x.Active);
      return this.View(rapoarteStiriDetalii);
    }
  }
}
