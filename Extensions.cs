using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERIShArp.File;
using System.IO;

namespace ERIShArp
{
    static class Extensions
    {
        public static int Write(this Stream target, EMCFile.FILE_HEADER a)
        {
            long beforePos = target.Position;
            target.Write(a.cHeader, 0, 8);
            target.Write(BitConverter.GetBytes(a.dwFileID), 0, 4);
            target.Write(BitConverter.GetBytes(a.dwReserved), 0, 4);
            target.Write(a.cFormatDesc, 0, 0x30);
            long afterPos = target.Position;
            return (int)(afterPos - beforePos);
        }

        public static EMCFile.FILE_HEADER ReadEMCFileHeader(this Stream target)
        {
            EMCFile.FILE_HEADER result = new EMCFile.FILE_HEADER();
            result.cHeader = new byte[8];
            result.cFormatDesc = new byte[0x30];
            byte[] buffer = new byte[4];
            if (target.Read(result.cHeader, 0, 8) != 8) throw new EndOfStreamException();
            if (target.Read(buffer, 0, 4) != 4) throw new EndOfStreamException();
            result.dwFileID = BitConverter.ToUInt32(buffer, 0);
            if (target.Read(buffer, 0, 4) != 4) throw new EndOfStreamException();
            result.dwReserved = BitConverter.ToUInt32(buffer, 0);
            if (target.Read(result.cFormatDesc, 0, 0x30) != 0x30) throw new EndOfStreamException();
            return result;
        }

        public static bool Compare(this byte[] source, byte[] target)
        {
            if (source.Length != target.Length) return false;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] != target[i]) return false;
            }
            return true;
        }

        public static ulong GetLargePosition(this Stream target)
        {
            return (ulong)target.Position;
        }

        public static ulong GetLargeLength(this Stream target)
        {
            return (ulong)target.Length;
        }

        public static void Write(this Stream target, EMCFile.RECORD_HEADER rechdr)
        {
            byte[] buffer =  BitConverter.GetBytes(rechdr.nRecordID);
            target.Write(buffer, 0, 8);
            buffer = BitConverter.GetBytes(rechdr.nRecLength);
            target.Write(buffer, 0, 8);
        }

        public static ulong ReadUInt64(this Stream target)
        {
            byte[] buffer = new byte[8];
            if (target.Read(buffer, 0, 8) != 8) throw new EndOfStreamException();
            return BitConverter.ToUInt64(buffer, 0);
        }

        public static ulong SeekLarge(this Stream target, ulong length, SeekOrigin origin)
        {
            return (ulong)target.Seek((long)length, origin);

        }

        public static uint ReadUInt32(this Stream target)
        {
            byte[] buffer = new byte[4];
            if (target.Read(buffer, 0, 4) != 4) throw new EndOfStreamException();
            return BitConverter.ToUInt32(buffer, 0);
        }
    }
}
