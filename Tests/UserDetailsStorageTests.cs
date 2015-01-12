using System.IO;
using Native;
using NUnit.Framework;
using UserData;

namespace Tests
{
    [TestFixture]
    public class UserDetailsStorageTests
    {
        [Test]
        public void CanRecallSavedData()
        {
            //given some user details
            var savedDetails = new UserDetails();
            savedDetails.AddSage50DataLocation("a location");

            //when we save them to a storage which does not already have some data
            var storage = CreateStorage();
            storage.Save(savedDetails);

            //then the details can be loaded correctly
            var loadedDetails = storage.Load();
            CollectionAssert.AreEqual(new[]{"a location"}, loadedDetails.Sage50DataLocations);
        }  
        
        [Test]
        public void WhenFileCorruptEmptyUserDetailsAreReturned()
        {
            //given a storage pointing at an empty file
            var storage = CreateStorage();            
            //when we load the details
            var loadedDetails = storage.Load();
            //then the collection is just empty
            CollectionAssert.IsEmpty(loadedDetails.Sage50DataLocations);
        }   
        
        [Test]
        public void WhenFileDoesNotExistEmptyUserDetailsAreReturned()
        {
            //given a storage pointing at a file which does not exist
            var fileWhichDoesNotExist = Path.GetTempFileName();
            File.Delete(fileWhichDoesNotExist);
            var storage = new UserDetailsStorage(new FileSystem(), fileWhichDoesNotExist);
            //when we load the details
            var loadedDetails = storage.Load();
            //then the collection is just empty
            CollectionAssert.IsEmpty(loadedDetails.Sage50DataLocations);
        }       
        
        [Test]
        public void WhenPathDoesNotExistItIsCreated()
        {
            //given a storage pointing at a path which does not exist
            var directoryWhichDoesNotExist = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var fileOnPathWhichDoesnNotExist = Path.Combine(directoryWhichDoesNotExist, Path.GetRandomFileName());            
            var storage = new UserDetailsStorage(new FileSystem(), fileOnPathWhichDoesnNotExist);

            //and some details
            var userDetails = new UserDetails();
            userDetails.AddSage50DataLocation("something");

            //when we try to save the details            
            storage.Save(userDetails);


            //then the collection is saved and can be retrieved successfully
            var loadedDetails = storage.Load();
            CollectionAssert.AreEqual(new[]{"something"}, loadedDetails.Sage50DataLocations);
        }

        [Test]
        public void CanRecallSavedDataFromDefaultLocation()
        {
            //given a storage which uses the default location
            var storage = new UserDetailsStorage(new FileSystem());

            //which has some data saved in it
            var savedDetails = new UserDetails();
            savedDetails.AddSage50DataLocation("a location");                        
            storage.Save(savedDetails);

            //then the details can be loaded correctly
            var loadedDetails = storage.Load();
            CollectionAssert.AreEqual(new[] { "a location" }, loadedDetails.Sage50DataLocations);
        }  

        private static UserDetailsStorage CreateStorage()
        {
            return new UserDetailsStorage(new FileSystem(), Path.GetTempFileName());
        }
    }
}
