using System;
using System.IO;
using System.Threading;
using ERIShArp.File;

namespace ERIShArp.Context
{
    
    public class ERISADecodeContext
    {
        protected int m_nIntBufCount;
        protected uint m_dwIntBuffer;
        protected uint m_nBufferingSize;
        protected uint m_nBufCount;
        protected byte[] m_ptrBuffer;
        protected Pointer m_ptrNextBuf;

        protected EMCFile m_pFile;
        protected ERISADecodeContext m_pContext;

        protected ContextPointer m_pfnDecodeSymbolBytes;

        protected int m_flgZero;
        protected uint m_nLength;

        protected uint m_dwERINAFlags;
        protected ERINA_HUFFMAN_TREE m_pLastHuffmanTree;
        protected ERINA_HUFFMAN_TREE[] m_ppHuffmanTree;

        protected uint m_dwCodeRegister;
        protected uint m_dwAugendRegister;
        protected int m_nPostBitCount;
        /// <summary>
        /// 4 bytes
        /// </summary>
        protected byte[] m_bytLastSymbol;
        protected int m_iLastSymbol;
        protected ERISA_PROB_MODEL m_pProbERISA;

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
        protected byte[] m_pbytPredecodeBuf;
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
            m_nIntBufCount = 0;
            m_nBufferingSize = (uint)((nBufferingSize + 0x03) & ~0x03);
            m_nBufCount = 0;
            m_ptrBuffer = new byte[nBufferingSize];
            m_pFile = null;
            m_pContext = null;

            m_pfnDecodeSymbolBytes = null;

            m_ppHuffmanTree = null;
            m_pProbERISA = null;
            m_pNemesisBuf = null;
            m_pNemesisLookup = null;
            m_pPhraseLenProb = null;
            m_pPhraseIndexProb = null;
            m_pRunLenProb = null;
            m_ppTableERISA = null;
            m_nFlagEOF = 0;
            m_pBshBuf = null;

            m_fPredecodingMode = false;
            m_pbytPredecodeBuf = null;
            m_dwPredecodedBytes = 0;
            m_dwUsedPredecodedBytes = 0;
            m_dwPredecodeBufSize = 0;
        }

        ~ERISADecodeContext()
        {
            m_ptrBuffer = null;
            if (m_ppHuffmanTree != null) m_ppHuffmanTree = null;
            if (m_pProbERISA != null) m_pProbERISA = null;
            if (m_pNemesisBuf != null) m_pNemesisBuf = null;
            if (m_pNemesisLookup != null) m_pNemesisLookup = null;
            if (m_pPhraseLenProb != null) m_pPhraseLenProb = null;
            if (m_pPhraseIndexProb != null) m_pPhraseIndexProb = null;
            if (m_pRunLenProb != null) m_pRunLenProb = null;
            if (m_ppTableERISA != null) m_ppTableERISA = null;
            m_pBshBuf = null;
            if (m_pbytPredecodeBuf != null)
            {
                m_pbytPredecodeBuf = null;
                m_dwPredecodedBytes = 0;
                m_dwPredecodeBufSize = 0;
            }
            if (m_threadPredecode.IsAlive)
            {
                m_threadPredecode.Abort();
                m_threadPredecode.Join();
            }
        }

        public void AttachInputFile(EMCFile pfile)
        {
            m_pFile = pfile;
            m_pContext = null;
        }

        public void AttachInputContext(ERISADecodeContext pcontext)
        {
            m_pFile = null;
            m_pContext = pcontext;
        }

        public virtual uint ReadNextData(byte[] ptrBuffer, uint nBytes)
        {
            if (m_pFile != null)
            {
                return m_pFile.Read(ptrBuffer, nBytes);
            }
            else if (m_pContext != null)
            {
                return m_pContext.DecodeSymbolBytes(ptrBuffer, nBytes);
            }
            else
            {
                throw new IOException();
            }
        }

        public bool PrefetchBuffer()
        {
            if (m_nIntBufCount == 0)
            {
                if (m_nBufCount == 0)
                {
                    m_ptrNextBuf.Data = m_ptrBuffer;
                    m_ptrNextBuf.Offset = 0;
                    m_nBufCount = ReadNextData(m_ptrBuffer, m_nBufferingSize);
                    if (m_nBufCount == 0)
                    {
                        return true;
                    }
                    if ((m_nBufCount & 0x03) != 0)
                    {
                        uint i = m_nBufCount;
                        m_nBufCount += 4 - (m_nBufCount & 0x03);
                        while (i < m_nBufCount)
                            m_ptrBuffer[i++] = 0x00;
                    }
                }
                m_nIntBufCount = 32;
                m_dwIntBuffer = m_ptrNextBuf.PeekUInt32;
                m_ptrNextBuf += 4;
                m_nBufCount -= 4;
            }
            return false;
        }

