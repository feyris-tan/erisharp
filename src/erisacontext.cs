using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERIShArp.Context
{
    public struct ERINA_HUFFMAN_NODE
    {
        public ushort m_weight;
        public ushort m_parent;
        public uint m_child_code;
    }

    public class ERINA_HUFFMAN_TREE
    {
        /// <summary>
        /// 0x201 elements (8 bytes in size)
        /// </summary>
        public ERINA_HUFFMAN_NODE[] m_hnTree;
        /// <summary>
        /// 0x100 elements
        /// </summary>
        int[] m_iSymLookup;
        public int m_iEscape;
        int m_iTreePointer;

        public void Initalize()
        {
            for (int i = 0; i < 0x100; i++)
            {
                m_iSymLookup[i] = (int)Constants.ERINA_HUFFMAN_NULL;
            }
            m_iEscape = (int)Constants.ERINA_HUFFMAN_NULL;
            m_iTreePointer = Constants.ERINA_HUFFMAN_ROOT;
            m_hnTree[Constants.ERINA_HUFFMAN_ROOT].m_weight = 0;
            m_hnTree[Constants.ERINA_HUFFMAN_ROOT].m_parent = (ushort)Constants.ERINA_HUFFMAN_NULL;
            m_hnTree[Constants.ERINA_HUFFMAN_ROOT].m_child_code = Constants.ERINA_HUFFMAN_NULL;
        }
        public void IncreaseOccuredCount(int iEntry)
        {
            m_hnTree[iEntry].m_weight++;
            Normalize(iEntry);
            if (m_hnTree[Constants.ERINA_HUFFMAN_ROOT].m_weight >= Constants.ERINA_HUFFMAN_MAX)
            {
                HalfAndRebuild();
            }
        }
        public void RecountOccuredCount(int iParent)
        {
            int iChild = (int)m_hnTree[iParent].m_child_code;
            m_hnTree[iParent].m_weight = (ushort)(m_hnTree[iChild].m_weight + m_hnTree[iChild + 1].m_weight);
        }
        public void Normalize(int iEntry)
        {
            while (iEntry < Constants.ERINA_HUFFMAN_ROOT)
            {
                int iSwap = iEntry + 1;
                ushort weight = m_hnTree[iEntry].m_weight;
                while (iSwap < Constants.ERINA_HUFFMAN_ROOT)
                {
                    if (m_hnTree[iSwap].m_weight >= weight)
                        break;
                    ++iSwap;
                }
                if (iEntry == --iSwap)
                {
                    iEntry = m_hnTree[iEntry].m_parent;
                    RecountOccuredCount(iEntry);
                    continue;
                }
                int iChild, nCode;
                if ((m_hnTree[iEntry].m_child_code & Constants.ERINA_CODE_FLAG) == 0)
                {
                    iChild = (int)m_hnTree[iEntry].m_child_code;
                    m_hnTree[iChild].m_parent = (ushort)iSwap;
                    m_hnTree[iChild + 1].m_parent = (ushort)iSwap;
                }
                else
                {
                    nCode = (int)(m_hnTree[iEntry].m_child_code & ~Constants.ERINA_CODE_FLAG);
                    if (nCode != Constants.ERINA_HUFFMAN_ESCAPE)
                        m_iSymLookup[nCode & 0xFF] = iSwap;
                    else
                        m_iEscape = iSwap;
                }
                if ((m_hnTree[iSwap].m_child_code & Constants.ERINA_CODE_FLAG) == 0)
                {
                    iChild = (int)m_hnTree[iSwap].m_child_code;
                    m_hnTree[iChild].m_parent = (ushort)iEntry;
                    m_hnTree[iChild + 1].m_parent = (ushort)iEntry;
                }
                else
                {
                    nCode = (int)(m_hnTree[iSwap].m_child_code & ~Constants.ERINA_CODE_FLAG);
                    if (nCode != Constants.ERINA_HUFFMAN_ESCAPE)
                        m_iSymLookup[nCode & 0xFF] = iEntry;
                    else
                        m_iEscape = iEntry;
                }
                ERINA_HUFFMAN_NODE node;
                ushort iEntryParent = m_hnTree[iEntry].m_parent;
                ushort iSwapParent = m_hnTree[iSwap].m_parent;
                node = m_hnTree[iSwap];
                m_hnTree[iSwap] = m_hnTree[iEntry];
                m_hnTree[iEntry] = node;
                m_hnTree[iSwap].m_parent = iSwapParent;
                m_hnTree[iEntry].m_parent = iEntryParent;

                RecountOccuredCount(iSwapParent);
                iEntry = iSwapParent;
            }
        }
        public void AddNewEntry(int nNewCode)
        {
            if (m_iTreePointer > 0)
            {
                int i = m_iTreePointer = m_iTreePointer - 2;

                ERINA_HUFFMAN_NODE phnNew = m_hnTree[i];
                phnNew.m_weight = 1;
                phnNew.m_child_code = (uint)(Constants.ERINA_CODE_FLAG | nNewCode);
                m_iSymLookup[nNewCode & 0xFF] = i;

                ERINA_HUFFMAN_NODE phnRoot = m_hnTree[Constants.ERINA_HUFFMAN_ROOT];
                if (phnRoot.m_child_code != Constants.ERINA_HUFFMAN_NULL)
                {
                    ERINA_HUFFMAN_NODE phnParent = m_hnTree[i + 2];
                    ERINA_HUFFMAN_NODE phnChild = m_hnTree[i + 1];
                    m_hnTree[i + 1] = m_hnTree[i + 2];
                    if ((phnChild.m_child_code & Constants.ERINA_CODE_FLAG) != 0)
                    {
                        int nCode = (int)(phnChild.m_child_code & ~Constants.ERINA_CODE_FLAG);
                        if (nCode != Constants.ERINA_HUFFMAN_ESCAPE)
                            m_iSymLookup[nCode & 0xFF] = i + 1;
                        else
                            m_iEscape = i + 1;
                    }
                    phnParent.m_weight = (ushort)(phnNew.m_weight + phnChild.m_weight);
                    phnParent.m_parent = phnChild.m_parent;
                    phnParent.m_child_code = (uint)i;
                    phnNew.m_parent = phnNew.m_parent = (ushort)(i + 2);
                    Normalize(i + 2);
                }
                else
                {
                    phnNew.m_parent = Constants.ERINA_HUFFMAN_ROOT;
                    ERINA_HUFFMAN_NODE phnEscape = m_hnTree[m_iEscape = i + 1];
                    phnEscape.m_weight = 1;
                    phnEscape.m_parent = Constants.ERINA_HUFFMAN_ROOT;
                    phnEscape.m_child_code = Constants.ERINA_CODE_FLAG | Constants.ERINA_HUFFMAN_ESCAPE;

                    phnRoot.m_weight = 2;
                    phnRoot.m_child_code = (uint)i;
                }
            }
            else
            {
                int i = m_iTreePointer;
                ERINA_HUFFMAN_NODE phnEntry = m_hnTree[i];
                if (phnEntry.m_child_code == (Constants.ERINA_CODE_FLAG | Constants.ERINA_HUFFMAN_ESCAPE))
                {
                    phnEntry = m_hnTree[i + 1];
                }
                phnEntry.m_child_code = (uint)(Constants.ERINA_CODE_FLAG | nNewCode);
            }
        }
        public void HalfAndRebuild()
        {
            int i;
            int iNextEntry = Constants.ERINA_HUFFMAN_ROOT;
            for (i = Constants.ERINA_HUFFMAN_ROOT - 1; i >= m_iTreePointer; i--)
            {
                if ((m_hnTree[i].m_child_code & Constants.ERINA_CODE_FLAG) != 0)
                {
                    m_hnTree[i].m_weight = (ushort)((m_hnTree[i].m_weight + 1) >> 1);
                    m_hnTree[iNextEntry--] = m_hnTree[i];
                }
            }
            ++iNextEntry;

            int iChild, nCode;
            i = m_iTreePointer;
            for (; ; )
            {
                m_hnTree[i] = m_hnTree[iNextEntry];
                m_hnTree[i + 1] = m_hnTree[iNextEntry + 1];
                iNextEntry += 2;
                ERINA_HUFFMAN_NODE phnChild1 = m_hnTree[i];
                ERINA_HUFFMAN_NODE phnChild2 = m_hnTree[i + 1];

                if ((phnChild1.m_child_code & Constants.ERINA_CODE_FLAG) == 0)
                {
                    iChild = (int)phnChild1.m_child_code;
                    m_hnTree[iChild].m_parent = (ushort)i;
                    m_hnTree[iChild + 1].m_parent = 1;
                }
                else
                {
                    nCode = (int)(phnChild1.m_child_code & ~Constants.ERINA_CODE_FLAG);
                    if (nCode == Constants.ERINA_HUFFMAN_ESCAPE)
                        m_iEscape = i;
                    else
                        m_iSymLookup[nCode & 0xFF] = i;
                }
                ushort weight = (ushort)(phnChild1.m_weight + phnChild2.m_weight);
                if (iNextEntry <= Constants.ERINA_HUFFMAN_ROOT)
                {
                    int j = iNextEntry;
                    for (; ; )
                    {
                        if (weight <= m_hnTree[j].m_weight)
                        {
                            m_hnTree[j - 1].m_weight = weight;
                            m_hnTree[j - 1].m_child_code = (uint)i;
                            break;
                        }
                        m_hnTree[j - 1] = m_hnTree[j];
                        if (++j > Constants.ERINA_HUFFMAN_ROOT)
                        {
                            m_hnTree[Constants.ERINA_HUFFMAN_ROOT].m_weight = weight;
                            m_hnTree[Constants.ERINA_HUFFMAN_ROOT].m_child_code = (uint)i;
                            break;
                        }
                    }
                    --iNextEntry;
                }
                else
                {
                    m_hnTree[Constants.ERINA_HUFFMAN_ROOT].m_weight = weight;
                    m_hnTree[Constants.ERINA_HUFFMAN_ROOT].m_parent = (ushort)Constants.ERINA_HUFFMAN_NULL;
                    m_hnTree[Constants.ERINA_HUFFMAN_ROOT].m_child_code = (uint)i;
                    phnChild1.m_parent = Constants.ERINA_HUFFMAN_ROOT;
                    phnChild2.m_parent = Constants.ERINA_HUFFMAN_ROOT;
                    break;
                }
                i += 2;
            }
        }

        public static uint Size
        {
            get
            {
                return ((0x201 * 8) + (0x100 * 4) + 4 + 4);
            }
        }
    }

    public struct ERISA_CODE_SYMBOL
    {
        public ushort wOccured;
        public short wSymbol;
    }

    public class ERISA_PROB_MODEL
    {
        public uint dwTotalCount;
        public uint dwSymbolSorts;
        /// <summary>
        /// 2 uints
        /// </summary>
        public uint[] dwReserved;
        /// <summary>
        /// ERISA_SYBOL_SORTS entries.
        /// </summary>
        public ERISA_CODE_SYMBOL[] acsSymTable;
        /// <summary>
        /// 3 uints
        /// </summary>
        public uint dwReserved2;
        /// <summary>
        /// ERISA_SUB_SORT_MAX entries;
        /// </summary>
        public ERISA_CODE_SYMBOL[] acsSubModel;

        public void Initalize()
        {
            dwTotalCount = Constants.ERISA_SYMBOL_SORTS;
            dwSymbolSorts = Constants.ERISA_SYMBOL_SORTS;

            int i;
            for (i = 0; i < 0x100; i++)
            {
                acsSymTable[i].wOccured = 1;
                acsSymTable[i].wSymbol = (short)(byte)i;
            }
            acsSymTable[0x100].wOccured = 1;
            acsSymTable[0x100].wSymbol = (short)Constants.ERISA_ESC_CODE;

            for (i = 0; i < Constants.ERISA_SUB_SORT_MAX; i++)
            {
                acsSubModel[i].wOccured = 0;
                acsSubModel[i].wSymbol = (short)-1;
            }
        }

        public int AccumulateProb(short wSymbol)
        {
            int iSym = FindSymbol(wSymbol);
            ESLAssert(iSym >= 0);
            uint dwOccured = acsSymTable[iSym].wOccured;
            int i = 0;
            while (dwOccured < dwTotalCount)
            {
                dwOccured <<= 1;
                i++;
            }
            return i;
        }

        public void HalfOccuredCount()
        {
            uint i;
            dwTotalCount = 0;
            for (i = 0; i < dwSymbolSorts; i++)
            {
                dwTotalCount += acsSymTable[i].wOccured = (ushort)((acsSymTable[i].wOccured + 1) >> 1);
            }
            for (i = 0; i < Constants.ERISA_SUB_SORT_MAX; i++)
            {
                acsSubModel[i].wOccured >>= 1;
            }
        }

        public int IncreaseSymbol(int index)
        {
            ushort wOccured = ++acsSymTable[index].wOccured;
            short wSymbol = acsSymTable[index].wSymbol;

            while (--index >= 0)
            {
                if (acsSymTable[index].wOccured >= wOccured)
                    break;
                acsSymTable[index + 1] = acsSymTable[index];
            }
            acsSymTable[++index].wOccured = wOccured;
            acsSymTable[index].wSymbol = wSymbol;

            if (++dwTotalCount >= Constants.ERISA_TOTAL_LIMIT)
            {
                HalfOccuredCount();
            }

            return index;
        }

        public int FindSymbol(short wSymbol)
        {
            int iSym = 0;
            while (acsSymTable[iSym].wSymbol != wSymbol)
            {
                if ((uint)(++iSym) >= dwSymbolSorts)
                    return -1;
            }
            return iSym;
        }

        public int AddSymbol(short wSymbol)
        {
            int iSym = (int)dwSymbolSorts++;
            dwTotalCount++;
            acsSymTable[iSym].wSymbol = wSymbol;
            acsSymTable[iSym].wOccured = 1;
            return iSym;
        }

        private void ESLAssert(bool condition)
        {
            if (!condition)
                throw new Exception();
        }
    }

    public struct ERISA_PROB_BASE
    {
        public ERISA_PROB_MODEL[] ptrProbWork;
        public uint dwWorkUsed;
        /// <summary>
        /// 2 uints
        /// </summary>
        public uint dwReserved;
        public ERISA_PROB_MODEL epmBaseModel;
        /// <summary>
        /// ERISA_PROB_SLOT_MAX entries;
        /// </summary>
        public ERISA_PROB_MODEL[] ptrProbIndex;

        /// <summary>
        /// 4 ints
        /// </summary>
        public int[] m_nShiftCount;
        /// <summary>
        /// 4 ints
        /// </summary>
        public int[] m_nNewProbLimit;
    }

    public struct ERISAN_PHRASE_LOOKUP
    {
        public uint first;
        /// <summary>
        /// NEMESIS_INDEX_LIMIT entries;
        /// </summary>
        public uint[] index;
    }

    public delegate uint ContextPointer(byte[] ptrDst, uint nCount);
    public delegate void SimpleDelegate();
}
