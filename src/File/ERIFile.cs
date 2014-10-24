using System;
using System.Drawing;
using ERIShArp.X;
using System.IO;

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
	    public static char[]	m_pwszTagName;

	    public class	ETagObject
	    {
		    public string		m_tag ;
		    public string		m_contents ;
		    public ETagObject()
            {
                throw new NotImplementedException();
            }
		    ~ETagObject()
            {
                throw new NotImplementedException();
            }
	    }

	    public class	ETagInfo
	    {
		    public ETagObject[]	m_lstTags ;
		    public ETagInfo()
            {
                throw new NotImplementedException();
            }
		    ~ETagInfo()
            {
                throw new NotImplementedException();
            }
		    public void CreateTagInfo( char[] pwszDesc )
            {
                throw new NotImplementedException();
            }
		    public void FormatDescription( string wstrDesc )
            {
                throw new NotImplementedException();
            }
		    public void AddTag( TagIndex tagIndex, char[] pwszContents )
            {
                throw new NotImplementedException();
            }
		    public void DeleteContents()
            {
                throw new NotImplementedException();
            }
		    public char[] GetTagContents( char[] pwszTag ) 
            {
                throw new NotImplementedException();
            }
		    public char[] GetTagContents( TagIndex tagIndex )
            {
                throw new NotImplementedException();
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

	    public struct	SEQUENCE_DELTA
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
            throw new NotImplementedException();
        }
	    ~ERIFile()
        {
            throw new NotImplementedException();
        }

	    public enum	OpenType
	    {
		    otOpenRoot,			
		    otReadHeader,		
		    otOpenStream,		
		    otOpenImageData		
	    } ;
	    public void Open( Stream pFile, OpenType type = OpenType.otOpenImageData )
        {
            throw new NotImplementedException();
        }
	    public SEQUENCE_DELTA[] GetSequenceTable( uint[] pdwLength ) 
        {
            throw new NotImplementedException();
        }

    }
}
