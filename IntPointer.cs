using System;
using System.IO;

namespace ERIShArp
{
    public class IntPointer
    {
        public IntPointer(uint length)
        {
            data = new int[length];
            offset = 0;
        }

        public IntPointer(int[] data, uint offset)
        {
            this.data = data;
            this.offset = offset;
        }

        int[] data;
        uint offset;

        public int[] Data
        {
            get { return data; }
            set { data = value; }
        }
        

        public uint Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public override string ToString()
        {
            return String.Format("Points to Integer #{0} ({2}) from a set of {1} integers.",offset,data.Length,data[offset]);
        }

        public IntPointer Clone()
        {
            return new IntPointer(this.data, this.offset);
        }

        public void Dump(string target)
        {
            uint dumpStart = this.offset;
            BinaryWriter bw = new BinaryWriter(System.IO.File.OpenWrite(target));
            while (dumpStart != data.Length)
            {
                bw.Write(Data[dumpStart++]);
            }
            bw.Flush();
            bw.Close();
        }
    }
}
