using UserData;

namespace Tests.Mocks
{
    public class MockUserDetailsStorage : IUserDetailsStorage
    {
        private readonly UserDetails userDetails;

        public MockUserDetailsStorage()
            :this(new UserDetails())
        {
        }

        public MockUserDetailsStorage(UserDetails userDetails)
        {
            this.userDetails = userDetails;
        }

        public void Save(UserDetails toSave)
        {            
        }

        public UserDetails Load()
        {            ;
            return userDetails;
        }
    }
}
