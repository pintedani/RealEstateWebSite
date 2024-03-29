﻿using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.ServiceLayer.Interfaces;
using Logging;

namespace Imobiliare.UI.ScheduledTasks
{
    public class AnunturiAdaugateCheckerJob : IJob
  {
    private static readonly ILog log = LogManager.GetLogger(typeof(AnunturiAdaugateCheckerJob));

    private IUnitOfWork unitOfWork;

    private IEmailManagerService emailManagerService;

    public AnunturiAdaugateCheckerJob(IUnitOfWork unitOfWork, IEmailManagerService emailManagerService)
    {
      this.unitOfWork = unitOfWork;
      this.emailManagerService = emailManagerService;
    }

    public void Execute()
    {
      try
      {
        var referenceDate = DateTime.Now.AddHours(-1);

        //Find Method - check if works
        var anunturiAdaugate =
          this.unitOfWork.AnunturiRepository.Find(
            x => x.DataAdaugareInitiala > referenceDate && x.StareAprobare == StareAprobare.InAsteptare);

        if (anunturiAdaugate.Count == 0)
        {
          log.Debug("[SERVICE ANUNTURI ADAUGATE] = No anunturi adaugate....");
        }
        else
        {
          //Send new email to admin/anunturi adaugate
          if (anunturiAdaugate.Count == 1)
          {
            //Send email as now
            var anunt = anunturiAdaugate.Single();
            this.emailManagerService.NotifyAdminAnuntAdaugat(anunt.Id, anunt.Title, anunt.UserProfile.UserName);
          }
          else
          {
            this.emailManagerService.NotifyAdminAnunturiAdaugate(1, anunturiAdaugate.Count);
          }
          log.Debug($"[SERVICE ANUNTURI ADAUGATE] = {anunturiAdaugate.Count.ToString()} anunturi adaugate -------------------- ");
        }
      }
      catch (Exception ex)
      {
        log.Error($"[SERVICE ANUNTURI ADAUGATE] = Error while checking for anuturi adaugate {ex.Message}");
        //throw;
      }
    }
  }
}