        public int GetABit()
        {
            PrefetchBuffer();
            int nValue = (int)(((short)m_dwIntBuffer) >> 31);
            --m_nIntBufCount;
            m_dwIntBuffer <<= 1;
            return nValue;
        }

        public uint GetNBits(int n)
        {
            uint nCode = 0;
            while (n != 0)
            {
                PrefetchBuffer();
                int nCopyBits = n;
                if (nCopyBits > m_nIntBufCount)
                    nCopyBits = m_nIntBufCount;
                nCode = (nCode << nCopyBits) | (m_dwIntBuffer >> (32 - nCopyBits));
                n -= nCopyBits;
                m_nIntBufCount -= nCopyBits;
                m_dwIntBuffer <<= nCopyBits;
            }
            return nCode;
        }

        public void FlushBuffer()
        {
            m_nIntBufCount = 0;
            m_nBufCount = 0;
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
            m_pfnDecodeSymbolBytes = DecodeGammaCodeBytes;
        }

        public void InitGammaContext()
        {
            m_flgZero = GetABit();
            m_nLength = 0;
        }

        public int GetGammaCode()
        {
            PrefetchBuffer();
            uint dwIntBuf;
            m_nIntBufCount--;
            dwIntBuf = m_dwIntBuffer;
            m_dwIntBuffer <<= 1;
            if ((dwIntBuf & 0x80000000) == 0)
            {
                return 1;
            }

            int nCode = 0, nBase = 2;
            PrefetchBuffer();
            if (((~m_dwIntBuffer & 0x55000000) != 0) && (m_nIntBufCount >= 8))
            {
                int i = (int)((m_dwIntBuffer >> 24) << 1);
                nCode = nGammaCodeLookup[i];
                int nBitCount = nGammaCodeLookup[i + 1];
                ESLAssert(nBitCount <= m_nIntBufCount);
                ESLAssert(nCode > 0);
                m_nIntBufCount -= nBitCount;
                m_dwIntBuffer <<= nBitCount;
                return nCode;
            }

            nCode = 0; nBase = 2;
            for (; ; )
            {
                if (m_nIntBufCount >= 2)
                {
                    dwIntBuf = m_dwIntBuffer;
                    m_dwIntBuffer <<= 2;
                    nCode = (int)((nCode << 1) | (dwIntBuf >> 31));
                    m_nIntBufCount -= 2;
                    if ((dwIntBuf & 0x40000000) == 0)
                    {
                        return nCode + nBase;
                    }
                    nBase <<= 1;
                }
                else
                {
                    PrefetchBuffer();
                    nCode = (int)((nCode << 1) | (m_dwIntBuffer >> 31));
                    m_nIntBufCount--;
                    m_dwIntBuffer <<= 1;
                    
                    PrefetchBuffer();
                    dwIntBuf = m_dwIntBuffer;
                    m_nIntBufCount--;
                    m_dwIntBuffer <<= 1;
                    if ((dwIntBuf & 0x80000000) == 0)
                    {
                        return nCode + nBase;
                    }
                    nBase <<= 1;
                }
            }
        }

