namespace SitecoreLibrary.BAL.Contracts
{
    public interface IPostService
    {
        void SendBookTakenEmail(string eMail, string bookName);
        void SendBookReturnedEmail(string eMail);
    }
}
