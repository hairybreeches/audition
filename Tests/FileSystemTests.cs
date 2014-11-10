using System.IO;
using Excel;
using Native;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class FileSystemTests
    {
        [Test]
        public void CanWriteToFile()
        {
            var fileSystem = new FileSystem();
            var fileName = Path.GetTempFileName();

            const string textToWrite = "I am some text";

            using (var writer = fileSystem.OpenFileToWrite(fileName))
            {                
                writer.Write(textToWrite);
            }

            var fileContents = File.ReadAllText(fileName);

            Assert.AreEqual(textToWrite, fileContents);

        }
    }
}
