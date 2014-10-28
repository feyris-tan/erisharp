using System;
using System.Drawing;
using ERIShArp.X;
using System.IO;
using System.Collections.Generic;

namespace ERIShArp.File
{
    public class ERIFile : EMCFile
    {
	    public enum	TagIndex
	    {
		    tagTitle,				
		    tagVocalPlayer,			
		    tagComposer,			
		    tagArranger,			
		    tagSource,				
		    tagTrack,				
		    tagReleaseDate,			
		    tagGenre,				
		    tagRewindPoint,
		    tagHotSpot,
		    tagResolution,
		    tagComment,
		    tagWords,
		    tagReferenceFile,
		    tagMax
	    }
        /// <summary>
        /// tagMax entries
        /// </summary>
	    public static string[]	m_pwszTagName = { "title", "vocal-player", "composer", "arranger", "source", "track", "release-date", "genre", "rewind-point", "hot-spot", "resolution", 
                                                  "comment", "words", "reference-file"} ;

	    public class	ETagObject
	    {
		    public string		m_tag ;
		    public string		m_contents ;
		    public ETagObject()
            {
            }
		    ~ETagObject()
            {
            }
	    }

	    public class	ETagInfo
	    {
		    public List<ETagObject>	m_lstTags ;
		    public ETagInfo()
            {
            }
		    ~ETagInfo()
            {
            }
		    public void CreateTagInfo( char[] pwszDesc )
            {
                throw new NotImplementedException();
            }
		    public void FormatDescription( string wstrDesc )
            {
                throw new NotImplementedException();
            }
		    public void AddTag( TagIndex tagIndex, string pwszContents )
            {
                ETagObject pTag = new ETagObject();
                pTag.m_tag = m_pwszTagName[(int)tagIndex];
                pTag.m_contents = pwszContents;
                m_lstTags.Add(pTag);
            }
		    public void DeleteContents()
            {
                m_lstTags.Clear();
            }
		    public char[] GetTagContents( string pwszTag ) 
            {
                throw new NotImplementedException();
            }
		    public char[] GetTagContents( TagIndex tagIndex )
            {
                return GetTagContents(ERIFile.m_pwszTagName[(int)tagIndex]);
            }
		    public int GetTrackNumber()
            {
                throw new NotImplementedException();
            }
		    public void GetReleaseDate( ref int year, ref int month, ref int day )
            {
                throw new NotImplementedException();
            }
		    public int GetRewindPoint()
            {
                throw new NotImplementedException();
            }
		    public Point GetHotSpot()
            {
                throw new NotImplementedException();
            }
		    public int GetResolution()
            {
                throw new NotImplementedException();
            }
	    }

	    public class SEQUENCE_DELTA
	    {
		    public uint	dwFrameIndex ;
		    public uint	dwDuration ;
	    }

	    public enum	ReadMask
	    {
		    rmFileHeader	= 0x00000001,
		    rmPreviewInfo	= 0x00000002,
		    rmImageInfo		= 0x00000004,
		    rmSoundInfo		= 0x00000008,
		    rmCopyright		= 0x00000010,
		    rmDescription	= 0x00000020,
		    rmPaletteTable	= 0x00000040,
		    rmSequenceTable	= 0x00000080
	    }

	    uint			m_fdwReadMask ;

	    ERI_FILE_HEADER	m_FileHeader ;
	    ERI_INFO_HEADER	m_PreviewInfo ;
	    ERI_INFO_HEADER	m_InfoHeader ;
	    MIO_INFO_HEADER	m_MIOInfHdr ;

        /// <summary>
        /// 0x100 entries
        /// </summary>
	    uint[]		m_PaletteTable;
	    string		m_wstrCopyright ;
	    string		m_wstrDescription ;

	    protected uint				m_dwSeqLength ;
	    protected SEQUENCE_DELTA[]	m_pSequence ;
    
	    public ERIFile()
        {
            m_dwSeqLength = 0;
            m_pSequence = null;
        }
	    ~ERIFile()
        {
            if (m_pSequence != null) m_pSequence = null;
        }

