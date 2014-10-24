using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using ERIShArp.Image;
using ERIShArp.Context;
using ERIShArp.Sound;

namespace ERIShArp.Play
{
    public class ERIAnimation
    {
	    public ERIAnimation()
        {
            throw new NotImplementedException();
        }
	    ~ERIAnimation()
        {
            throw new NotImplementedException();
        }

	    protected class	EPreloadBuffer
	    {
		    public byte[]	m_ptrBuffer ;
		    public uint	m_iFrameIndex ;
		    public ulong	m_ui64RecType ;
		    public EPreloadBuffer( uint dwLength )
            {
                throw new NotImplementedException();
            }
		    ~EPreloadBuffer()
            {
                throw new NotImplementedException();
            }
	    }

	    protected class	EKeyPoint
	    {
            public uint	m_iKeyFrame ;
		    public uint	m_dwSubSample ;
		    public uint	m_dwRecOffset ;
            /// <summary>
            /// actually empty in the *.h file
            /// </summary>
		    public EKeyPoint() 
            { 
            }
		    public EKeyPoint( EKeyPoint key )
            {
                this.m_dwRecOffset = key.m_dwRecOffset;
                this.m_dwSubSample = key.m_dwSubSample;
                this.m_iKeyFrame = key.m_iKeyFrame;
            }
	    }

	    protected enum	ThreadMessage
	    {
		    tmSeekFrame	= 0x0400,
		    tmSeekSound,
		    tmQuit
	    }
	    protected enum	FrameType
	    {
    		ftOtherData		= -1,
	    	ftIntraFrame,
		    ftPredictionalFrame,
		    ftBidirectionalFrame		
	    }

	    protected bool					m_fTopDown ;
	    protected bool					m_fWaveOutput ;
	    protected bool					m_fWaveStreaming ;
	    protected Stream					m_erif ;
	    protected uint					m_fdwDecFlags ;
	    protected ERISADecodeContext 	m_peric ;
	    protected ERISADecoder 			m_perid ;
	    protected ERISADecodeContext 	m_pmioc ;
	    protected MIODecoder 			m_pmiod ;
	    protected uint			m_iCurrentFrame ;
	    protected uint			m_iDstBufIndex ;	
	    protected uint			m_nCacheBFrames ;	
        /// <summary>
        /// 5 entries
        /// </summary>
	    protected Bitmap[]		m_pDstImage;
        /// <summary>
        /// 5 entries
        /// </summary>
	    protected uint			m_iDstFrameIndex;
	    protected Thread					m_hThread ;
	    protected uint					m_idThread ;
	    protected uint			m_iPreloadFrame ;
	    protected uint		m_nPreloadWaveSamples ;
	    protected uint			m_nPreloadLimit ;
	    protected EPreloadBuffer[]	m_queueImage ;
	    protected IntPtr					m_hQueueNotEmpty ;
        protected IntPtr m_hQueueSpace;
	    protected List<EKeyPoint>			m_listKeyFrame ;
	    protected List<EKeyPoint>			m_listKeyWave ;
	    protected IntPtr		m_cs ;

        protected virtual Bitmap CreateImageBuffer(uint format, int width, int height, uint bpp)
        {
            throw new NotImplementedException();
        }
	    protected virtual void DeleteImageBuffer( Bitmap peii )
        {
            throw new NotImplementedException();
        }
	    protected virtual ERISADecoder CreateERIDecoder()
        {
            throw new NotImplementedException();
        }
	    protected virtual MIODecoder CreateMIODecoder()
        {
            throw new NotImplementedException();
        }
	    protected virtual bool RequestWaveOut( uint channels, uint frequency, uint bps )
        {
            throw new NotImplementedException();
        }
	    protected virtual void CloseWaveOut()
        {
            throw new NotImplementedException();
        }
        protected virtual void PushWaveBuffer(byte[] ptrWaveBuf, uint dwBytes)
        {
            throw new NotImplementedException();
        }

        public virtual byte[] AllocateWaveBuffer(uint dwBytes)
        {
            throw new NotImplementedException();
        }
        public virtual void DeleteWaveBuffer(byte[] ptrWaveBuf)
        {
            throw new NotImplementedException();
        }
	    public virtual void BeginWaveStreaming()
        {
            throw new NotImplementedException();
        }
	    public virtual void EndWaveStreaming()
        {
            throw new NotImplementedException();
        }

	    public void Open( Stream pFile, uint nPreloadSize = 0, uint fdwFlags = 0 )
        {
            throw new NotImplementedException();
        }
	    public void Close()
        {
            throw new NotImplementedException();
        }

	    public void SeekToBegin()
        {
            throw new NotImplementedException();
        }
        public void SeekToNextFrame(int nSkipFrame = 0)
        {
            throw new NotImplementedException();
        }
        public void SeekToFrame(uint iFrameIndex)
        {
            throw new NotImplementedException();
        }
        public bool IsKeyFrame(uint iFrameIndex)
        {
            throw new NotImplementedException();
        }
        public uint GetBestSkipFrames(uint nCurrentTime)
        {
            throw new NotImplementedException();
        }

        protected void DecodeFrame(EPreloadBuffer pFrame, uint fdwFlags = 0)
        {
            throw new NotImplementedException();
        }
        protected void ApplyPaletteTable(EPreloadBuffer pBuffer)
        {
            throw new NotImplementedException();
        }
	    protected EPreloadBuffer GetPreloadBuffer()
        {
            throw new NotImplementedException();
        }
        protected void AddPreloadBuffer(EPreloadBuffer pBuffer)
        {
            throw new NotImplementedException();
        }
        protected int GetFrameBufferType(EPreloadBuffer pBuffer)
        {
            throw new NotImplementedException();
        }

	    public Stream GetERIFile()
        {
            throw new NotImplementedException();
        }
	    public uint CurrentIndex()
        {
            throw new NotImplementedException();
        }
	    public Bitmap GetImageInfo()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 4 entries
        /// </summary>
        /// <returns></returns>
	    public byte[] GetPaletteEntries()
        {
            throw new NotImplementedException();
        }
	    public uint GetKeyFrameCount()
        {
            throw new NotImplementedException();
        }
	    public uint GetAllFrameCount( )
        {
            throw new NotImplementedException();
        }
	    public uint GetTotalTime()
        {
            throw new NotImplementedException();
        }
	    public uint FrameIndexToTime( uint iFrameIndex )
        {
            throw new NotImplementedException();
        }
	    public uint TimeToFrameIndex( uint nMilliSec )
        {
            throw new NotImplementedException();
        }

	    protected static uint ThreadProc( IntPtr parameter )
        {
            throw new NotImplementedException();
        }
	    protected uint LoadingThreadProc()
        {
            throw new NotImplementedException();
        }
	    protected EPreloadBuffer LoadMovieStream( uint iCurrentFrame )
        {
            throw new NotImplementedException();
        }
	    protected void AddKeyPoint( List<EKeyPoint> list, EKeyPoint key )
        {
            throw new NotImplementedException();
        }
	    protected EKeyPoint SearchKeyPoint( List<EKeyPoint> list, uint iKeyFrame )
        {
            throw new NotImplementedException();
        }
	    protected void SeekKeyPoint( List<EKeyPoint> list, uint iFrame, ref uint iCurtrentFrame )
        {
            throw new NotImplementedException();
        }
	    protected void SeekKeyWave(List<EKeyPoint> list, uint iFrame )
        {
            throw new NotImplementedException();
        }

	    public void Lock()
        {
            throw new NotImplementedException();
        }
	    public void Unlock()
        {
            throw new NotImplementedException();
        }


    }
}
