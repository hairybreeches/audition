using System;
using System.Collections.Generic;
using Native;
using NSubstitute;
using NUnit.Framework;
using Sage50;
using Tests.Mocks;
using UserData;

namespace Tests
{
    [TestFixture]
    public class Sage50DataDirectoryStorageTests
    {

        public IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(new string[0], new[] { "Sage Line 50 v20" }, new string[0], new string[0])
                    .SetName("Does Not Return Non-existent DemoData Directories");
                
                yield return new TestCaseData(
                        new[] {"C:\\programdata\\sage\\accounts\\2014\\demodata"},
                        new[] {"Sage Line 50 v20"},
                        new string[0],
                        new[] {"C:\\programdata\\sage\\accounts\\2014\\demodata"})
                    .SetName("Returns Existing DemoDataD irectories");
                
                yield return new TestCaseData(
                        new[] {"C:\\programdata\\sage\\accounts\\2014\\demodata"},
                        new[] {"Sage Line 50 v20"},
                        new[] { "C:\\programdata\\sage\\accounts\\2014\\demodata" },
                        new[] {"C:\\programdata\\sage\\accounts\\2014\\demodata"})
                    .SetName("Does not return duplicates when user has used a demo data folder");

                yield return new TestCaseData(new[]
                {
                    "C:\\programdata\\sage\\accounts\\2010\\demodata", 
                    "C:\\programdata\\sage\\accounts\\2012\\demodata"
                }, new[]
                {
                    "Sage Line 50 v16",
                    "Sage Line 50 v17",
                    "Sage Line 50 v18"
                }, new[]
                {
                    "C:\\first\\place",
                    "c:\\second\\place"
                }, new[]
            {
                "c:\\second\\place",
                "C:\\first\\place",
                "C:\\programdata\\sage\\accounts\\2012\\demodata",
                "C:\\programdata\\sage\\accounts\\2010\\demodata"                
            }).SetName("Returns User Supplied Directories First");


            }
        }
        
        [TestCaseSource("TestCases")]
        public void ReturnsCorrectDataDirectories(string[] existentDirectories, string[] sageDriversInstalled, string[] directoriesUsed, string[] expected)
        {
            var returnedDirectories = GetDataDirectories(
                existentDirectories, 
                sageDriversInstalled, 
                directoriesUsed);
            CollectionAssert.AreEqual(expected, returnedDirectories, StringComparer.InvariantCultureIgnoreCase);
        }

        

        private static IEnumerable<string> GetDataDirectories(string[] existentDirectories, string[] sageDriversInstalled, params string[] directoriesUsed)
        {                        
            var registry = new MockRegistry().SetSage50Drivers(sageDriversInstalled);
            var fileSystem = CreateFileSystem(existentDirectories);
            var userDetails = CreateUserDetails(directoriesUsed);

            var storage = CreateSage50DataDirectoryStorage(fileSystem, registry, userDetails);

            return storage.GetSageDataDirectories();
        }

        private static Sage50DataDirectoryStorage CreateSage50DataDirectoryStorage(IFileSystem fileSystem, ILocalMachineRegistry registry, UserDetails userDetails)
        {
            return new Sage50DataDirectoryStorage(new MockUserDetailsStorage(userDetails), fileSystem, new Sage50DriverDetector(new OdbcRegistryReader(registry)));
        }

        private static UserDetails CreateUserDetails(IEnumerable<string> directoriesUsed)
        {
            var userDetails = new UserDetails();
            foreach (var directory in directoriesUsed)
            {
                userDetails.AddSage50DataLocation(directory);
            }
            return userDetails;
        }

        private static IFileSystem CreateFileSystem(IEnumerable<string> expectedDirectories)
        {
            var fileSystem = Substitute.For<IFileSystem>();
            foreach (var expectedDirectory in expectedDirectories)
            {
                var directory = expectedDirectory;
                fileSystem.DirectoryExists(Arg.Is<string>(x => directory.Equals(x, StringComparison.InvariantCultureIgnoreCase)))
                    .Returns(true);
            }
            return fileSystem;
        }
    }
}
