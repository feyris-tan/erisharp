using System;
using System.Drawing;
using ERIShArp.X;
using ERIShArp.Context;

namespace ERIShArp.Image
{
    public class ERISAEncoder
    {

	    protected ERI_INFO_HEADER		m_eihInfo ;				

	    protected uint				m_nBlockSize ;
	    protected uint				m_nBlockArea ;			
	    protected uint				m_nBlockSamples ;		
	    protected uint				m_nChannelCount ;		
	    protected uint				m_nWidthBlocks ;		
	    protected uint				m_nHeightBlocks ;		

	    protected byte[]				m_ptrSrcBlock ;
	    protected int				m_nSrcLineBytes ;		
	    protected uint				m_nSrcPixelBytes ;		
	    protected uint				m_nSrcWidth ;			
	    protected uint				m_nSrcHeight ;
	    protected uint				m_fdwEncFlags ;

	    protected byte[]				m_ptrColumnBuf ;		
	    protected byte[]				m_ptrLineBuf ;
	    protected byte[]				m_ptrEncodeBuf ;
	    protected byte[]				m_ptrArrangeBuf ;
        /// <summary>
        /// 4 elements
        /// </summary>
	    protected int[]				m_pArrangeTable;	

	    protected uint				m_nBlocksetCount ;		
	    protected float[]			m_ptrVertBufLOT ;		
	    protected float[]			m_ptrHorzBufLOT ;
        /// <summary>
        /// 36 elements
        /// </summary>
	    protected float[]			m_ptrBlocksetBuf;
	    /// <summary>
	    /// 16 elements
	    /// </summary>
	    protected float[]			m_ptrMatrixBuf;	
        /// <summary>
        /// 2 elements
        /// </summary>
	    protected float[]			m_pQuantumizeScale;	
	    protected byte[]				m_pQuantumizeTable ;

	    protected int				m_nMovingVector ;		
	    protected byte[]				m_pMoveVecFlags ;
	    protected byte[]				m_pMovingVector ;
	    protected int					m_fPredFrameType ;
	    protected int				m_nIntraBlockCount ;	
	    protected float				m_rDiffDeflectBlock ;	
	    protected float				m_rMaxDeflectBlock ;	

	    protected byte[]				m_ptrCoefficient ;
	    protected byte[]				m_ptrImageDst ;
	    protected float[]			m_ptrSignalBuf ;

	    protected ERINA_HUFFMAN_TREE[]	m_pHuffmanTree;
	    protected ERISA_PROB_MODEL[]		m_pProbERISA ;

	    /// <summary>
	    /// 0x10 elements
	    /// </summary>
        protected static PTR_PROCEDURE	m_pfnColorOperation;

		    public const ushort efTopDown		= 0x0001;
		    public const ushort efDifferential	= 0x0002;
		    public const ushort efNoMoveVector	= 0x0004;
		    public const ushort efBestCmpr		= 0x0000;
		    public const ushort efHighCmpr		= 0x0010;
		    public const ushort efNormalCmpr	= 0x0020;
		    public const ushort efLowCmpr		= 0x0030;
		    public const ushort efCmprModeMask	= 0x0030;

	    public enum	PresetParameter
	    {
		    ppClearQuality,
		    ppHighQuality,
		    ppAboveQuality,
		    ppStandardQuality,
		    ppBelowQuality,
		    ppLowQuality,
		    ppLowestQuality,
		    ppMax
	    }
	
		public const ushort pfUseLoopFilter		= 0x0001;
		public const ushort pfLocalLoopFilter	= 0x0002;

	    public struct	PARAMETER
	    {
		    uint			m_dwFlags ;
		    float			m_rYScaleDC ;
		    float			m_rCScaleDC ;
		    float			m_rYScaleLow ;
		    float			m_rCScaleLow ;
		    float			m_rYScaleHigh ;			
		    float			m_rCScaleHigh ;			
		    int				m_nYThreshold ;			
		    int				m_nCThreshold ;			
		    int				m_nYLPFThreshold ;		
		    int				m_nCLPFThreshold ;		
		    int				m_nAMDFThreshold ;		
		    float			m_rPFrameScale ;		
		    float			m_rBFrameScale ;		
		    uint			m_nMaxFrameSize ;		
		    uint			m_nMinFrameSize	;		

		    void LoadPresetParam ( PresetParameter ppIndex, ERI_INFO_HEADER infhdr )
            {
                throw new NotImplementedException();
            }
	    }


	protected PARAMETER			m_prmCmprOpt ;


	public ERISAEncoder()
    {
            throw new NotImplementedException();
    }

	~ERISAEncoder()
    {
            throw new NotImplementedException();
    }

	public virtual void Initialize(ERI_INFO_HEADER  infhdr )
    {
        throw new NotImplementedException();
    }
	
	public virtual void Delete()
    {
        throw new NotImplementedException();
    }    
	
	public virtual void EncodeImage (EGL_IMAGE_INFO srcimginf, ERISAEncodeContext context, uint fdwFlags = efNormalCmpr)
	{
        throw new NotImplementedException();
    }
	
    public void SetCompressionParameter(PARAMETER  prmCmprOpt )
    {
        throw new NotImplementedException();
    }

	protected virtual void OnEncodedBlock( int line, int column )
    {
        throw new NotImplementedException();
    }

