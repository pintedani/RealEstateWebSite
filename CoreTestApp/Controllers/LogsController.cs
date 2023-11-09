using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.UI.Models;
//using log4net;
//using log4net.Appender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imobiliare.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LogsController : Controller
    {
        private string DateFormat = "dd/MM/yyyy";

        private IUnitOfWork unitOfWork;

        public LogsController(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: Logs
        public ActionResult Index(int? page, string logType, string searchText, string date)
        {
            var selectedPageFinal = 1;

            if (page != null)
            {
                selectedPageFinal = page.Value;
            }

            DateTime finalDate = DateTime.Today;
            if (!string.IsNullOrEmpty(date))
            {
                finalDate = DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture);
            }

            var logTypeFinal = logType;
            if (logTypeFinal == null)
            {
                logTypeFinal = "TOATE";
            }

            //Flush log4net buffer
            //var rep = LogManager.GetRepository();
            //foreach (var appender in rep.GetAppenders())
            //{
            //    var buffered = appender as BufferingAppenderSkeleton;
            //    if (buffered != null)
            //    {
            //        buffered.Flush();
            //    }
            //}

            int totalNumberOfPages;
            var logsFilter = new Entities.Filter { PageSize = 300, Page = selectedPageFinal };
            var logs = this.unitOfWork.LogsRepository.GetFiltered(log => log.Date.Day == finalDate.Day && log.Date.Month == finalDate.Month && log.Date.Year == finalDate.Year
                                                                  && (logTypeFinal == "TOATE" || log.Level == logTypeFinal)
                                                                  && (string.IsNullOrEmpty(searchText) || log.Message.Contains(searchText)), logsFilter, out totalNumberOfPages);
            return
                this.View(new LogsData
                {
                    Logs = logs,
                    SelectedDate = finalDate,
                    SelectedSeverity = logTypeFinal,
                    SearchText = searchText,
                    NumberOfUsers = 0,
                    LogsFilter = logsFilter,
                    NumberOfPages = (int)Math.Ceiling((double)totalNumberOfPages / logsFilter.PageSize)

                });
        }
    }
}