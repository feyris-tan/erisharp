using System;
using ERIShArp.Image;
using ERIShArp.Context;
using System.IO;
using ERIShArp.File;
using System.Drawing;
using ERIShArp.X;
using ERIShArp.Image;
using ERIShArp.Sound;

namespace ERIShArp.Play
{
    public class ERIAnimationWriter : ESLAnimationOutputInterface
    {
	    public ERIAnimationWriter()
        {
            throw new NotImplementedException();
        }
	     ~ERIAnimationWriter()
        {
            throw new NotImplementedException();
        }

	    public enum	FileIdentity
	    {
		    fidImage,
		    fidSound,
		    fidMovie
	    }

	    protected class	EEncodeContext	: ERISAEncodeContext
	    {
            /// <summary>
            /// might have to implement EStreamBuffer
            /// </summary>
		    public Stream m_buf ;
		    public EEncodeContext()
                : base(0x10000)
            {           
            }
            ~EEncodeContext()
            {
                throw new NotImplementedException();
            }
		    public void Delete() 
            { 
                m_buf.Dispose( ); 
            }
		    public virtual uint WriteNextData( byte[] ptrBuffer, uint nBytes )
            {
                throw new NotImplementedException();
            }
	    }

	    protected enum	ThreadMessage
	    {
		    tmEncodeImage	= 0x0400,
		    tmQuit
	    }

	    protected enum	WriterStatus
	    {
    		wsNotOpened,
	    	wsOpened,
		    wsWritingHeader,
		    wsWritingStream
	    }
	    protected WriterStatus			m_wsStatus ;
	    protected EMCFile					m_eriwf ;
	    protected uint					m_dwHeaderBytes ;	
	    protected ERI_FILE_HEADER			m_efh ;				
	    protected ERI_INFO_HEADER			m_prvwih ;			
	    protected ERI_INFO_HEADER			m_eih ;				
	    protected MIO_INFO_HEADER			m_mih ;				
	    protected WAVEFORMATEX			m_wfx ;
	    protected bool					m_fWithSeqTable ;	
	    protected uint					m_dwKeyFrame ;		
	    protected uint					m_dwBidirectKey ;	
	    protected uint					m_dwKeyWave ;		
	    protected uint					m_dwFrameCount ;	
	    protected uint					m_dwWaveCount ;		
	    protected uint					m_dwDiffFrames ;	
	    protected uint					m_dwMioHeaderPos ;		
	    protected uint					m_dwOutputWaveSamples ;
	    protected EEncodeContext			m_eric1 ;
	    protected EEncodeContext			m_eric2 ;
	    protected ERISAEncoder 			m_perie1 ;
	    protected ERISAEncoder 			m_perie2 ;
	    protected ERISAEncodeContext 	m_pmioc ;
	    protected MIOEncoder			m_pmioe ;
	    protected EGL_IMAGE_INFO 		m_peiiLast ;
	    protected EGL_IMAGE_INFO 		m_peiiNext ;
	    protected EGL_IMAGE_INFO 		m_peiiBuf ;
	    protected uint[]		m_lstEncFlags ;
	    protected EGL_IMAGE_INFO[]       m_lstFrameBuf ;	    
	    protected bool					m_fKeyWaveBlock ;
	    protected Stream			m_bufWaveBuffer ;
	    protected uint					m_dwWaveBufSamples ;
	    protected bool					m_fDualEncoding ;
	    protected bool					m_fCompressSuccessed ;
        protected IntPtr					m_hCompressed ;
	    protected IntPtr					m_hThread ;
	    protected uint					m_idThread ;
	    protected ERISAEncoder.PARAMETER	m_eriep_i ;
	    protected ERISAEncoder.PARAMETER	m_eriep_p ;
	    protected ERISAEncoder.PARAMETER	m_eriep_b ;
	    protected MIOEncoder.PARAMETER	m_mioep ;

	    public void Open( Stream pFile, FileIdentity fidType )
        {
            throw new NotImplementedException();
        }
	    public void Close()
        {
            throw new NotImplementedException();
        }

	    public void BeginFileHeader( uint dwKeyFrame, uint dwKeyWave, uint dwBidirectKey = 3 )
        {
            throw new NotImplementedException();
        }
	    public void WritePreviewInfo( ERI_INFO_HEADER eih )
        {
            throw new NotImplementedException();
        }
	    public void WriteEriInfoHeader( ERI_INFO_HEADER eih )
        {
            throw new NotImplementedException();
        }
	    public void WriteMioInfoHeader( MIO_INFO_HEADER mih )
        {
            throw new NotImplementedException();
        }
	    public void WriteCopyright( byte[] ptrCopyright, uint dwBytes )
        {
            throw new NotImplementedException();
        }
	    public void WriteDescription( byte[] ptrDescription, uint dwBytes )
        {
            throw new NotImplementedException();
        }
	    public void WriteSequenceTable( ERIFile.SEQUENCE_DELTA  pSequence, uint dwLength )
        {
            throw new NotImplementedException();
        }
	    public void EndFileHeader()
        {
            throw new NotImplementedException();
        }

	    public void SetImageCompressionParameter( ERISAEncoder.PARAMETER eriep )
        {
            throw new NotImplementedException();
        }
        
	    public void SetSoundCompressionParameter( MIOEncoder.PARAMETER mioep )
        {
            throw new NotImplementedException();
        }

	    public virtual WAVEFORMATEX GetWaveFormat()
        {
            throw new NotImplementedException();
        }
	    public virtual void BeginStream()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paltbl">4 entries</param>
        /// <param name="nLength"></param>
	    public virtual void WritePaletteTable( byte[] paltbl, uint nLength )
        {
            throw new NotImplementedException();
        }
	    public virtual void WritePreviewData( EGL_IMAGE_INFO eii, uint fdwFlags )
        {
            throw new NotImplementedException();
        }
	    public virtual void WriteWaveData( byte[] ptrWaveBuf, uint dwSampleCount )
        {
            throw new NotImplementedException();
        }
	    public void WriteImageData( EGL_IMAGE_INFO eii, uint fdwFlags )
        {
            throw new NotImplementedException();
        }
	    public virtual void WriteImageData( EGL_IMAGE_INFO infImage )
        {
            throw new NotImplementedException();
        }
	    public virtual void EndStream( uint dwTotalTime )
        {
            throw new NotImplementedException();
        }

	    protected void WriteBirectionalFrames()
        {
            throw new NotImplementedException();
        }
	    protected void WriteWaveBuffer()
        {
            throw new NotImplementedException();
        }
	    protected EGL_IMAGE_INFO CreateImageBuffer(ERI_INFO_HEADER eih )
        {
            throw new NotImplementedException();
        }
	    protected void DeleteImageBuffer( EGL_IMAGE_INFO peii )
        {
            throw new NotImplementedException();
        }
	    protected virtual ERISAEncoder CreateERIEncoder()
        {
            throw new NotImplementedException();
        }
	    protected virtual MIOEncoder CreateMIOEncoder()
        {
            throw new NotImplementedException();
        }

	    public uint GetWrittenFrameCount()
        {
            throw new NotImplementedException();
        }
        public void EnableDualEncoding( bool fDualEncoding )
        {
            throw new NotImplementedException();
        }

	    public static uint ThreadProc( IntPtr parameter )
        {
            throw new NotImplementedException();
        }
	    public uint EncodingThreadProc()
        {
            throw new NotImplementedException();
        }
    }
}
