namespace Imobiliare.Repositories
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Imobiliare.Entities;
  using Imobiliare.Repositories.Interfaces;

  public class LogsRepository : Repository<Log>, ILogsRepository
  {
    public LogsRepository(ApplicationDbContext dbContext) : base(dbContext, new SortSpec(nameof(Log.Id), SortDirection.Descending))
    {
    }

    public int DeleteLogsOltherThanDate(DateTime parse)
    {
      string sqlFormattedDate = parse.ToString("yyyy-MM-dd HH:mm:ss");
      //Be sure to check Db Name: "Log" or "Logs"
      return this.DbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.Log WHERE Date < '" + sqlFormattedDate + "'");
    }
  }
}