namespace UserData
{
    public interface IUserDetailsStorage
    {
        void Save(UserDetails userDetails);
        UserDetails Load();
    }
}