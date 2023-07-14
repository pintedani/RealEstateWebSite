namespace Imobiliare.Repositories.Interfaces
{
  using System;
  using System.Collections.Generic;

  using Imobiliare.Entities;

  public interface ILogsRepository : IRepository<Log>
  {
    int DeleteLogsOltherThanDate(DateTime parse);
  }
}