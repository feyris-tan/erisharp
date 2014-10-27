using System;
using System.IO;

namespace ERIShArp.Context
{
    public class ERISAEncodeContext
    {
        protected int m_nIntBufCount;
        protected uint m_dwIntBuffer;
        protected uint m_nBufferingSize;
        protected uint m_nBufCount;
        protected byte[] m_ptrBuffer;

        protected Stream m_pFile;
        protected ERISAEncodeContext m_pContext;

        protected ContextPointer m_pfnEncodeSymbolBytes;
        protected SimpleDelegate m_pfnFinishEncoding;

        protected uint m_dwERINAFlags;
        protected ERINA_HUFFMAN_TREE[] m_pLastHuffmanTree;
        protected ERINA_HUFFMAN_TREE[] m_ppHuffmanTree;

        protected uint m_dwCodeRegister;
        protected uint m_dwAugendRegister;
        protected uint m_dwCodeBuffer;
        protected int m_dwBitBufferCount;

        /// <summary>
        /// 4 bytes
        /// </summary>
        protected byte[] m_bytLastSymbol;
        protected int m_iLastSymbol;
        protected ERISA_PROB_BASE[] m_pProbERISA;
        protected uint m_dwERISAFlags;
        protected byte[] m_pNemesisBuf;
        protected int m_nNemesisIndex;
        protected ERISAN_PHRASE_LOOKUP[] m_pNemesisLookup;
        protected ERISA_PROB_MODEL[] m_pPhraseLenProb;
        protected ERISA_PROB_MODEL[] m_pPhraseIndexProb;
        protected ERISA_PROB_MODEL[] m_pRunLenProb;
        protected ERISA_PROB_MODEL[] m_pLastERISAProb;
        protected ERISA_PROB_MODEL[] m_ppTableERISA;

        protected ERIBshfBuffer[] m_pBshfBuf;
        protected uint m_dwBufPos;

        public ERISAEncodeContext(uint nBufferingSize)
        {
            throw new NotImplementedException();
        }

        ~ERISAEncodeContext()
        {
            throw new NotImplementedException();
        }

        public void AttachOutputFile(Stream pfile)
        {
            throw new NotImplementedException();
        }

        public void AttachOutputContext(ERISAEncodeContext pcontext)
        {
            throw new NotImplementedException();
        }

        public virtual uint WriteNextData(byte[] ptrBuffer, uint nBytes)
        {
            throw new NotImplementedException();
        }

        public void OutNBits(uint dwData, int nBits)
        {
            throw new NotImplementedException();
        }

        public void Flushout()
        {
            throw new NotImplementedException();
        }

        public uint EncodeSymbolBytes(byte[] ptrSrc, uint nCount)
        {
            if (m_pfnEncodeSymbolBytes != null) throw new Exception();
            return m_pfnEncodeSymbolBytes(ptrSrc, nCount);
        }

        public void FinishEncoding()
        {
            if (m_pfnFinishEncoding != null) throw new Exception();
            m_pfnFinishEncoding();
        }

        public void PrepareToEncodeGammaCode()
        {
            throw new NotImplementedException();
        }

        public static uint EstimateGammaCode(int num)
        {
            throw new NotImplementedException();
        }

        public static uint EstimateGammaCodeBytes(byte[] ptrSrc, uint nCount)
        {
            throw new NotImplementedException();
        }

        public void OutGammaCode(int num)
        {
            throw new NotImplementedException();
        }

        public uint EncodeGammaCodeBytes(byte[] ptrSrc, uint nCount)
        {
            throw new NotImplementedException();
        }

        public enum ERINAEncodingFlag : uint 
        {
            efERINAOrder0 = 0x0000,
            efERINAOrder1 = 0x0001
        }

        public void PrepareToEncodeERINACode(uint dwFlags = (uint)ERINAEncodingFlag.efERINAOrder1)
        {
            throw new NotImplementedException();
        }

        public void OutHuffmanCode(ERINA_HUFFMAN_TREE tree, int num)
        {
            throw new NotImplementedException();
        }

        public void OutLengthHuffman(ERINA_HUFFMAN_TREE tree, int length)
        {
            throw new NotImplementedException();
        }

        public uint EncodeERINACodeBytes(byte[] ptrSrc, uint nCount)
        {
            throw new NotImplementedException();
        }

        public enum ERISAEncodingFlag : uint
        {
            efSimple = 0x0000,
            efNemesis = 0x0001,
            efRunLength = 0x0002,
            efRLNemesis = 0x0003
        }

        public void PrepareToEncodeERISACode()
        {
            throw new NotImplementedException();
        }

        public void PrepareToEncodeERISANCode(uint dwFlags = (uint)ERISAEncodingFlag.efNemesis)
        {
            throw new NotImplementedException();
        }

        public int EncodeERISACodeSymbol(ERISA_PROB_MODEL pModel, short wSymbol)
        {
            throw new NotImplementedException();
        }

        public int EncodeERISACodeIndex(ERISA_PROB_MODEL pModel, int iSym, ushort wFs)
        {
            throw new NotImplementedException();
        }

        public uint EncodeERISACodeBytes(byte[] ptrSrc, uint nCount)
        {
            throw new NotImplementedException();
        }

        public uint EncodeERISACodeWords(int[] ptrSrc, uint nCount)
        {
            throw new NotImplementedException();
        }

        public uint EncodeERISANCodeBytes(byte[] ptrSrc, uint nCount)
        {
            throw new NotImplementedException();
        }

        public void EncodeERISANCodeEOF()
        {
            throw new NotImplementedException();
        }

        public void FinishERISACode()
        {
            throw new NotImplementedException();
        }

        public void PrepareToEncodeBSHFCode(byte[] pszPssword)
        {
            throw new NotImplementedException();
        }

        public uint EncodeBSHFCodeBytes(byte[] ptrSrc, uint nCount)
        {
            throw new NotImplementedException();
        }

        public void FinishBSHFCode()
        {
            throw new NotImplementedException();
        }
    }
}
