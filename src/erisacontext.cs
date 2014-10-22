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

    public struct ERINA_HUFFMAN_TREE
    {
        /// <summary>
        /// 0x201 elements
        /// </summary>
        ERINA_HUFFMAN_NODE[] m_hnTree;
        /// <summary>
        /// 0x100 elements
        /// </summary>
        int[] m_iSymLookup;
        int m_iEscape;
        int m_iTreePointer;

        public void Initalize()
        {
            throw new NotImplementedException();
        }
        public void IncreaseOccuredCount(int iEntry)
        {
            throw new NotImplementedException();
        }
        public void RecountOccuredCount(int iParent)
        {
            throw new NotImplementedException();
        }
        public void Normalize(int iEntry)
        {
            throw new NotImplementedException();
        }
        public void AddNewEntry(int nNewCode)
        {
            throw new NotImplementedException();
        }
        public void HalfAndRebuild()
        {
            throw new NotImplementedException();
        }
    }

    public struct ERISA_CODE_SYMBOL
    {
        public ushort wOccured;
        public short wSymbol;
    }

    public struct ERISA_PROB_MODEL
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
            throw new NotImplementedException();
        }

        public int AccumulateProbe(short wSymbol)
        {
            throw new NotImplementedException();
        }

        public void HalfOccuredCount()
        {
            throw new NotImplementedException();
        }

        public int IncreaseSymbol(int index)
        {
            throw new NotImplementedException();
        }

        public int FindSymbol(short wSymbol)
        {
            throw new NotImplementedException();
        }

        public int AddSymbol(short wSymbol)
        {
            throw new NotImplementedException();
        }
    }

    public struct ERISSA_PROB_BASE
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
