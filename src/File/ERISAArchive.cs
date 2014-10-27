using System;
using ERIShArp.Context;
using System.IO;
using System.Collections.Generic;

namespace ERIShArp.File
{
    class ERISAArchive : EMCFile
    {
	    public ERISAArchive()
        {
            m_pCurDir = null;
            m_pCurFile = null;
            m_pBufFile = null;
            m_pDecERISA = null;
            m_pDecBSHF = null;
            m_pEncERISA = null;
            m_pEncBSHF = null;
        }
	    ~ERISAArchive()
        {
            Close();
        }

	    public class FILE_TIME
	    {
            public FILE_TIME()
            {
            }
		    public byte	nSecond ;
            public byte nMinute;
            public byte nHour;
            public byte nWeek;
            public byte nDay;
            public byte nMonth;
            public uint nYear;

            public override string ToString()
            {
                return new DateTime((int)nYear, nMonth, nDay, nHour, nMinute, nSecond).ToString();
            }
	    }
	    public class	FILE_INFO
	    {
		    public ulong		nBytes ;
            public uint dwAttribute;
            public uint dwEncodeType;
            public ulong nOffsetPos;
            public FILE_TIME ftFileTime;
            public uint dwExtraInfoBytes;
            public byte[] ptrExtraInfo;
            public uint dwFileNameLen;
            public char[] ptrFileName;

		    public FILE_INFO()
            {
                nBytes = 0;
                dwAttribute = (uint)FileAttribute.attrNormal;
                dwEncodeType = (uint)EncodeType.etRaw;
                nOffsetPos = 0;
                ftFileTime = new FILE_TIME();
                ftFileTime.nSecond = 0;
                ftFileTime.nMinute = 0;
                ftFileTime.nHour = 0;
                ftFileTime.nWeek = 0;
                ftFileTime.nDay = 0;
                ftFileTime.nMonth = 0;
                ftFileTime.nYear = 0;
                dwExtraInfoBytes = 0;
                ptrExtraInfo = null;
                dwFileNameLen = 0;
                ptrFileName = null;
            }
		    public FILE_INFO(FILE_INFO finfo )       
            {
                throw new NotImplementedException();
            }
		    ~FILE_INFO()
            {
                if (ptrExtraInfo != null)
                {
                    ptrExtraInfo = null;
                }
                if (ptrFileName != null)
                {
                    ptrFileName = null;
                }
            }
		    public void SetExtraInfo( byte[] pInfo, uint dwBytes )
            {
                throw new NotImplementedException();
            }
		    public void SetFileName( string pszFileName )
            {
                if (ptrFileName != null)
                {
                    ptrFileName = null;
                }
                int nLen = 0;
                while (pszFileName[nLen] != '\0')
                    ++nLen;

                dwFileNameLen = (uint)(nLen + 1);
                ptrFileName = new char[dwFileNameLen];
                for (int i = 0; i < nLen; i++)
                {
                    ptrFileName[i] = pszFileName[i];
                }
            }

