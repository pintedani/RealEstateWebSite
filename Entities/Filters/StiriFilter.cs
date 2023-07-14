namespace Imobiliare.Entities
{
  public class StiriFilter : Filter
    {
    public StiriFilter()
    {
      this.Page = 1;
      this.PageSize = 10;
      this.Active = null;
    }

    /// <summary>
    /// null= all
    /// true=only active
    /// false= only inactive
    /// </summary>
    public bool? Active { get; set; }
  }
}