	    public enum	OpenType
	    {
		    otOpenRoot,			
		    otReadHeader,		
		    otOpenStream,		
		    otOpenImageData		
	    } ;
	    public void Open( EMCFile pFile, OpenType type = OpenType.otOpenImageData )
        {
            m_fdwReadMask = 0;
            m_dwSeqLength = 0;
            if (m_pSequence != null)
            {
                m_pSequence = null;
            }
            base.Open(pFile);
            if (type == OpenType.otOpenRoot)
                return;

            DescendRecord(new ulong[] { BitConverter.ToUInt64(System.Text.Encoding.ASCII.GetBytes("Header  "), 0) });
            DescendRecord(new ulong[] { BitConverter.ToUInt64(System.Text.Encoding.ASCII.GetBytes("FileHdr "), 0) });
            m_FileHeader = ReadERIFileHeader();
            AscendRecord();
            m_fdwReadMask = (uint)ReadMask.rmFileHeader;
            if (m_FileHeader.dwVersion > 0x00020100)
            {
                throw new Exception("version mismatch.");
            }
            for (; ; )
            {
                if (DescendRecord())
                    break;
                ulong ui64RecID = GetRecordID();
                if (System.Text.Encoding.ASCII.GetString(BitConverter.GetBytes(ui64RecID)) == "PrevwInf")
                {
                    m_PreviewInfo = ReadERIInfoHeader();
                    m_fdwReadMask |= (uint)ReadMask.rmPreviewInfo;
                }
                else if (System.Text.Encoding.ASCII.GetString(BitConverter.GetBytes(ui64RecID)) == "ImageInf")
                {
                    m_InfoHeader = ReadERIInfoHeader();
                    m_fdwReadMask |= (uint)ReadMask.rmImageInfo;
                }
                else if (System.Text.Encoding.ASCII.GetString(BitConverter.GetBytes(ui64RecID)) == "SoundInf")
                {
                    m_MIOInfHdr = ReadMIOInfoHeader();
                    m_fdwReadMask |= (uint)ReadMask.rmSoundInfo;
                }
                else if (System.Text.Encoding.ASCII.GetString(BitConverter.GetBytes(ui64RecID)) == "Sequence")
                {
                    uint dwBytes;
                    m_dwSeqLength = GetLength() / 8;
                    dwBytes = m_dwSeqLength * 8;
                    m_pSequence = new SEQUENCE_DELTA[m_dwSeqLength];
                    for (uint i = 0; i < m_dwSeqLength; i++)
                    {
                        m_pSequence[i] = new SEQUENCE_DELTA();
                        m_pSequence[i].dwFrameIndex = ReadUInt32();
                        m_pSequence[i].dwDuration = ReadUInt32();
                    }
                    m_fdwReadMask |= (uint)ReadMask.rmSequenceTable;
                }
                else
                {
                    int nType = -1;
                    if (System.Text.Encoding.ASCII.GetString(BitConverter.GetBytes(ui64RecID)) == "cpyright")
                    {
                        nType = 0;
                        m_fdwReadMask |= (uint)ReadMask.rmCopyright;
                    }
                    else if (System.Text.Encoding.ASCII.GetString(BitConverter.GetBytes(ui64RecID)) == "descript")
                    {
                        nType = 1;
                        m_fdwReadMask |= (uint)ReadMask.rmDescription;
                    }
                    if (nType >= 0)
                    {
                        int nLength = (int)GetLength();
                        byte[] bufDesc = new byte[nLength + 2];
                        Read(bufDesc,(uint)nLength);
                        if ((nLength >= 2) & (bufDesc[0] == 0xFF) & (bufDesc[1] == 0xFE))
                        {
                            if (nType == 0)
                            {
                                m_wstrCopyright = System.Text.Encoding.UTF8.GetString(bufDesc);
                            }
                            else
                            {
                                m_wstrDescription = System.Text.Encoding.UTF8.GetString(bufDesc);
                            }
                        }
                        else
                        {
                            bufDesc[nLength] = 0;
                            if (nType == 0)
                            {
                                m_wstrCopyright = System.Text.Encoding.ASCII.GetString(bufDesc);
                            }
                            else
                            {
                                m_wstrDescription = System.Text.Encoding.ASCII.GetString(bufDesc);
                            }
                        }
                    }
                }
                AscendRecord();
            }
            AscendRecord();
            if (((m_fdwReadMask & (uint)ReadMask.rmImageInfo) != 0) && (((m_fdwReadMask & (uint)ReadMask.rmSoundInfo) != 0)))
            {
                throw new Exception("Image information header was not found.");
            }
            if (type == OpenType.otReadHeader)
            {
                return;
            }

            DescendRecord(new ulong[] { BitConverter.ToUInt64(System.Text.Encoding.ASCII.GetBytes("Stream  "), 0) });
            if (type == OpenType.otOpenStream)
            {
                return;
            }

            for (; ; )
            {
                if (DescendRecord())
                {
                    throw new Exception();
                }
                ulong nRecID = GetRecordID();
                if (System.Text.Encoding.ASCII.GetString(BitConverter.GetBytes(nRecID)) == "ImageFrm")
                {
                    break;
                }
                if (System.Text.Encoding.ASCII.GetString(BitConverter.GetBytes(nRecID)) == "Palette ")
                {
                    m_PaletteTable = new uint[0x100];
                    for (uint i = 0; i < 0x100; i++)
                    {
                        m_PaletteTable[i] = ReadUInt32();
                    }
                    m_fdwReadMask |= (uint)ReadMask.rmPaletteTable;
                }
                AscendRecord();
            }
        }

