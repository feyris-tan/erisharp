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

            public FILE_HEADER Clone()
            {
                FILE_HEADER result = new FILE_HEADER();
                result.cHeader = (byte[])this.cHeader.Clone();
                result.dwFileID = this.dwFileID;
                result.dwReserved = this.dwReserved;
                result.cFormatDesc = (byte[])this.cFormatDesc.Clone();
                return result;
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
	    public virtual EMCFile Duplicate()
        {
            EMCFile pDupFile = new EMCFile();
            pDupFile.m_pFile = pDupFile.m_pOwnFile = m_pFile;
            pDupFile.m_pFile.SeekLarge(m_pFile.GetLargePosition(), SeekOrigin.Begin);
            pDupFile.m_fhHeader = m_fhHeader.Clone();

            pDupFile.m_pRecord = null;
            if (m_pRecord != null)
            {
                RECORD_INFO pSrcRec = m_pRecord;
                RECORD_INFO pDstRec = new RECORD_INFO();
                pDupFile.m_pRecord = pDstRec;
                for (; ; )
                {
                    pDstRec.dwWriteFlag = pSrcRec.dwWriteFlag;
                    pDstRec.qwBasePos = pSrcRec.qwBasePos;
                    pDstRec.rechdr = pSrcRec.rechdr;

                    if (pSrcRec.pParent == null)
                    {
                        pDstRec.pParent = null;
                        break;
                    }
                    else
                    {
                        pDstRec.pParent = new RECORD_INFO();
                        pSrcRec = pSrcRec.pParent;
                        pDstRec = pDstRec.pParent;
                    }
                }
            }
            return pDupFile;
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
            if (m_pRecord != null)
            {
                if (m_pRecord.dwWriteFlag != 0)
                {
                    ESLAssert(m_pFile.CanWrite);
                    ulong nPos = GetLargePosition();
                    if (nPos > m_pRecord.rechdr.nRecLength)
                    {
                        m_pRecord.rechdr.nRecLength = nPos;
                    }
                    m_pFile.SeekLarge(m_pRecord.qwBasePos - 16, SeekOrigin.Begin);
                    m_pFile.Write(m_pRecord.rechdr);
                }
            }
            SeekLarge((long)GetLargeLength(), SeekOrigin.Begin);

            RECORD_INFO pRec = m_pRecord;
            m_pRecord = m_pRecord.pParent;
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
            ESLAssert(m_pFile != null);
            ESLAssert(m_pRecord != null);
            ulong nPos = m_pFile.GetLargePosition() - m_pRecord.qwBasePos;
            if (m_pRecord.dwWriteFlag != 0)
            {
                if (nPos > m_pRecord.rechdr.nRecLength)
                {
                    m_pRecord.rechdr.nRecLength = nPos;
                }
            }
            uint nRecLength = (uint)m_pRecord.rechdr.nRecLength;

            switch (fSeekFrom)
            {
                case SeekOrigin.Begin:
                default:
                    break;
                case SeekOrigin.Current:
                    nOffsetPos += (long)nPos;
                    break;
                case SeekOrigin.End:
                    nOffsetPos += nRecLength;
                    break;
            }

            if (m_pRecord.dwWriteFlag != 0)
            {
                if ((uint)nOffsetPos > m_pRecord.rechdr.nRecLength)
                {
                    m_pRecord.rechdr.nRecLength = (ulong)nOffsetPos;
                }
            }
            else
            {
                if ((uint)nOffsetPos > m_pRecord.rechdr.nRecLength)
                {
                    nOffsetPos = (int)m_pRecord.rechdr.nRecLength;
                }
            }

            return m_pFile.SeekLarge((ulong)((long)nOffsetPos + (long)m_pRecord.qwBasePos), SeekOrigin.Begin) - m_pRecord.qwBasePos;
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
