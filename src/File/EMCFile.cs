using System;
using System.IO;

namespace ERIShArp.File
{
    /// <summary>
    /// might need to implement ESLFileObject
    /// </summary>
    public class	EMCFile
    {
	    public EMCFile()
        {
            m_pFile = null;
            m_pOwnFile = null;
            m_pRecord = null;
        }
        ~EMCFile()
        {
            Close();
        }
	    
        public enum	FileIdentity
	    {
		    fidArchive			= 0x02000400,
		    fidRasterizedImage	= 0x03000100,
		    fidEGL3DSurface		= 0x03001100,
		    fidEGL3DModel		= 0x03001200,
		    fidUndefinedEMC		= -1
	    }
	    public class	FILE_HEADER
	    {
            /// <summary>
            /// 8 entries
            /// </summary>
		    public byte[]	cHeader;
            public uint dwFileID;
            public uint dwReserved;
			/// <summary>
			/// 0x30 entries
			/// </summary>
            public byte[] cFormatDesc;

            public string FormatDescription
            {
                get
                {
                    return System.Text.Encoding.ASCII.GetString(cFormatDesc);
                }
            }
	    }
	    
        public struct	RECORD_HEADER
	    {
    		public ulong			nRecordID ;
	    	public ulong			nRecLength ;

            public override string ToString()
            {
                return System.Text.Encoding.ASCII.GetString(BitConverter.GetBytes(nRecordID));
            }
	    }

	    protected class	RECORD_INFO
	    {
		    public RECORD_INFO 	pParent ;
            public uint dwWriteFlag;
            public ulong qwBasePos;
            public RECORD_HEADER rechdr;		
	    }
	    protected Stream	m_pFile ;
	    protected Stream	m_pOwnFile ;
	    protected RECORD_INFO 	m_pRecord ;
	    protected FILE_HEADER		m_fhHeader ;

        /// <summary>
        /// 8 elemets
        /// </summary>
	    protected static byte[]	m_cDefSignature = { (byte)'E', (byte)'n', (byte)'t', (byte)'i', (byte)'s', 0x1A, 0, 0 };

	    public void Open( Stream pfile, FILE_HEADER pfhHeader = null)
        {
            Close();
            ESLAssert(pfile != null);
            m_pFile = pfile;
            if ((pfile.Length == 0) && (pfhHeader != null))
            {
                m_pFile.Write(pfhHeader);
                m_pRecord = new RECORD_INFO();
                m_pRecord.pParent = null;
                m_pRecord.dwWriteFlag = 1;
                m_pRecord.qwBasePos = (ulong)m_pFile.Position;
                m_pRecord.rechdr.nRecordID = 0;
                m_pRecord.rechdr.nRecLength = 0;
            }
            else
            {
                m_fhHeader = pfile.ReadEMCFileHeader();
                if ((m_fhHeader.dwReserved != 0) || !m_fhHeader.cHeader.Compare(m_cDefSignature))
                {
                    throw new Exception("Invalid file header.");
                }
                m_pRecord = new RECORD_INFO();
                m_pRecord.pParent = null;
                m_pRecord.dwWriteFlag = 0;
                m_pRecord.qwBasePos = m_pFile.GetLargePosition();
                m_pRecord.rechdr.nRecordID = 0;
                m_pRecord.rechdr.nRecLength = m_pFile.GetLargeLength() - m_pRecord.qwBasePos;
            }
        }
	    public void Close()
        {
            if (m_pFile != null)
            {
                while (m_pRecord != null)
                {
                    AscendRecord();
                }
            }
            while (m_pRecord != null)
            {
                RECORD_INFO pParent = m_pRecord.pParent;
                m_pRecord = pParent;
            }
            if (m_pOwnFile != null) m_pOwnFile.Dispose();
            m_pFile = null;
            m_pOwnFile = null;
        }
	    public virtual Stream Duplicate()
        {
            throw new NotImplementedException();
        }
	    public Stream GetOriginalFile()
		{
			return	m_pFile ;
		}

