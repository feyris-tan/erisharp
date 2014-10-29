using System;
using ERIShArp.X;
using ERIShArp.Context;
using System.Drawing;

namespace ERIShArp.Image
{
    public class ERISADecoder
    {
        protected ERI_INFO_HEADER m_eihInfo;

        protected uint m_nBlockSize;
        protected uint m_nBlockArea;
        protected uint m_nBlockSamples;
        protected uint m_nChannelCount;
        protected uint m_nWidthBlocks;
        protected uint m_nHeightBlocks;

        protected byte[] m_ptrDstBlock;
        protected int m_nDstLineBytes;
        protected uint m_nDstPixelBytes;
        protected uint m_nDstWidth;			
        protected uint m_nDstHeight;
        protected uint m_fdwDecFlags;

        protected int m_fEnhancedMode;		
        protected byte[] m_ptrOperations;		
        protected byte[] m_ptrColumnBuf;
        protected byte[] m_ptrLineBuf;
        protected byte[] m_ptrDecodeBuf;
        protected byte[] m_ptrArrangeBuf;
        /// <summary>
        /// 4 elements
        /// </summary>
        protected int[] m_pArrangeTable;	

        protected uint m_nBlocksetCount;
        protected float[] m_ptrVertBufLOT;
        protected float[] m_ptrHorzBufLOT;
        //16 elements
        protected float[] m_ptrBlocksetBuf;	
        protected float[] m_ptrMatrixBuf;
        protected float[] m_ptrIQParamBuf;
        protected byte[] m_ptrIQParamTable;

        protected byte[] m_ptrBlockLineBuf;
        protected byte[] m_ptrNextBlockBuf;
        protected byte[] m_ptrImageBuf;
        protected byte[] m_ptrYUVImage;
        protected int m_nYUVLineBytes;		
        protected uint m_nYUVPixelBytes;

        protected byte[] m_ptrMovingVector;
        protected byte[] m_ptrMoveVecFlags;
        protected byte[] m_ptrMovePrevBlocks;	
        protected byte[] m_ptrNextPrevBlocks;
        protected Bitmap m_pPrevImageRef;
        protected int m_dwPrevLineBytes;		
        protected Bitmap m_pNextImageRef;
        protected int m_dwNextLineBytes;		
        protected Bitmap m_pFilterImageBuf;

        protected ERINA_HUFFMAN_TREE m_pHuffmanTree;
        protected ERISA_PROB_MODEL m_pProbERISA;

        /// <summary>
        /// 0x10 entries;
        /// </summary>
        protected static PTR_PROCEDURE[] m_pfnColorOperation;

        public ERISADecoder()
        {
            m_ptrDstBlock = null;
            m_ptrOperations = null;
            m_ptrColumnBuf = null;
            m_ptrLineBuf = null;
            m_ptrDecodeBuf = null;
            m_ptrArrangeBuf = null;
            m_pArrangeTable = new int[4];
            m_ptrVertBufLOT = null;
            m_ptrHorzBufLOT = null;
            m_ptrBlocksetBuf = new float[16];
            m_ptrMatrixBuf = null;
            m_ptrIQParamBuf = null;
            m_ptrIQParamTable = null;
            m_ptrBlockLineBuf = null;
            m_ptrImageBuf = null;
            m_ptrYUVImage = null;
            m_ptrMovingVector = null;
            m_ptrMoveVecFlags = null;
            m_ptrMovePrevBlocks = null;
            m_pPrevImageRef = null;
            m_pNextImageRef = null;
            m_pFilterImageBuf = null;
            m_pHuffmanTree = null;
            m_pProbERISA = null;
        }

        ~ERISADecoder()
        {
            throw new NotImplementedException();
        }

        public const uint dfTopDown = 0x0001;
		public const uint dfDifferential	= 0x0002;
		public const uint dfQuickDecode	= 0x0100;
		public const uint dfQualityDecode	= 0x0200;
		public const uint dfNoLoopFilter	= 0x0400;
		public const uint dfUseLoopFilter	= 0x0800;
		public const uint dfPreviewDecode	= 0x1000;

        public virtual void Initalize(ERI_INFO_HEADER infhdr)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete()
        {
            throw new NotImplementedException();
        }

