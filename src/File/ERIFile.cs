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
	    protected SEQUENCE_DELTA	m_pSequence ;
    
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
            throw new NotImplementedException();
        }
	    public SEQUENCE_DELTA[] GetSequenceTable( uint[] pdwLength ) 
        {
            throw new NotImplementedException();
        }

    }
}
