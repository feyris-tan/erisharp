using System;
using System.IO;

namespace ERIShArp.File
{
    /// <summary>
    /// This class is not present in the original. In turns a physical .NET Stream into something EMC understands.
    /// </summary>
    public class PhysicalFile : EMCFile
    {
        Stream parent;

        public PhysicalFile(Stream s)
        {
            parent = s;
        }

        public bool Writeable
        {
            get
            {
                return parent.CanWrite;
            }
        }

        public override string ToString()
        {
            return "Encapsulated " + parent.GetType().Name;
        }

        public override uint GetLength()
        {
            return (uint)parent.Length;
        }

        public override uint Read(byte[] ptrBuffer, uint nBytes)
        {
            return (uint)parent.Read(ptrBuffer, 0, (int)nBytes);
        }

        public override ulong GetLargePosition()
        {
            if (!this.parent.CanRead && !this.parent.CanSeek)
            {
                //looks like it's already closed.
                return 0;
            }
            return (ulong)parent.Position;
        }

        public override ulong GetLargeLength()
        {
            return (ulong)parent.Length;
        }

        public override uint GetPosition()
        {
            return (uint)parent.Position;
        }

        public override ulong SeekLarge(long nOffsetPos, SeekOrigin fSeekFrom)
        {
            if (!this.parent.CanRead && !this.parent.CanSeek)
            {
                //looks like it's already closed.
                return 0;
            }
            return (ulong)parent.Seek(nOffsetPos, fSeekFrom);
        }

        public double PlaybackProgress
        {
            get
            {
                return (double)GetLargePosition() / (double)GetLargeLength();
            }
        }
    }
}
