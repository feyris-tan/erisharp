using System;
using ERIShArp.Context;
using System.IO;

namespace ERIShArp.File
{
    class ERISAArchive : EMCFile
    {
	    public ERISAArchive()
        {
            throw new NotImplementedException();
        }
	    ~ERISAArchive()
        {
            throw new NotImplementedException();
        }

	    public class	FILE_TIME
	    {
		    public byte	nSecond ;
            public byte nMinute;
            public byte nHour;
            public byte nWeek;
            public byte nDay;
            public byte nMonth;
            public uint nYear;
	    }
	    public class	FILE_INFO
	    {
		    ulong		nBytes ;
		    uint		dwAttribute ;
		    uint		dwEncodeType ;
		    ulong		nOffsetPos ;
		    FILE_TIME	ftFileTime ;
		    uint		dwExtraInfoBytes ;
		    byte[]		ptrExtraInfo ;
		    uint		dwFileNameLen ;
		    byte[]		ptrFileName ;

		    public FILE_INFO()
            {
                throw new NotImplementedException();
            }
		    public FILE_INFO(FILE_INFO finfo )       
            {
                throw new NotImplementedException();
            }
		    ~FILE_INFO()
            {
                throw new NotImplementedException();
            }
		    public void SetExtraInfo( byte[] pInfo, uint dwBytes )
            {
                throw new NotImplementedException();
            }
		    public void SetFileName( string pszFileName )
            {
                throw new NotImplementedException();
            }
	    }
	    public struct	FILE_EXTRA_INFO
	    {
		    uint		dwCRC32 ;			
            /// <summary>
            /// 1 entries
            /// </summary>
		    uint[]		dwDecrypeKey;	
	    }
	    public enum	FileAttribute
	    {
		    attrNormal			= 0x00000000,
		    attrReadOnly		= 0x00000001,
		    attrHidden			= 0x00000002,
		    attrSystem			= 0x00000004,
		    attrDirectory		= 0x00000010,
		    attrEndOfDirectory	= 0x00000020,
		    attrNextDirectory	= 0x00000040,
		    attrFileNameUTF8	= 0x01000000,
	    }
	    public enum	EncodeType : uint
	    {
		    etRaw				= 0x00000000,
		    etERISACode			= 0x80000010,
		    etBSHFCrypt			= 0x40000000,
		    etSimpleCrypt32		= 0x20000000,
		    etERISACrypt		= 0xC0000010,
		    etERISACrypt32		= 0xA0000010,
	    }
	    
	    public class	EDirectory
	    {
		    public string			m_strName ;
		    public EDirectory	m_pParent ;
		    public uint			m_dwWriteFlag ;
		    public EDirectory()
            {
                m_pParent = null;
                m_dwWriteFlag = 0;
            }
		    ~EDirectory() 
            { 
            }
		    public void CopyDirectory(EDirectory dir )       
            {
                throw new NotImplementedException();
            }
		    public FILE_INFO GetFileInfo( string pszFileName )
            {
                throw new NotImplementedException();
            }
		    public void AddFileEntry( string pszFileName, uint dwAttribute = 0, uint dwEncodeType = (uint)EncodeType.etRaw, FILE_TIME pTime = null )
            {
                throw new NotImplementedException();
            }
	    }

        public enum	OpenType
	    {
		    otNormal,
		    otStream
	    }

	    protected EDirectory			m_pCurDir ;
	    protected FILE_INFO				m_pCurFile ;

	    protected ERISADecodeContext 	m_pDecERISA ;
	    protected ERISADecodeContext 	m_pDecBSHF ;
	    protected Stream			m_pBufFile ;		


    	protected ERISAEncodeContext 	m_pEncERISA ;
	    protected ERISAEncodeContext 	m_pEncBSHF ;

	    protected ulong					m_qwWrittenBytes ;
        /// <summary>
        /// 4 bytes
        /// </summary>
	    protected byte[]					m_bufCRC;	
	    protected int						m_iCRC ;
	    
        protected void Open ( Stream pfile, EDirectory pRootDir = null )
        {
            throw new NotImplementedException();
        }
	    protected void Close()
        {
            throw new NotImplementedException();
        }
	    protected virtual void DescendRecord( uint[] pRecID = null )
        {
            throw new NotImplementedException();
        }
	    protected virtual void AscendRecord()
        {
            throw new NotImplementedException();
        }
	
	    public void GetFileEntries( EDirectory dirFiles )
        {
            throw new NotImplementedException();
        }
	    public EDirectory  ReferFileEntries()
		{
            if (m_pCurDir != null) throw new Exception();
			return	m_pCurDir ;
		}
	    public void DescendDirectory( string pszDirName, EDirectory pDir = null )
        {
            throw new NotImplementedException();
        }
	    public void AscendDirectory()
        {
            throw new NotImplementedException();
        }
	    public void DescendFile( string pszFileName, string pszPassword = null, OpenType otType = OpenType.otNormal )
        {
            throw new NotImplementedException();
        }
	    public void AscendFile()
        {
            throw new NotImplementedException();
        }
	    public void OpenDirectory( string pszDirPath )
        {
            throw new NotImplementedException();
        }
	    public void OpenFile( string pszFilePath, string pszPassword = null, OpenType otType = OpenType.otNormal )
        {
            throw new NotImplementedException();
        }
	    protected void WriteinDirectoryEntry()
        {
            throw new NotImplementedException();
        }
	    protected void LoadDirectoryEntry()
        {
            throw new NotImplementedException();
        }
	    protected virtual void OnProcessFileData( uint dwCurrent, uint dwTotal )
        {
            throw new NotImplementedException();
        }

	    public virtual Stream Duplicate()
        {
            throw new NotImplementedException();
        }
	    public virtual uint Read( byte[] ptrBuffer, uint nBytes )
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
	    public Stream OpenFileObject( char[] pwszFileName, int nOpenFlags )
        {
            throw new NotImplementedException();
        }

    }
}