	    public FILE_HEADER GetFileHeader()
		{
			return	m_fhHeader ;
		}
	    public static void SetFileHeader( FILE_HEADER fhHeader, uint dwFileID, byte[] pszDesc = null )
        {
            throw new NotImplementedException();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="cHeader">8 bytes</param>
	    public static void GetFileSignature( byte[] cHeader )
        {
            for (int i = 0; i < 8; i++)
            {
                cHeader[i] = m_cDefSignature[i];
            }
        }
        public static void SetFileSignature(byte[] cHeader)
        {
            for (int i = 0; i < 8; i++)
            {
                m_cDefSignature[i] = cHeader[i];
            }
        }

        public virtual void DescendRecord(uint[] pRecID = null)
        {
            if (m_pFile.Position == m_pFile.Length)   //creation
            {
                ESLAssert(pRecID != null);
                RECORD_HEADER rechdr;
                rechdr.nRecordID = pRecID[0];
                rechdr.nRecLength = 0;
                m_pFile.Write(rechdr);
                RECORD_INFO pRec = new RECORD_INFO();
                pRec.pParent = m_pRecord;
                pRec.dwWriteFlag = 1;
                pRec.qwBasePos = m_pFile.GetLargePosition();
                pRec.rechdr = rechdr;
                m_pRecord = pRec;
            }
            else
            {
                RECORD_HEADER rechdr;
                for (; ; )
                {
                    rechdr = ReadRecordHeader();
                    if (pRecID == null)
                        break;
                    if (pRecID[0] == rechdr.nRecordID)
                        break;
                    m_pFile.SeekLarge(rechdr.nRecLength, SeekOrigin.Current);
                }
                RECORD_INFO pRec = new RECORD_INFO();
                pRec.pParent = m_pRecord;
                pRec.dwWriteFlag = 0;
                pRec.qwBasePos = m_pFile.GetLargePosition();
                pRec.rechdr = rechdr;
                m_pRecord = pRec;

                if (rechdr.nRecLength == 0xFFFFFFFFFFFFFFFF)
                {
                    pRec.rechdr.nRecLength = m_pFile.GetLargeLength() - pRec.qwBasePos;
                }
            }
        }
	    public virtual void AscendRecord()
        {
            throw new NotImplementedException();
        }
	    public ulong GetRecordID()
		{
            if (m_pRecord != null) throw new Exception();
            return m_pRecord.rechdr.nRecordID;
            
		}
	    public ulong GetRecordLength()
		{
            if (m_pRecord != null) throw new Exception();
            return m_pRecord.rechdr.nRecLength;
		}

        private RECORD_HEADER ReadRecordHeader()
        {
            byte[] buffer = new byte[8];
            if (Read(buffer, 8) != 8) throw new EndOfStreamException();

            RECORD_HEADER rh = new RECORD_HEADER();
            rh.nRecordID = BitConverter.ToUInt64(buffer, 0);

            if (Read(buffer, 8) != 8) throw new EndOfStreamException();
            rh.nRecLength = BitConverter.ToUInt64(buffer, 0);
            return rh;
        }
	    public virtual uint Read(byte[] ptrBuffer, uint nBytes )
        {
            ESLAssert(m_pFile != null);
            ESLAssert(m_pRecord != null);

            long nPos = (long)m_pFile.GetLargePosition() - (long)m_pRecord.qwBasePos;
            if ((ulong)nPos + nBytes > m_pRecord.rechdr.nRecLength)
            {
                nBytes = (uint)((long)m_pRecord.rechdr.nRecLength - nPos);
            }
            if ((int)nBytes < 0)
            {
                nBytes = 0;
            }
            return (uint)m_pFile.Read(ptrBuffer, 0, (int)nBytes);
        }
	    public virtual uint Write( byte[] ptrBuffer, uint nBytes )
        {
            throw new NotImplementedException();
        }
	    public virtual ulong GetLargeLength()
        {
            return m_pRecord.rechdr.nRecLength;
        }
	    public virtual uint GetLength()
        {
            return (uint)m_pRecord.rechdr.nRecLength;
        }
	    public virtual ulong SeekLarge( long nOffsetPos, SeekOrigin fSeekFrom )
        {
            throw new NotImplementedException();
        }
	    public virtual uint Seek( int nOffsetPos, SeekOrigin fSeekFrom )
        {
            return (uint)SeekLarge(nOffsetPos, fSeekFrom);
        }
	    public virtual ulong GetLargePosition()
        {
            return m_pFile.GetLargeLength() - m_pRecord.qwBasePos;
        }
	    public virtual uint GetPosition()
        {
            return (uint)m_pFile.GetLargePosition();
        }
	    public virtual void SetEndOfFile()
        {
            throw new NotImplementedException();
        }

        protected static void ESLAssert(bool condition)
        {
            if (!condition)
                throw new Exception();
        }
    }
}