        public virtual void DecodeImage(Bitmap dstimginf, ERISADecodeContext context,uint fdwFlags = dfTopDown)
        {
            throw new NotImplementedException();
        }

        public void SetRefPreviousFrame(Bitmap pPrevFrame, Bitmap pNextFrame = null)
        {
            throw new NotImplementedException();
        }

        public Bitmap GetFilteredImageBuffer()
        {
            return m_pFilterImageBuf;
        }

        public void SetFilteredImageBuffer(Bitmap pImageBuf)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnDecodedBlock(int line, int column, Rectangle rect)
        {
            throw new NotImplementedException();
        }
    
        protected void DecodeLosslessImage(Bitmap imginf, ERISADecodeContext context, uint fdwFlags)
        {
            throw new NotImplementedException();
        }

        protected void InitalizeArrangeTable()
        {
            throw new NotImplementedException();
        }

        protected void PerformOperation(uint dwOpCode, uint nAllBlockLines, byte[] pNextLineBuf)
        {
            throw new NotImplementedException();
        }

        protected void ColorOperation0000()
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

        protected void RestoreGray8()
        {
            throw new NotImplementedException();
        }

        protected void RestoreRGB16()
        {
            throw new NotImplementedException();
        }

        protected void RestoreRGB24()
        {
            throw new NotImplementedException();
        }

        protected void RestoreRGBA32()
        {
            throw new NotImplementedException();
        }

        protected virtual PTR_PROCEDURE GetLLRestoreFunc(uint fdwFormatType, uint dwBitsPerPixel, uint fdwFlags)
        {
            throw new NotImplementedException();
        }

        protected void DecodeLossyImage(Bitmap imginf, ERISADecodeContext context, uint fdwFlags)
        {
            throw new NotImplementedException();
        }    

        protected void CalcImageSizeInBlocks(uint fdwTransformation)
        {
            throw new NotImplementedException();
        }

        protected void InitalizeZigZagTable()
        {
            throw new NotImplementedException();
        }

        protected void SetupMovingVector()
        {
            throw new NotImplementedException();
        }

        protected void ArrangeAndIQuantumize(byte[] ptrSrcData, byte[] ptrCoefficient)
        {
            throw new NotImplementedException();
        }

        protected void MatrixIDCT8x8(float[] a)
        {
            throw new NotImplementedException();
        }

        protected void MatrixILOT8x8( float[] ptrVertBufLOT )
        {
            throw new NotImplementedException();
        }

	    protected void BlockScaling444( int x, int y, uint fdwFlags )
        {
            throw new NotImplementedException();
        }

	    protected void BlockScaling411( int x, int y, uint fdwFlags )
        {
            throw new NotImplementedException();
        }
	    
        protected void StoreYUVImageChannel( int xBlock, int yBlock, int iChannel )
        {
            throw new NotImplementedException();
        }
	    
        protected void StoreYUVImageChannelX2( int xBlock, int yBlock, int iChannel )
        {
            throw new NotImplementedException();
        }
	    
        protected void ConvertImageYUVtoRGB( uint fdwFlags )
        {
            throw new NotImplementedException();
        }
	    
        protected void MoveImageWithVector()
        {
            throw new NotImplementedException();
        }
	    
        protected void LS_RestoreGray8()
        {
            throw new NotImplementedException();
        }
	    
        protected void LS_RestoreRGB24()
        {
            throw new NotImplementedException();
        }

	    protected void LS_RestoreRGBA32() 
        {
            throw new NotImplementedException();
        }

	    protected void LS_RestoreDeltaGray8()
        {
            throw new NotImplementedException();
        }
	    
        protected void LS_RestoreDeltaRGB24()
        {
            throw new NotImplementedException();
        }
	    
        protected void LS_RestoreDeltaRGBA32()
        {
            throw new NotImplementedException();
        }
	    
        protected virtual PTR_PROCEDURE GetLSRestoreFunc( uint fdwFormatType, uint dwBitsPerPixel, uint fdwFlags )
        {
            throw new NotImplementedException();
        }
    }

    public delegate void PTR_PROCEDURE();
}
