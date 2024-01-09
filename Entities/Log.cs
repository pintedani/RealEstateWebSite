namespace Imobiliare.Entities
{
  using System.Runtime.Serialization;

  public class Log
  {
    #region Primitive Properties
    [DataMember]
    public virtual int Id
    {
      get;
      set;
    }
    [DataMember]
    public virtual System.DateTime Date
    {
      get;
      set;
    }
    [DataMember]
    public virtual string? Thread
    {
      get;
      set;
    }
    [DataMember]
    public virtual string Level
    {
      get;
      set;
    }
    [DataMember]
    public virtual string Logger
    {
      get;
      set;
    }
    [DataMember]
    public virtual string Message
    {
      get;
      set;
    }
    [DataMember]
    public virtual string Exception
    {
      get;
      set;
    }

    #endregion
  }
}