namespace Imobiliare.Repositories
{
  using System;
  using System.Collections.Generic;
    using System.Linq;

  using Imobiliare.Entities;
  using Imobiliare.Repositories.Interfaces;
    using Logging;
    using Microsoft.EntityFrameworkCore;

    public class CartierRepository : Repository<Cartier>, ICartierRepository
  {
    private static readonly ILog log = LogManager.GetLogger(typeof(CartierRepository));

    public CartierRepository(ApplicationDbContext dbContext)
      : base(dbContext, new SortSpec(nameof(Stire.Id)))
    {
    }

    public Dictionary<Cartier, int> GetSelectableCartiersWithNumber(ImobilFilter imobilFilter)
    {
      var selectableCartierWithNumber = new Dictionary<Cartier, int>();
      DateTime dateLimit = DateTime.Now;
      if (imobilFilter.VechimeMaximaZile > 0)
      {
        dateLimit = DateTime.Now.AddDays(-imobilFilter.VechimeMaximaZile);
      }

      var cartiere = this.DbContext.Cartiere.Where(x => x.OrasId == imobilFilter.OrasId).OrderBy(x => x.Nume);
      foreach (var cartier in cartiere.ToList())
      {
        //to satisfy linq
        var cartier1 = cartier;
        selectableCartierWithNumber.Add(cartier1, 0);
      }
      foreach (var imobil in this.DbContext.Imobile.Include("Cartier").Where(x => x.StareAprobare == StareAprobare.Aprobat
       && (imobilFilter.TipProprietate == TipProprietate.Toate || imobilFilter.TipProprietate == x.TipProprietate)
       && (imobilFilter.TipOfertaGen == TipOfertaGen.Toate || imobilFilter.TipOfertaGen == x.TipOfertaGen)
              && (imobilFilter.CamereMin == 0 || x.NumarCamere >= imobilFilter.CamereMin)
              && (imobilFilter.CamereMax == 0 || x.NumarCamere <= imobilFilter.CamereMax)
              && (imobilFilter.MpMin == 0 || x.Suprafata >= imobilFilter.MpMin)
              && (imobilFilter.MpMax == 0 || x.Suprafata <= imobilFilter.MpMax)
              && (imobilFilter.PretMin == 0 || x.Price >= imobilFilter.PretMin)
              && (imobilFilter.PretMax == 0 || x.Price <= imobilFilter.PretMax)
              && (x.TipVanzator == imobilFilter.TipVanzator || imobilFilter.TipVanzator == TipVanzator.TotiVanzatorii)
              && (imobilFilter.VechimeMaximaZile == 0 || x.DataAdaugare >= dateLimit)))
      {
        if (imobil.Cartier != null && selectableCartierWithNumber.Any(x => x.Key.Id == imobil.Cartier.Id))
        {
          var cartier = selectableCartierWithNumber.Single(x => x.Key.Id == imobil.Cartier.Id).Key;
          selectableCartierWithNumber[cartier]++;
        }
      }
      return selectableCartierWithNumber;
    }

    public List<Cartier> GetSelectableCartiers(int orasId)
    {
      var cartiere = this.DbContext.Cartiere.Where(x => x.OrasId == orasId);
      return cartiere.OrderBy(x => x.Nume).ToList();
    }

    public bool AddCartier(string nume, int orasId)
    {
      try
      {
        if (nume == "0" || nume == string.Empty)
        {
          throw new ArgumentException();
        }
        if (!this.DbContext.Orase.Any(x => x.Id == orasId))
        {
          throw new ArgumentException();
        }
        this.DbContext.Cartiere.Add(new Cartier { Nume = nume, OrasId = orasId });
        return true;
      }
      catch (Exception exception)
      {
        //log.ErrorFormat("Error while adding cartier {0} for oras with id {1}, exception {2}", nume, orasId, exception);
        return false;
      }
    }

    public bool DeleteLastCartier(int cartierID)
    {
      var cartier = this.DbContext.Cartiere.Single(e => e.Id == cartierID);
      try
      {
        this.DbContext.Cartiere.Remove(cartier);
      }
      catch (Exception exception)
      {
        string error = exception.Message;
        if (exception.InnerException != null)
        {
          error += exception.InnerException.Message;
        }
        log.ErrorFormat("Error while deleting cartier with id {0}, exception {1}", cartierID, error);
        return false;
      }
      return true;
    }

    public bool EditeazaCartier(int cartierID, string nume)
    {
      try
      {
        var cartier = this.DbContext.Cartiere.Single(x => x.Id == cartierID);
        cartier.Nume = nume;
      }
      catch (Exception exception)
      {
        string error = exception.Message;
        if (exception.InnerException != null)
        {
          error += exception.InnerException.Message;
        }
        log.ErrorFormat("Error while editing cartier with id {0}, exception {1}", cartierID, error);
        return false;
      }
      return true;
    }
  }
}