        public uint DecodeGammaCodeBytes(byte[] ptrDst, uint nCount)
        {
            int ptr = 0;
            uint nDecoded = 0, nRepeat;
            byte nSign, nCode;

            if (m_nLength == 0)
            {
                m_nLength = (uint)GetGammaCode();
                if (m_nLength == 0)
                {
                    return nDecoded;
                }
            }
            for (;;)
            {
                nRepeat = m_nLength;
                if (nRepeat > nCount)
                {
                    nRepeat = nCount;
                }
                ESLAssert(nRepeat > 0);
                m_nLength -= nRepeat;
                nCount -= nRepeat;
                if (m_flgZero == 0)
                {
                    nDecoded += nRepeat;
                    do
                    {
                        ptrDst[ptr++] = 0;
                    }
                    while (--nRepeat != 0);
                }
                else
                {
                    do
                    {
                        nSign = (byte)GetABit();
                        nCode = (byte)GetGammaCode();
                        if (nCode == 0)
                        {
                            return nDecoded;
                        }
                        nDecoded++;
                        ptrDst[ptr++] = (byte)((nCode ^ nSign) - nSign);
                    }
                    while (--nRepeat != 0);
                }
                if (nCount == 0)
                {
                    if (m_nLength == 0)
                    {
                        m_flgZero = ~m_flgZero;
                    }
                    return nDecoded;
                }

                m_flgZero = ~m_flgZero;
                m_nLength = (uint)GetGammaCode();
                if (m_nLength == 0)
                {
                    return nDecoded;
                }
            }
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

        public int GetHuffmanCode(ERINA_HUFFMAN_TREE tree)
        {
            int nCode;
            if (tree.m_iEscape != Constants.ERINA_HUFFMAN_NULL)
            {
                int iEntry = Constants.ERINA_HUFFMAN_ROOT;
                int iChild = (int)(tree.m_hnTree[Constants.ERINA_HUFFMAN_ROOT].m_child_code);

                do
                {
                    if (PrefetchBuffer())
                    {
                        return Constants.ERINA_HUFFMAN_ESCAPE;
                    }
                    iEntry = (int)(iChild + (m_dwIntBuffer >> 31));
                    --m_nIntBufCount;
                    iChild = (int)(tree.m_hnTree[iEntry].m_child_code);
                    m_dwIntBuffer <<= 1;
                } while ((iChild & Constants.ERINA_CODE_FLAG) ==0);
                if ((m_dwERINAFlags != (uint)ERINAEncodingFlag.efERINAOrder0) || (tree.m_hnTree[Constants.ERINA_HUFFMAN_ROOT].m_weight < Constants.ERINA_HUFFMAN_MAX-1))
                {
                    tree.IncreaseOccuredCount(iEntry);
                }
                nCode = (int)(iChild & ~Constants.ERINA_CODE_FLAG);
                if (nCode != Constants.ERINA_HUFFMAN_ESCAPE)
                {
                    return nCode;
                }
            }
            nCode = (int)(GetNBits(8));
            tree.AddNewEntry(nCode);
            return nCode;
        }

        public int GetLengthHuffman(ERINA_HUFFMAN_TREE tree)
        {
            int nCode;
            if (tree.m_iEscape != Constants.ERINA_HUFFMAN_NULL)
            {
                int iEntry = Constants.ERINA_HUFFMAN_ROOT;
                int iChild = (int)tree.m_hnTree[Constants.ERINA_HUFFMAN_ROOT].m_child_code;
                do
                {
                    if (PrefetchBuffer())
                    {
                        return Constants.ERINA_HUFFMAN_ESCAPE;
                    }
                    iEntry = (int)(iChild + (m_dwIntBuffer >> 31));
                    --m_nIntBufCount;
                    iChild = (int)tree.m_hnTree[iEntry].m_child_code;
                    m_dwIntBuffer <<= 1;
                } while ((iChild & Constants.ERINA_CODE_FLAG) == 0);
                if ((m_dwERINAFlags != (uint)ERINAEncodingFlag.efERINAOrder0) || (tree.m_hnTree[Constants.ERINA_HUFFMAN_ROOT].m_weight < Constants.ERINA_HUFFMAN_MAX-1))
                {
                    tree.IncreaseOccuredCount(iEntry);
                }
                nCode = (int)(iChild & ~Constants.ERINA_CODE_FLAG);
                if (nCode != Constants.ERINA_HUFFMAN_ESCAPE)
                {
                    return nCode;
                }
            }
            nCode = GetGammaCode();
            if (nCode == -1)
            {
                return Constants.ERINA_HUFFMAN_ESCAPE;
            }
            tree.AddNewEntry(nCode);
            return nCode;
        }
        
        public uint DecodeERINACodeBytes(byte[] ptrDst, uint nCount)
        {
            ERINA_HUFFMAN_TREE tree = m_pLastHuffmanTree;
            int symbol, length;
            uint i = 0;
            if (m_nLength > 0)
            {
                length = (int)m_nLength;
                if (length > (int)nCount)
                {
                    length = (int)nCount;
                }
                m_nLength -= (uint)length;
                do
                {
                    ptrDst[i++] = 0;
                } while (--length != 0);
            }
            while (i < nCount)
            {
                symbol = GetHuffmanCode(tree);
                if (symbol == Constants.ERINA_HUFFMAN_ESCAPE)
                {
                    break;
                }
                ptrDst[i++] = (byte)symbol;
                if (symbol == 0)
                {
                    length = GetLengthHuffman(m_ppHuffmanTree[0x100]);
                    if (length == Constants.ERINA_HUFFMAN_ESCAPE)
                    {
                        break;
                    }
                    if (--length != 0)
                    {
                        m_nLength = (uint)length;
                        if (i + length > nCount)
                        {
                            length = (int)(nCount - i);
                        }
                        m_nLength -= (uint)length;
                        if (length > 0)
                        {
                            do
                            {
                                ptrDst[i++] = 0;
                            } while (--length != 0);
                        }
                    }
                }
                tree = m_ppHuffmanTree[symbol & 0xFF];
            }
            m_pLastHuffmanTree = tree;
            return i;
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
            m_nLength = 0;
            m_dwCodeRegister = GetNBits(32);
            m_dwAugendRegister = 0xFFFF;
            m_nPostBitCount = 0;
        }

        public int DecodeERISACode(ERISA_PROB_MODEL pModel)
        {
            int iSym = DecodeERISACodeIndex(pModel);
            int nSymbol = Constants.ERISA_ESC_CODE;
            if (iSym >= 0)
            {
                nSymbol = pModel.acsSymTable[iSym].wSymbol;
                pModel.IncreaseSymbol(iSym);
            }
            return nSymbol;
        }

        public int DecodeERISACodeIndex(ERISA_PROB_MODEL pModel)
        {
            uint dwAcc = m_dwCodeRegister * pModel.dwTotalCount / m_dwAugendRegister;
            if (dwAcc >= Constants.ERISA_TOTAL_LIMIT)
            {
                return -1;
            }

            int iSym = 0;
            short wAcc = (short)dwAcc;
            short wFs = 0;
            short wOccured;
            for (; ; )
            {
                wOccured = (short)pModel.acsSymTable[iSym].wOccured;
                if (wAcc < wOccured)
                {
                    break;
                }
                wAcc -= wOccured;
                wFs += wOccured;
                if ((uint)++iSym >= pModel.dwSymbolSorts)
                {
                    return -1;
                }
            }
            m_dwCodeRegister -= (uint)((m_dwAugendRegister * wFs + pModel.dwTotalCount - 1) / pModel.dwTotalCount);
            m_dwAugendRegister = (uint)(m_dwAugendRegister * wOccured / pModel.dwTotalCount);
            ESLAssert(m_dwAugendRegister != 0);
            while ((m_dwAugendRegister & 0x8000) == 0)
            {
                int nNextBit = GetABit();
                if (nNextBit == 1)
                {
                    if ((++m_nPostBitCount) >= 256)
                    {
                        return -1;
                    }
                    nNextBit = 0;
                }
                m_dwCodeRegister = (uint)((m_dwCodeRegister << 1) | (nNextBit & 0x01));
                m_dwAugendRegister <<= 1;
            }
            ESLAssert((m_dwAugendRegister & 0x8000) != 0);
            m_dwCodeRegister &= 0xFFFF;
            return iSym;
        }

        public uint DecodeERISACodeBytes(byte[] ptrDst, uint nCount)
        {
            throw new NotImplementedException();
        }

        public uint DecodeERISACodeWords(short[] ptrDst, uint nCount)
        {
            throw new NotImplementedException();
        }

        public uint DecodeERISANCodeBytes(byte[] ptrDst, uint nCount)
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

        private void ESLAssert(bool condition)
        {
            if (!condition)
                throw new Exception();
        }

        private static readonly byte[] nGammaCodeLookup = {
   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,
   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,
   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,
   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,
   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,
   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,
   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,
   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,   2,  2,
   4,  4,   4,  4,   4,  4,   4,  4,   4,  4,   4,  4,   4,  4,   4,  4,
   4,  4,   4,  4,   4,  4,   4,  4,   4,  4,   4,  4,   4,  4,   4,  4,
   8,  6,   8,  6,   8,  6,   8,  6,  16,  8,  255, 255,  17,  8,  255, 255,
   9,  6,   9,  6,   9,  6,   9,  6,  18,  8,  255, 255,  19,  8,  255, 255,
   5,  4,   5,  4,   5,  4,   5,  4,   5,  4,   5,  4,   5,  4,   5,  4,
   5,  4,   5,  4,   5,  4,   5,  4,   5,  4,   5,  4,   5,  4,   5,  4,
  10,  6,  10,  6,  10,  6,  10,  6,  20,  8,  255, 255,  21,  8,  255, 255,
  11,  6,  11,  6,  11,  6,  11,  6,  22,  8,  255, 255,  23,  8,  255, 255,
   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,
   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,
   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,
   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,
   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,
   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,
   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,
   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,   3,  2,
   6,  4,   6,  4,   6,  4,   6,  4,   6,  4,   6,  4,   6,  4,   6,  4,
   6,  4,   6,  4,   6,  4,   6,  4,   6,  4,   6,  4,   6,  4,   6,  4,
  12,  6,  12,  6,  12,  6,  12,  6,  24,  8,  255, 255,  25,  8,  255, 255,
  13,  6,  13,  6,  13,  6,  13,  6,  26,  8,  255, 255,  27,  8,  255, 255,
   7,  4,   7,  4,   7,  4,   7,  4,   7,  4,   7,  4,   7,  4,   7,  4,
   7,  4,   7,  4,   7,  4,   7,  4,   7,  4,   7,  4,   7,  4,   7,  4,
  14,  6,  14,  6,  14,  6,  14,  6,  28,  8,  255, 255,  29,  8,  255, 255,
  15,  6,  15,  6,  15,  6,  15,  6,  30,  8,  255, 255,  31,  8,  255, 255
};
    }
}