        ERI_INFO_HEADER ReadERIInfoHeader()
        {
            ERI_INFO_HEADER result = new ERI_INFO_HEADER();
            result.dwVersion = ReadUInt32();
            result.fdwTransformation = ReadUInt32();
            result.dwArchitecture = ReadUInt32();
            result.fdwFormatType = ReadUInt32();
            result.nImageWidth = ReadInt32();
            result.nImageHeight = ReadInt32();
            result.dwBitsPerPixel = ReadUInt32();
            result.dwClippedPixel = ReadUInt32();
            result.dwSamplingFlags = ReadUInt32();
            result.dwQuantumizedBits[0] = ReadInt32();
            result.dwQuantumizedBits[1] = ReadInt32();
            result.dwAllottedBits[0] = ReadUInt32();
            result.dwAllottedBits[1] = ReadUInt32();
            result.dwBlockingDegree = ReadUInt32();
            result.dwLappedBlock = ReadUInt32();
            result.dwFrameTransform = ReadUInt32();
            result.dwFrameDegree = ReadUInt32();
            return result;
        }

        MIO_INFO_HEADER ReadMIOInfoHeader()
        {
            MIO_INFO_HEADER result = new MIO_INFO_HEADER();
            result.dwVersion = ReadUInt32();
            result.fdwTransformation = ReadUInt32();
            result.dwArchitecture = ReadUInt32();
            result.dwChannelCount = ReadUInt32();
            result.dwSamplesPerSec = ReadUInt32();
            result.dwBlocksetCount = ReadUInt32();
            result.dwSubbandDegree = ReadUInt32();
            result.dwAllSampleCount = ReadUInt32();
            result.dwLappedDegree = ReadUInt32();
            result.dwBitsPerSample = ReadUInt32();
            return result;
        }
        ERI_FILE_HEADER ReadERIFileHeader()
        {
            ERI_FILE_HEADER result = new ERI_FILE_HEADER();

            result.dwVersion = ReadUInt32();
            result.dwContainedFlag = ReadUInt32();
            result.dwKeyFrameCount = ReadUInt32();
            result.dwFrameCount = ReadUInt32();
            result.dwAllFrameTime = ReadUInt32();
            return result;
        }

        uint ReadUInt32()
        {
            byte[] buffer = new byte[4];
            if (Read(buffer, 4) != 4)
            {
                throw new Exception();
            }
            return BitConverter.ToUInt32(buffer, 0);
        }

        int ReadInt32()
        {
            byte[] buffer = new byte[4];
            if (Read(buffer, 4) != 4)
            {
                throw new Exception();
            }
            return BitConverter.ToInt32(buffer, 0);
        }
	    public SEQUENCE_DELTA[] GetSequenceTable( uint[] pdwLength ) 
        {
            throw new NotImplementedException();
        }

    }
}
