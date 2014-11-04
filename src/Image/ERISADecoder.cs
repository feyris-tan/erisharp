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
        protected IntPointer[] m_pArrangeTable;	

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
        protected EGL_IMAGE_INFO m_pPrevImageRef;
        protected int m_dwPrevLineBytes;		
        protected EGL_IMAGE_INFO m_pNextImageRef;
        protected int m_dwNextLineBytes;		
        protected EGL_IMAGE_INFO m_pFilterImageBuf;

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
            m_pArrangeTable = new IntPointer[4];
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
            Delete();
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
            Delete();
            m_eihInfo = infhdr;
            if (m_eihInfo.fdwTransformation == Constants.CVTYPE_LOSSLESS_ERI)
            {
                if ((m_eihInfo.dwArchitecture != Constants.ERI_RUNLENGTH_GAMMA) && (m_eihInfo.dwArchitecture != Constants.ERI_RUNLENGTH_HUFFMAN) && (m_eihInfo.dwArchitecture != Constants.ERISA_NEMESIS_CODE))
                {
                    throw new Exception();
                }
                switch ((m_eihInfo.fdwFormatType & Constants.ERI_TYPE_MASK))
                {
                    case Constants.ERI_RGB_IMAGE:
                        if (m_eihInfo.dwBitsPerPixel <= 8)
                            m_nChannelCount = 1;
                        else if ((m_eihInfo.fdwFormatType & Constants.ERI_WITH_ALPHA) == 0)
                            m_nChannelCount = 3;
                        else
                            m_nChannelCount = 4;
                        break;
                    case Constants.ERI_GRAY_IMAGE:
                        m_nChannelCount = 1;
                        break;
                    default:
                        throw new Exception();
                }
                if (m_eihInfo.dwBlockingDegree == 0)
                {
                    throw new Exception();
                }
                m_nBlockSize = (uint)((int)1 << (int)m_eihInfo.dwBlockingDegree);
                m_nBlockArea = (uint)((int)1 << (int)(m_eihInfo.dwBlockingDegree * 2));
                m_nBlockSamples = m_nBlockArea * m_nChannelCount;
                m_nWidthBlocks = (uint)((int)(m_eihInfo.nImageWidth + m_nBlockSize - 1) >> (int)(m_eihInfo.dwBlockingDegree));
                if (m_eihInfo.nImageHeight < 0)
                {
                    m_nHeightBlocks = (uint)-m_eihInfo.nImageHeight;
                }
                else
                {
                    m_nHeightBlocks = (uint)m_eihInfo.nImageHeight;
                }
                m_nHeightBlocks = (uint)((int)(m_nHeightBlocks + m_nBlockSize - 1) >> (int)m_eihInfo.dwBlockingDegree);

                m_ptrOperations = new byte[m_nWidthBlocks * m_nHeightBlocks];
                m_ptrColumnBuf = new byte[m_nBlockSize * m_nChannelCount];
                m_ptrLineBuf = new byte[m_nChannelCount * ((int)m_nWidthBlocks << (int)m_eihInfo.dwBlockingDegree)];
                m_ptrDecodeBuf = new byte[m_nBlockSamples];
                m_ptrArrangeBuf = new byte[m_nBlockSamples];

                if ((m_ptrOperations == null) || (m_ptrColumnBuf == null) || (m_ptrLineBuf == null) || (m_ptrDecodeBuf == null) || (m_ptrArrangeBuf == null))
                {
                    throw new Exception();
                }
                if (m_eihInfo.dwVersion == 0x00020100)
                {
                    m_fEnhancedMode = 0;
                    InitalizeArrangeTable();
                }
                else if (m_eihInfo.dwVersion == 0x00020200)
                {
                    m_fEnhancedMode = 2;
                    InitalizeArrangeTable();
                    if (m_eihInfo.dwArchitecture == Constants.ERI_RUNLENGTH_HUFFMAN)
                    {
                        m_pHuffmanTree = new ERINA_HUFFMAN_TREE();
                    }
                    else if (m_eihInfo.dwArchitecture == Constants.ERISA_NEMESIS_CODE)
                    {
                        m_pProbERISA = new ERISA_PROB_MODEL();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            else if ((m_eihInfo.fdwTransformation == Constants.CVTYPE_LOT_ERI) || (m_eihInfo.fdwTransformation == Constants.CVTYPE_LOT_ERI))
            {
                if ((m_eihInfo.dwArchitecture != Constants.ERI_RUNLENGTH_GAMMA) && (m_eihInfo.dwArchitecture != Constants.ERI_RUNLENGTH_HUFFMAN) && (m_eihInfo.dwArchitecture != Constants.ERISA_NEMESIS_CODE))
                {
                    throw new Exception();
                }
                switch ((m_eihInfo.fdwFormatType & Constants.ERI_TYPE_MASK))
                {
                    case Constants.ERI_RGB_IMAGE:
                        if (m_eihInfo.dwBitsPerPixel <= 8)
                            m_nChannelCount = 1;
                        else if ((m_eihInfo.fdwFormatType & Constants.ERI_WITH_ALPHA) == 0)
                            m_nChannelCount = 3;
                        else
                            m_nChannelCount = 4;
                        break;
                    case Constants.ERI_GRAY_IMAGE:
                        m_nChannelCount = 1;
                        break;
                    default:
                        throw new Exception();
                }
                if (m_eihInfo.dwBlockingDegree != 3)
                {
                    throw new Exception();
                }
                m_nBlockSize = (uint)(1 << (int)m_eihInfo.dwBlockingDegree);
                m_nBlockArea = (uint)(1 << (int)(m_eihInfo.dwBlockingDegree * 2));
                m_nBlockSamples = m_nBlockArea * m_nChannelCount;
                if (m_eihInfo.fdwTransformation == Constants.CVTYPE_LOT_ERI)
                {
                    m_nWidthBlocks = (uint)((m_eihInfo.nImageWidth + m_nBlockSize * 2 - 1) >> (int)(m_eihInfo.dwBlockingDegree + 1));
                    if (m_eihInfo.nImageHeight < 0)
                    {
                        m_nHeightBlocks = (uint)-m_eihInfo.nImageHeight;
                    }
                    else
                    {
                        m_nHeightBlocks = (uint)m_eihInfo.nImageHeight;
                    }
                    m_nHeightBlocks = (m_nHeightBlocks + m_nBlockSize * 2 - 1) >> (int)(m_eihInfo.dwBlockingDegree + 1);
                    m_nWidthBlocks += 1;
                    m_nHeightBlocks += 1;
                    if (m_eihInfo.dwSamplingFlags == Constants.ERISF_YUV_4_4_4)
                    {
                        m_nBlocksetCount = m_nChannelCount * 4;
                    }
                    else if (m_eihInfo.dwSamplingFlags == Constants.ERISF_YUV_4_1_1)
                    {
                        switch (m_nChannelCount)
                        {
                            case 1:
                                m_nBlocksetCount = 4;
                                break;
                            case 3:
                                m_nBlocksetCount = 6;
                                break;
                            case 4:
                                m_nBlocksetCount = 10;
                                break;
                            default:
                                throw new Exception();
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    m_nWidthBlocks = (uint)(((int)(m_eihInfo.nImageWidth + (m_nBlockSize * 2 - 1))) >> (int)(m_eihInfo.dwBlockingDegree + 1));
                    if (m_eihInfo.nImageHeight < 0)
                    {
                        m_nHeightBlocks = (uint)-m_eihInfo.nImageHeight;
                    }
                    else
                    {
                        m_nHeightBlocks = (uint)m_eihInfo.nImageHeight;
                    }
                    m_nHeightBlocks = (uint)((m_nHeightBlocks + (m_nBlockSize * 2 - 1))) >> (int)(m_eihInfo.dwBlockingDegree + 1);
                    //
                    if (m_eihInfo.dwSamplingFlags == Constants.ERISF_YUV_4_4_4)
                    {
                        m_nBlocksetCount = m_nChannelCount * 4;
                    }
                    else if (m_eihInfo.dwSamplingFlags == Constants.ERISF_YUV_4_1_1)
                    {
                        switch (m_nChannelCount)
                        {
                            case 1:
                                m_nBlocksetCount = 4;
                                break;
                            case 3:
                                m_nBlocksetCount = 6;
                                break;
                            case 4:
                                m_nBlocksetCount = 10;
                                break;
                            default:
                                throw new Exception();
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                m_ptrDecodeBuf = new byte[m_nBlockArea * 16 * 1];
                m_ptrVertBufLOT = new float[m_nBlockSamples * 2 * m_nWidthBlocks];
                m_ptrHorzBufLOT = new float[m_nBlockSamples * 2];
                m_ptrBlocksetBuf = new float[m_nBlockSamples * 2];
                m_ptrBlocksetBuf[0] = 1;
                m_ptrMatrixBuf = new float[m_nBlockArea * 16];
                m_ptrIQParamBuf = new float[m_nBlockArea * 2];
                m_ptrIQParamTable = new byte[m_nBlockArea * 2];
                uint dwTotalBlocks = m_nWidthBlocks * m_nHeightBlocks;
                m_ptrImageBuf = new byte[dwTotalBlocks * m_nBlockArea * m_nBlocksetCount];
                m_ptrMovingVector = new byte[dwTotalBlocks * 4 * 1];
                m_ptrMoveVecFlags = new byte[dwTotalBlocks * 1];
                m_ptrMovePrevBlocks = new byte[dwTotalBlocks * 4];
                m_pPrevImageRef = null;
                for (int i = 1; i < 16; i++)
                {
                    m_ptrBlocksetBuf[i] = m_ptrBlocksetBuf[0] + (m_nBlockArea * i);
                }
                m_nYUVPixelBytes = m_nChannelCount;
                if (m_nYUVPixelBytes == 3)
                {
                    m_nYUVPixelBytes = 4;
                }
                m_nYUVPixelBytes = (uint)(((m_nYUVPixelBytes * m_nWidthBlocks * m_nBlockSize * 2) + 0x0F) & (~0x0F));
                uint nYUVImageSIze = (uint)(m_nYUVLineBytes * m_nHeightBlocks * m_nBlockSize * 2);
                m_ptrBlockLineBuf = new byte[m_nYUVLineBytes * 16];
                m_ptrYUVImage = new byte[nYUVImageSIze];
                if ((m_ptrDecodeBuf == null) || (m_ptrVertBufLOT == null) || (m_ptrHorzBufLOT == null) || (m_ptrBlocksetBuf == null) || (m_ptrMatrixBuf == null) || (m_ptrIQParamBuf == null) || (m_ptrIQParamTable == null) || (m_ptrOperations == null) || (m_ptrImageBuf == null) || (m_ptrMovingVector == null) || (m_ptrMovePrevBlocks == null))
                {
                    throw new Exception();
                }
                InitalizeZigZagTable();
                m_pHuffmanTree = new ERINA_HUFFMAN_TREE();
                m_pProbERISA = new ERISA_PROB_MODEL();
            }
            else
            {
                throw new Exception();
            }
        }

        public virtual void Delete()
        {
            m_ptrOperations = null;
            m_ptrColumnBuf = null;
            m_ptrLineBuf = null;
            m_ptrDecodeBuf = null;
            m_ptrArrangeBuf = null;
            m_ptrVertBufLOT = null;
            m_ptrHorzBufLOT = null;
            m_ptrMatrixBuf = null;
            m_ptrIQParamBuf = null;
            m_ptrIQParamTable = null;
            m_ptrBlockLineBuf = null;
            m_ptrImageBuf = null;
            m_ptrYUVImage = null;
            m_ptrMovingVector = null;
            m_ptrMoveVecFlags = null;
            m_ptrMovePrevBlocks = null;
            if (m_pHuffmanTree != null)
            {
                m_pHuffmanTree = null;
            }
            if (m_pProbERISA != null)
            {
                m_pProbERISA = null;
            }
        }

        public virtual void DecodeImage(EGL_IMAGE_INFO dstimginf, ERISADecodeContext context,uint fdwFlags = dfTopDown)
        {
            EGL_IMAGE_INFO imginf = dstimginf;
            bool fReverse = ((fdwFlags & dfTopDown) != 0);
            if (m_eihInfo.nImageHeight < 0)
            {
                fReverse = !fReverse;
            }
            if (fReverse)
            {
                imginf.ptrImageArray.Offset = (int)((imginf.ptrImageArray.Offset) + (imginf.dwImageHeight - 1) * imginf.dwBytesPerLine);
                imginf.dwBytesPerLine = -imginf.dwBytesPerLine;
            }
            if ((imginf.fdwFormatType & Constants.EIF_SIDE_BY_SIDE) != 0)
            {
                imginf.dwImageWidth *= 2;
            }
            if (m_eihInfo.fdwTransformation == Constants.CVTYPE_LOSSLESS_ERI)
            {
                DecodeLosslessImage(imginf, context, fdwFlags);
            }
            else if ((m_eihInfo.fdwTransformation == Constants.CVTYPE_LOT_ERI) || (m_eihInfo.fdwTransformation == Constants.CVTYPE_DCT_ERI))
            {
                DecodeLossyImage(imginf, context, fdwFlags);
            }
            throw new Exception();
        }

        public void SetRefPreviousFrame(EGL_IMAGE_INFO pPrevFrame, EGL_IMAGE_INFO pNextFrame = null)
        {
            throw new NotImplementedException();
        }

        public EGL_IMAGE_INFO GetFilteredImageBuffer()
        {
            return m_pFilterImageBuf;
        }

        public void SetFilteredImageBuffer(EGL_IMAGE_INFO pImageBuf)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnDecodedBlock(int line, int column, Rectangle rect)
        {
            throw new NotImplementedException();
        }
    
        protected void DecodeLosslessImage(EGL_IMAGE_INFO imginf, ERISADecodeContext context, uint fdwFlags)
        {
            throw new NotImplementedException();
        }

        protected void InitalizeArrangeTable()
        {
            uint i, j, k, l, m;
            IntPointer ptrTable = new IntPointer(m_nBlockSamples * 4);
            IntPointer ptrNext;
            m_pArrangeTable[0] = ptrTable;
            m_pArrangeTable[1] = new IntPointer(ptrTable.Data, ptrTable.Offset + (m_nBlockSamples));
            m_pArrangeTable[2] = new IntPointer(ptrTable.Data, ptrTable.Offset + (m_nBlockSamples * 2));
            m_pArrangeTable[3] = new IntPointer(ptrTable.Data, ptrTable.Offset + (m_nBlockSamples * 3));

            ptrNext = m_pArrangeTable[0].Clone();
            for (i = 0; i < m_nBlockSamples; i++)
            {
                ptrNext.Data[ptrNext.Offset + i] = (int)i;
            }
            ptrNext = m_pArrangeTable[1].Clone();
            l = 0;
            for (i = 0; i < m_nChannelCount; i++)
            {
                for (j = 0; j < m_nBlockSize; j++)
                {
                    m = l + j;
                    for (k = 0; k < m_nBlockSize; k++)
                    {
                        ptrNext.Data[ptrNext.Offset++] = (int)m;
                        m += m_nBlockSize;
                    }
                }
                l += m_nBlockArea;
            }
            ptrNext = m_pArrangeTable[2].Clone();
            for (i = 0; i < m_nBlockArea; i++)
            {
                k = i;
                for (j = 0; j < m_nChannelCount; j++)
                {
                    ptrNext.Data[ptrNext.Offset++] = (int)k;
                    k += m_nBlockArea;
                }
            }
            ptrNext = m_pArrangeTable[3].Clone();
            for (i = 0; i < m_nBlockSize; i++)
            {
                l = i;
                for (j = 0; j < m_nBlockSize; j++)
                {
                    m = l;
                    l += m_nBlockSize;
                    for (k = 0; k < m_nChannelCount; k++)
                    {
                        ptrNext.Data[ptrNext.Offset++] = (int)m;
                        m += m_nBlockArea;
                    }
                }
            }
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

        protected void DecodeLossyImage(EGL_IMAGE_INFO imginf, ERISADecodeContext context, uint fdwFlags)
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
