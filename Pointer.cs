using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERIShArp
{
    public class Pointer : IChecksum
    {
        public Pointer(byte[] data, int offset)
        {
            this.Data = data;
            this.Offset = offset;
            Valid = true;
        }

        public Pointer(bool valid)
        {
            Valid = valid;
            Offset = -1;
            Data = null;
        }
        
        public byte[] Data;
        public int Offset;
        public bool Valid;

        public Pointer Clone()
        {
            return new Pointer(this.Data, this.Offset);
        }

        public byte PeekByte
        {
            get
            {
                return Data[Offset];
            }
        }

        public bool IsInRange
        {
            get
            {
                return Data.Length > Offset;
            }
        }

        public override string ToString()
        {
            return String.Format("Points to offset {0} in an {1}-byte array", Offset, Data.Length);
        }

        public byte this[int index]
        {
            get 
            {
                return Data[Offset + index];
            }
            set 
            {
                Data[Offset + index] = value;
            }
        }

        public byte this[uint index]
        {
            get
            {
                return Data[(int)(Offset + index)];
            }
            set
            {
                Data[(int)(Offset + index)] = value;
            }
        }

        public static Pointer operator +(Pointer l, int r)
        {
            return new Pointer(l.Data, l.Offset + r);
        }

        public static Pointer operator -(Pointer l, int r)
        {
            return new Pointer(l.Data, l.Offset - r);
        }

        public static bool operator <(Pointer l, Pointer r)
        {
            if (l.Data.Length != r.Data.Length) throw new Exception("bad comparision!");
            return l.Offset < r.Offset;
        }

        public static bool operator >(Pointer l, Pointer r)
        {
            if (l.Data.Length != r.Data.Length) throw new Exception("bad comparision!");
            return l.Offset > r.Offset;
        }

        public ushort PeekUInt16
        {
            get
            {
                return BitConverter.ToUInt16(Data, Offset);
            }
        }

        public uint PeekUInt32
        {
            get
            {
                return BitConverter.ToUInt32(Data, Offset);
            }
        }

        public uint PeekUInt32Flipped
        {
            get
            {
                byte[] flipBfr = new byte[4];
                flipBfr[0] = Data[Offset + 3];
                flipBfr[1] = Data[Offset + 2];
                flipBfr[2] = Data[Offset + 1];
                flipBfr[3] = Data[Offset + 0];
                return BitConverter.ToUInt32(flipBfr, 0);
            }
        }

        public ulong Checksum
        {
            get 
            {
                ulong result = 0;
                for (int i = 0; i < Data.Length; i++)
                {
                    result += (uint)Data[i];
                }
                return result;
            }
        }
    }
}