	public void ProcessMovingVector(EGL_IMAGE_INFO nextimg, EGL_IMAGE_INFO previmg, int nAbsMaxDiff, EGL_IMAGE_INFO ppredimg2 = null )
    {
        throw new NotImplementedException();
    }
	

	public void ClearMovingVector()
    {
        throw new NotImplementedException();
    }

	protected int PredictMovingVector( EGL_IMAGE_INFO nextimg, EGL_IMAGE_INFO previmg, int xBlock, int yBlock, Point[] ptMoveVec, int nAbsMaxDiff, double rDeflectBlock, EGL_IMAGE_INFO ppredimg = null )
    {
        throw new NotImplementedException();
    }
	
	protected void SearchMovingVector( EGL_IMAGE_INFO nextblock, EGL_IMAGE_INFO predblock, EGL_IMAGE_INFO nextimg, EGL_IMAGE_INFO predimg, int xBlock, int yBlock, Point ptMoveVec )
    {
        throw new NotImplementedException();
    }
	
	protected static int CalcSumDeflectBlock(EGL_IMAGE_INFO imgblock )
    {
        throw new NotImplementedException();
    }

	
	protected static int CalcSumSqrDifferenceBlock( EGL_IMAGE_INFO dstimg, EGL_IMAGE_INFO srcimg )
    {
        throw new NotImplementedException();
    }

	
	protected static void MakeBlockValueHalf( EGL_IMAGE_INFO imgblock )
    {
        throw new NotImplementedException();
    }

	protected void EncodeLosslessImage(EGL_IMAGE_INFO imginf, ERISAEncodeContext context, uint fdwFlags )
    {
        throw new NotImplementedException();
    }

	protected void InitializeSamplingTable()        
    {
        throw new NotImplementedException();
    }

	
	protected void DifferentialOperation( int nAllBlockLines, byte[] pNextLineBuf )
    {
        throw new NotImplementedException();
    }

	protected uint DecideOperationCode( uint fdwFlags, int nAllBlockLines, byte[] pNextLineBuf)
    {
        throw new NotImplementedException();
    }

	protected void ColorOperation0000()
    {
        throw new NotImplementedException();
    }

	protected void ColorOperation0001()
    {
        throw new NotImplementedException();
    }

	protected void ColorOperation0010()
    {
        throw new NotImplementedException();
    }

	protected void ColorOperation0011()
    {
        throw new NotImplementedException();
    }

	protected void ColorOperation0100()
    {
        throw new NotImplementedException();
    }

	protected void ColorOperation0101()
    {
        throw new NotImplementedException();
    }

	protected void ColorOperation0110()
    {
        throw new NotImplementedException();
    }

	protected void ColorOperation0111()
    {
        throw new NotImplementedException();
    }

	protected void ColorOperation1000()
    {
        throw new NotImplementedException();
    }

	protected void ColorOperation1001()
    {
        throw new NotImplementedException();
    }

	protected void ColorOperation1010()
    {
        throw new NotImplementedException();
    }

	protected void ColorOperation1011()
    {
        throw new NotImplementedException();
    }

	protected void ColorOperation1100()
    {
        throw new NotImplementedException();
    }

	protected void ColorOperation1101()
    {
        throw new NotImplementedException();
    }

	protected void ColorOperation1110()
    {
        throw new NotImplementedException();
    }

	protected void ColorOperation1111()
    {
        throw new NotImplementedException();
    }

	protected void SamplingGray8()
    {
        throw new NotImplementedException();
    }

	protected void SamplingRGB16()
    {
        throw new NotImplementedException();
    }

	protected void SamplingRGB24()
    {
        throw new NotImplementedException();
    }

	protected void SamplingRGBA32()
    {
        throw new NotImplementedException();
    }

	protected virtual PTR_PROCEDURE GetLLSamplingFunc( uint fdwFormatType, uint dwBitsPerPixel, uint fdwFlags )
    {
        throw new NotImplementedException();
    }


	protected void EncodeLossyImage( EGL_IMAGE_INFO imginf, ERISAEncodeContext context, uint fdwFlags )
    {
        throw new NotImplementedException();
    }

	protected void CalcImageSizeInBlocks( uint fdwTransformation )
    {
        throw new NotImplementedException();
    }

	protected void InitializeZigZagTable()
    {
        throw new NotImplementedException();
    }

	protected void InitializeQuantumizeTable( double r = 1.0 )
    {
        throw new NotImplementedException();
    }


	protected void SamplingMacroBlock( int xBlock, int yBlock,int nLeftWidth, int nLeftHeight,uint dwBlockStepAddr, byte[] ptrSrcLineAddr,PTR_PROCEDURE pfnSamplingFunc)
    {
        throw new NotImplementedException();
    }

	protected void FillBlockOddArea( uint dwFlags )
    {
        throw new NotImplementedException();
    }

	protected void BlockScaling444()
    {
        throw new NotImplementedException();
    }

	protected void BlockScaling411()
    {
        throw new NotImplementedException();
    }

	protected void MatrixDCT8x8()
    {
        throw new NotImplementedException();
    }

	protected void MatrixLOT8x8( float[] ptrVertBufLOT )
    {
        throw new NotImplementedException();
    }

	protected void ArrangeAndQuantumize( byte[] ptrCoefficient, uint fdwFlags )
    {
        throw new NotImplementedException();
    }

	protected virtual PTR_PROCEDURE GetLSSamplingFunc( uint fdwFormatType, uint dwBitsPerPixel, uint fdwFlags )
    {
        throw new NotImplementedException();
    }


    }
}
