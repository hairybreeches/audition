using System;
using System.Collections.Generic;
using System.IO;
using CsvExport;
using Native;
using Native.Disk;

namespace Tests.Mocks
{
    public class MockFileSystem : IFileSystem
    {
        private readonly IDictionary<string, string> files = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

        public StreamWriter OpenFileToWrite(string filename)
        {
            var stream = new CopyingStream(value => files[filename] = value);
            return new StreamWriter(stream);
        }

        public StreamReader OpenFileToRead(string filename)
        {
            throw new ShouldNotHappenInThisTestException();
        }

        public void DeleteFile(string filename)
        {
            throw new ShouldNotHappenInThisTestException();
        }

        public bool DirectoryExists(string directoryName)
        {
            throw new ShouldNotHappenInThisTestException();
        }

        public void EnsureDirectoryExists(string directory)
        {
            throw new ShouldNotHappenInThisTestException();
        }

        public bool FileExists(string filename)
        {
            throw new ShouldNotHappenInThisTestException();
        }

        public Stream OpenFileStreamToRead(string filename)
        {
            throw new ShouldNotHappenInThisTestException();
        }

        public TempFile GetTempFile(string extension = "tmp")
        {
            throw new ShouldNotHappenInThisTestException();
        }

        public string GetFileValue(string filename)
        {
            return files[filename];
        }


        private class CopyingStream : Stream
        {
            private readonly Stream stream = new MemoryStream();
            private readonly Action<string> onDisposal;

            public CopyingStream(Action<string> onDisposal)
            {
                this.onDisposal = onDisposal;
            }

            public override void Flush()
            {
                stream.Flush();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return stream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                stream.SetLength(value);
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return stream.Read(buffer, offset, count);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                stream.Write(buffer, offset, count);
            }

            public override bool CanRead
            {
                get { return stream.CanRead; }
            }

            public override bool CanSeek
            {
                get { return stream.CanSeek; }
            }

            public override bool CanWrite
            {
                get { return stream.CanWrite; }
            }

            public override long Length
            {
                get { return stream.Length; }
            }

            public override long Position
            {
                get { return stream.Position; }
                set { stream.Position = value; }
            }

            protected override void Dispose(bool disposing)
            {
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    onDisposal(reader.ReadToEnd());
                }
            }
        }
    }
}