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
            throw new NotImplementedException();
        }
        ~EMCFile()
        {
            throw new NotImplementedException();
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
		    byte[]	cHeader;			
		    uint	dwFileID ;				
		    uint	dwReserved ;
			/// <summary>
			/// 0x30 entries
			/// </summary>
		    byte[]	cFormatDesc;		
	    }
	    protected struct	RECORD_HEADER
	    {
    		public ulong			nRecordID ;
	    	public ulong			nRecLength ;	
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
	    protected static byte[]	m_cDefSignature;

	    public void Open( Stream pfile, FILE_HEADER pfhHeader = null)
        {
            throw new NotImplementedException();
        }
	    public void Close()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
	    public static void SetFileSignature( byte[] cHeader )
        {
            throw new NotImplementedException();
        }

	    public virtual void DescendRecord( ulong[] pRecID = null )
        {
            throw new NotImplementedException();
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

	    public virtual uint Read(byte[] ptrBuffer, uint nBytes )
        {
            throw new NotImplementedException();
        }
	    public virtual uint Write( byte[] ptrBuffer, uint nBytes )
        {
            throw new NotImplementedException();
        }
	    public virtual ulong GetLargeLength()
        {
            throw new NotImplementedException();
        }
	    public virtual uint GetLength()
        {
            throw new NotImplementedException();
        }
	    public virtual ulong SeekLarge( long nOffsetPos, SeekOrigin fSeekFrom )
        {
            throw new NotImplementedException();
        }
	    public virtual uint Seek( int nOffsetPos, SeekOrigin fSeekFrom )
        {
            throw new NotImplementedException();
        }
	    public virtual ulong GetLargePosition()
        {
            throw new NotImplementedException();
        }
	    public virtual uint GetPosition()
        {
            throw new NotImplementedException();
        }
	    public virtual void SetEndOfFile()
        {
            throw new NotImplementedException();
        }
    }
}
