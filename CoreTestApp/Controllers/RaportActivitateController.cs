using System.Globalization;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.UI.Models;
using Imobiliare.UI.Utils.Rapoarte;
using Microsoft.AspNetCore.Mvc;
using Logging;

namespace Imobiliare.UI.Controllers
{
    public class RaportActivitateController : Controller
    {
        private const int DEFAULT_NUMBER_OF_DAYS = 10;
        private string DateFormat = "dd/MM/yyyy";

        private readonly IUnitOfWork unitOfWork;

        private static readonly ILog log = LogManager.GetLogger(typeof(RaportActivitateController));

        public RaportActivitateController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: RaportActivitate
        public async Task<ActionResult> Index(string startDateTime, string endDateTime)
        {
            DateTime startDateTimeFinal = DateTime.Now.AddDays(-DEFAULT_NUMBER_OF_DAYS);
            if (!string.IsNullOrEmpty(startDateTime))
            {
                startDateTimeFinal = DateTime.ParseExact(startDateTime, DateFormat, CultureInfo.InvariantCulture);
            }

            DateTime endDateTimeFinal = DateTime.Now;
            if (!string.IsNullOrEmpty(endDateTime))
            {
                endDateTimeFinal = DateTime.ParseExact(endDateTime, DateFormat, CultureInfo.InvariantCulture);
            }

            var result = new RaportActivitateData
            {
                RaportActivitates = this.unitOfWork.RaportActivitateRepository.Find(x =>
                    x.CreateDateTime >= startDateTimeFinal && x.CreateDateTime <= endDateTimeFinal)
            };
            result.RaportActivitateGraphics =
                RaportActivitateToRaportActivitateGraphicConverter.GenerateRaportActivitateGraphic(
                    result.RaportActivitates);
            result.StartDate = startDateTimeFinal;
            result.EndDate = endDateTimeFinal;
            return View(result);
        }
    }
}
