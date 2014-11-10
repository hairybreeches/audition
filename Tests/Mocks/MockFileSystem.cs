using System;
using System.Collections.Generic;
using System.IO;
using Excel;
using Native;

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