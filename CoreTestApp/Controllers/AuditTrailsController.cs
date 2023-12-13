using System;
using System.Globalization;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imobiliare.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuditTrailsController : Controller
    {
        private DateTimeFormatInfo dateFormat = new DateTimeFormatInfo { ShortDatePattern = "dd'/'MM'/'yyyy" };

        private const int PageSize = 300;

        private IUnitOfWork unitOfWork;

        public AuditTrailsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: AuditTrails
        public ActionResult Index(int? page, string searchText, string date)
        {
            var selectedPageFinal = 1;

            if (page != null)
            {
                selectedPageFinal = page.Value;
            }

            var s1 = DateTime.Now.ToString(dateFormat);
            DateTime finalDate = DateTime.Parse(s1, dateFormat);

            if (!string.IsNullOrWhiteSpace(date))
            {
                finalDate = DateTime.Parse(date, dateFormat);
            }

            int totalNumberOfPages;
            var logsFilter = new Entities.Filter { PageSize = PageSize, Page = selectedPageFinal };
            var auditTrails = this.unitOfWork.AuditTrailRepository.GetFiltered(log => log.Date.Day == finalDate.Day && log.Date.Month == finalDate.Month && log.Date.Year == finalDate.Year
                                                                  && (string.IsNullOrEmpty(searchText) || log.Message.Contains(searchText)), logsFilter, out totalNumberOfPages);
            return
                this.View(new AuditTrailData
                {
                    AuditTrails = auditTrails,
                    SelectedDate = finalDate,
                    SearchText = searchText,
                    NumberOfUsers = 0,
                    LogsFilter = logsFilter,
                    NumberOfPages = (int)Math.Ceiling((double)totalNumberOfPages / logsFilter.PageSize)

                });
        }
    }
}
