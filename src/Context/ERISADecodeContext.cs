using System;
using System.IO;
using System.Threading;

namespace ERIShArp.Context
{

    
    public class ERISADecodeContext
    {
        protected int m_nIntBufCount;
        protected uint m_dwIntBuffer;
        protected uint m_nBufferingSize;
        protected uint m_nBufCount;
        protected byte[] m_ptrBuffer;
        protected byte[] m_ptrNextBuf;

        protected Stream m_pFile;
        protected ERISADecodeContext m_pContext;

        protected ContextPointer m_pfnDecodeSymbolBytes;

        protected int m_flgZero;
        protected uint m_nLength;

        protected uint m_dwERINAFlags;
        protected ERINA_HUFFMAN_TREE[] m_pLastHuffmanTree;
        protected ERINA_HUFFMAN_TREE[] m_ppHuffmanTree;

        protected uint m_dwCodeRegister;
        protected uint m_dwAugendRegister;
        protected int m_nPostBitCount;
        /// <summary>
        /// 4 bytes
        /// </summary>
        protected byte[] m_bytLastSymbol;
        protected int m_iLastSymbol;
        protected ERISA_PROB_MODEL m_pRobERISA;

        protected int m_nNemesisLeft;
        protected int m_nNemesisNext;
        protected byte[] m_pNemesisBuf;
        protected int m_nNemesisIndex;

        protected ERISAN_PHRASE_LOOKUP[] m_pNemesisLookup;
        protected ERISA_PROB_MODEL[] m_pPhraseLenProb;
        protected ERISA_PROB_MODEL[] m_pPhraseIndexProb;
        protected ERISA_PROB_MODEL[] m_pRunLenProb;
        protected ERISA_PROB_MODEL[] m_pLastERISAProb;
        protected ERISA_PROB_MODEL[] m_ppTableERISA;
        protected int m_nFlagEOF;

        protected ERIBshfBuffer[] m_pBshBuf;
        protected uint m_dwBufPos;

        protected Thread m_threadPredecode;
        protected event SimpleDelegate m_eventPredecoded;

        protected bool m_fPredecodingMode;
        protected byte[] m_bytPredecodeBuf;
        protected uint m_dwPredecodedBytes;
        protected uint m_dwUsedPredecodedBytes;
        protected uint m_dwPredecodeBufSize;

        protected enum ThreadMessage
        {
            thmsgPredecode = 0x0400,
            thmsgQuit = 0x0012
        }

        public ERISADecodeContext(uint nBufferingSize)
        {
            throw new NotImplementedException();
        }

        ~ERISADecodeContext()
        {
            throw new NotImplementedException();
        }

        public void AttachInputFile(Stream pfile)
        {
            throw new NotImplementedException();
        }

        public void AttachInputContext(ERISADecodeContext pcontext)
        {
            throw new NotImplementedException();
        }

        public virtual uint ReadNextData(byte[] ptrBuffer, uint nBytes)
        {
            throw new NotImplementedException();
        }

        public void PrefetchBuffer()
        {
            throw new NotImplementedException();
        }

        public int GetABit()
        {
            throw new NotImplementedException();
        }

        public uint GetNBits(int n)
        {
            throw new NotImplementedException();
        }

        public void FlushBuffer()
        {
            throw new NotImplementedException();
        }

        public uint DecodeSymbolBytes(byte[] ptrDst, uint nCount)
        {
            if (m_fPredecodingMode)
            {
                return ReadPredecodedCodeBytes(ptrDst, nCount);
            }
            if (m_pfnDecodeSymbolBytes != null) throw new Exception();
            return m_pfnDecodeSymbolBytes(ptrDst, nCount);
        }

        public void PrepareGammaCode()
        {
            throw new NotImplementedException();
        }

        public void InitGammaContext()
        {
            throw new NotImplementedException();
        }

        public int GetGammaCode()
        {
            throw new NotImplementedException();
        }

        public uint DecodeGammaCodeBytes(byte[] ptrDst, uint nCount)
        {
            throw new NotImplementedException();
        }

        public enum ERINAEncodingFlag : uint
        {
            efERINAOrder0 = 0x0000,
            efERINAOrder1 = 0x0001
        }

        public void PrepareToDecodeERINACode(uint dwFlags = (uint)ERINAEncodingFlag.efERINAOrder1)
        {
            throw new NotImplementedException();
        }

        public int GetHuffmanCode(ERINA_HUFFMAN_TREE[] tree)
        {
            throw new NotImplementedException();
        }

        public int GetLengthHuffman(ERINA_HUFFMAN_TREE[] tree)
        {
            throw new NotImplementedException();
        }
        
        public uint DecodeERINACodeBytes(byte[] ptrDst, uint nCount)
        {
            throw new NotImplementedException();
        }

        public void PrepareToPredecode()
        {
            throw new NotImplementedException();
        }

        public void PredecodeSymbolBytes(uint nCount)
        {
            throw new NotImplementedException();
        }

        public uint ReadPredecodedCodeBytes(byte[] ptrDst, uint nCount)
        {
            throw new NotImplementedException();
        }

        public static uint PredecodeThreadProc(Thread pThread, object pInstance)
        {
            throw new NotImplementedException();
        }

        public uint PredecodeThread()
        {
            throw new NotImplementedException();
        }

        public void PrepareToDecodeERISACode()
        {
            throw new NotImplementedException();
        }

        public void PrepareToDecodeERISANCode()
        {
            throw new NotImplementedException();
        }

        public void InitalizeERISACode()
        {
            throw new NotImplementedException();
        }

        public int DecodeERISACode(ERISA_PROB_MODEL[] pModel)
        {
            throw new NotImplementedException();
        }

        public int DecideERISACodeIndex(ERISA_PROB_MODEL[] pModel)
        {
            throw new NotImplementedException();
        }

        public uint DecodeERISACodeBytes(byte[] ptrDst, uint nCount)
        {
            throw new NotImplementedException();
        }

        public uint DecodeERISACodeWords(short[] ptrDst, uint nCount)
        {
            throw new NotImplementedException();
        }

        public int GetEOFFlag()
        {
            return m_nFlagEOF;
        }

        public void PrepareToDecodeBSHFCode(byte[] pszPassword)
        {
            throw new NotImplementedException();
        }

        public uint DecodeBSHFCodeBytes(byte[] ptrDst, uint nCount)
        {
            throw new NotImplementedException();
        }


    }
}
