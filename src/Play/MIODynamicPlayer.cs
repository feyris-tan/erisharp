using System;
using ERIShArp.X;
using ERIShArp.File;
using ERIShArp.Sound;
using ERIShArp.Context;
using System.Threading;
using System.IO;

namespace ERIShArp.Play
{
    public class MIODynamicPlayer
    {
        
	    public MIODynamicPlayer()
        {
        }
	
        ~MIODynamicPlayer()
        {
        }

        /// <summary>
        /// actually derived from EMemoryFile - put we'll probably won't need to implement that.
        /// </summary>
	    protected class	EPreloadBuffer
	    {
		    public byte[]			m_ptrBuffer ;
		    public uint			    m_nKeySample ;
		    public MIO_DATA_HEADER	m_miodh ;
	
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
		    public uint	m_nKeySample ;
		    public uint	m_dwRecOffset ;

            /// <summary>
            /// actually empty in the *.h file
            /// </summary>
    		public EKeyPoint() 
            { 
            }

		    public EKeyPoint(EKeyPoint key )
            {
                this.m_dwRecOffset = key.m_dwRecOffset;
                this.m_nKeySample = key.m_nKeySample;
            }
	    }

	    protected enum	ThreadMessage
	    {
		    tmSeekSound	= 0x0400,
		    tmQuit
	    }

    	protected ERIFile					m_erif ;
	    protected ERISADecodeContext[]	m_pmioc ;
	    protected MIODecoder[]			m_pmiod ;
	    protected Thread					m_hThread ;
	    protected uint					m_idThread ;
	    protected EPreloadBuffer[]	m_queueSound;
	    protected IntPtr					m_hQueueFull ;
        protected IntPtr m_hQueueNotEmpty;
        protected IntPtr m_hQueueSpace;	
	    protected int			m_nCurrentSample ;
	    protected EKeyPoint[]	m_listKeySample ;
        protected IntPtr m_cs;

	    public void Open( Stream pFile, uint nPreloadSize = 0 )
        {
            throw new NotImplementedException();
        }
	    public void Close()
        {
            throw new NotImplementedException();
        }

	    public virtual byte[] GetWaveBufferFrom( uint nSample, ref uint dwBytes, ref uint dwOffsetBytes )
        {
            throw new NotImplementedException();
        }
    	public virtual bool IsNextDataRewound()
        {
            throw new NotImplementedException();
        }
	    public virtual byte[] GetNextWaveBuffer( ref uint dwBytes )
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
	    public virtual MIODecoder CreateMIODecoder()
        {
            throw new NotImplementedException();
        }

	    public ERIFile GetERIFile()
        {
            throw new NotImplementedException();
        }
	    public uint GetChannelCount()
        {
            throw new NotImplementedException();
        }
	    public uint GetFrequency()
        {
            throw new NotImplementedException();
        }
	    public uint GetBitsPerSample()
        {
            throw new NotImplementedException();
        }
	    public uint GetTotalSampleCount()
        {
            throw new NotImplementedException();
        }

    	protected  EPreloadBuffer GetPreloadBuffer()
        {
            throw new NotImplementedException();
        }
	    protected void AddPreloadBuffer(EPreloadBuffer pBuffer)
        {
            throw new NotImplementedException();
        }

	    protected static uint ThreadProc( byte[] parameter )
        {
            throw new NotImplementedException();
        }
	    protected uint LoadingThreadProc()
        {
            throw new NotImplementedException();
        }
	    protected EPreloadBuffer LoadSoundStream( ref uint nCurrentSample )
        {
            throw new NotImplementedException();
        }
	    protected void AddKeySample(EKeyPoint key )
        {
            throw new NotImplementedException();
        }
	    protected EKeyPoint SearchKeySample( uint nKeySample )
        {
            throw new NotImplementedException();
        }
	    protected void SeekKeySample( uint nSample, ref uint nCurrentSample )
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
