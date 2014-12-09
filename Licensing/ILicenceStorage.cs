namespace Licensing
{
    public interface ILicenceStorage
    {
        Licence GetLicence();
        void StoreLicence(string licenceKey);
    }
}