            public override string ToString()
            {
                return new string(ptrFileName);
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
	    
	    public class	EDirectory : SortedDictionary<string,FILE_INFO>
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
                this.Clear();
            }
		    public void CopyDirectory(EDirectory dir )       
            {
                throw new NotImplementedException();
            }
		    public FILE_INFO GetFileInfo( string pszFileName )
            {
                FILE_INFO pInfo = this[pszFileName];
                if (pInfo != null)
                {
                    return pInfo;
                }
                return null;
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
	    
        public void Open ( EMCFile pfile, EDirectory pRootDir = null )
        {
            Close();
            if ((pfile.GetLength() == 0) && (pRootDir != null))
            {
                FILE_HEADER pfhHeader = null;
                FILE_HEADER fhHeader = new FILE_HEADER();
                SetFileHeader(fhHeader, (uint)FileIdentity.fidArchive);
                base.Open(pfile, fhHeader);

                ESLAssert(pRootDir != null);
                m_pCurDir = new EDirectory();
                m_pCurDir.m_dwWriteFlag = 1;
                m_pCurDir.CopyDirectory(pRootDir);
                WriteinDirectoryEntry();
            }
            else
            {
                base.Open(pfile, null);
                m_pCurDir = new EDirectory();
                LoadDirectoryEntry();
            }
        }

	    public  void Close()
        {
            if (m_pCurFile != null)
            {
                AscendFile();
            }
            if (m_pBufFile != null)
            {
                m_pBufFile.Dispose();
                m_pBufFile = null;
            }
            while (m_pCurDir != null)
            {
                if (m_pCurDir.m_dwWriteFlag != 0)
                {
                    SeekLarge(0, SeekOrigin.Begin);
                    WriteinDirectoryEntry();
                }
                EDirectory pDir = m_pCurDir;
                m_pCurDir = m_pCurDir.m_pParent;
                if (m_pCurDir == null)
                {
                    break;
                }
                AscendRecord();
            }
            base.Close();
        }

	    protected virtual void DescendRecord( ulong[] pRecID = null )
        {
            base.DescendRecord(pRecID);
        }
	    protected virtual void AscendRecord()
        {
            base.AscendRecord();
        }
	
	    public void GetFileEntries( EDirectory dirFiles )
        {
            throw new NotImplementedException();
        }
	    public EDirectory  ReferFileEntries()
		{
            ESLAssert(m_pCurDir != null);
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
            ESLAssert(m_pCurDir != null);
            DescendRecord();
            if (m_pRecord.rechdr.ToString() != "DirEntry")
            {
                AscendRecord();
                throw new Exception("doesn't look like an archive to me.");
            }
            EDirectory pParentDir = m_pCurDir;
            m_pCurDir = new EDirectory();
            m_pCurDir.m_pParent = pParentDir;
            uint dwEntryCount;
            dwEntryCount = ReadUInt32();

            for (uint i = 0; i < dwEntryCount; i++)
            {
                FILE_INFO pfinfo = new FILE_INFO();
                pfinfo.nBytes = ReadUInt64();
                pfinfo.dwAttribute = ReadUInt32();
                pfinfo.dwEncodeType = ReadUInt32();
                pfinfo.nOffsetPos = ReadUInt64();
                pfinfo.ftFileTime = ReadFileTime();
                uint dwExtraInfoBytes;
                dwExtraInfoBytes = ReadUInt32();
                if (dwExtraInfoBytes > 0)
                {
                    byte[] buf = new byte[dwExtraInfoBytes];
                    Read(buf, dwExtraInfoBytes);
                    pfinfo.SetExtraInfo(buf, dwExtraInfoBytes);
                }
                uint dwFileNameLen = ReadUInt32();
                if (dwFileNameLen > 0)
                {
                    string strFileName;
                    byte[] pszFileName = new byte[dwFileNameLen];
                    if (Read(pszFileName, dwFileNameLen) != dwFileNameLen) throw new EndOfStreamException();
                    if ((pfinfo.dwAttribute & (uint)(FileAttribute.attrFileNameUTF8)) != 0)
                    {
                        strFileName = System.Text.Encoding.UTF8.GetString(pszFileName);
                    }
                    else
                    {
                        strFileName = System.Text.Encoding.ASCII.GetString(pszFileName);
                    }
                    pfinfo.SetFileName(strFileName);
                }
                m_pCurDir.Add(new string(pfinfo.ptrFileName), pfinfo);
            }
            AscendRecord();
        }
        protected virtual void OnProcessFileData(uint dwCurrent, uint dwTotal)
        {
            throw new NotImplementedException();
        }

	    public virtual Stream Duplicate()
        {
            throw new NotImplementedException();
        }

        private FILE_TIME ReadFileTime()
        {
            byte[] buffer = new byte[8];
            if (Read(buffer, 8) != 8) throw new EndOfStreamException();

            FILE_TIME result = new FILE_TIME();
            result.nSecond = buffer[0];
            result.nMinute = buffer[1];
            result.nHour = buffer[2];
            result.nWeek = buffer[3];
            result.nDay = buffer[4];
            result.nMonth = buffer[5];
            result.nYear = BitConverter.ToUInt16(buffer,6);
            return result;
        }
        private int ReadInt32()
        {
            byte[] buffer = new byte[4];
            if (Read(buffer, 4) != 4) throw new Exception();
            return BitConverter.ToInt32(buffer, 0);
        }
        private uint ReadUInt32()
        {
            byte[] buffer = new byte[4];
            if (Read(buffer, 4) != 4) throw new Exception();
            return BitConverter.ToUInt32(buffer, 0);
        }
        private ulong ReadUInt64()
        {
            byte[] buffer = new byte[8];
            if (Read(buffer, 8) != 8) throw new Exception();
            return BitConverter.ToUInt64(buffer, 0);
        }
	    public virtual uint Read( byte[] ptrBuffer, uint nBytes )
        {
            if (m_pBufFile != null)
            {
                return (uint)m_pBufFile.Read(ptrBuffer, 0, (int)nBytes);
            }
            else if ((m_pDecERISA != null) || (m_pDecBSHF != null))
            {
                uint nReadBytes = nBytes;
                byte[] ptrDst = ptrBuffer;
                if (m_qwWrittenBytes + nReadBytes >= m_pCurFile.nBytes)
                {
                    nReadBytes = (uint)(m_pCurFile.nBytes - m_qwWrittenBytes);
                }
                if (m_pDecERISA != null)
                {
                    nReadBytes = m_pDecERISA.DecodeERISANCodeBytes(ptrDst, nReadBytes);
                }
                else
                {
                    nReadBytes = m_pDecBSHF.DecodeBSHFCodeBytes(ptrDst, nReadBytes);
                }
                for (uint i = 0; i < nReadBytes; i++)
                {
                    m_bufCRC[m_iCRC] ^= ptrDst[i];
                    m_iCRC = (m_iCRC + 1) & 0x03;
                }
                m_qwWrittenBytes += nReadBytes;
                return nReadBytes;
            }
            else
            {
                return base.Read(ptrBuffer, nBytes);
            }
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
	    public EMCFile OpenFileObject( string pwszFileName, int nOpenFlags )
        {
            if (m_pCurDir == null)
            {
                return null;
            }
            FILE_INFO pfi = m_pCurDir.GetFileInfo(pwszFileName);
            if (pfi == null)
            {
                return null;
            }
            EMCFile pNewFile = base.Duplicate();
            EMCFile pEMCFile = pNewFile;
            if (pEMCFile == null)
            {
                pNewFile = null;
                return null;
            }
            if (m_pCurFile != null)
            {
                pEMCFile.AscendRecord();
            }
            pEMCFile.SeekLarge((long)pfi.nOffsetPos, SeekOrigin.Begin);
            pEMCFile.DescendRecord();
            return pEMCFile;
        }

    }
}
