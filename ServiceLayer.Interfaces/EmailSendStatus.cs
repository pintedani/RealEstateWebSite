namespace Imobiliare.ServiceLayer.Interfaces
{
    //Extend with other items... timeout, invalid email etc
    public enum EmailSendStatus
    {
        Success = 0,
        Error = 1,
        RestrictionatPrimireEmail = 2,
        UserNotExistsInDb = 3
    }
}
