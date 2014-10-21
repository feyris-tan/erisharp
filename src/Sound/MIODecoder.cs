using System;
using ERIShArp.X;
using ERIShArp.Matrix;
using ERIShArp.Context;

namespace ERIShArp.Sound
{
    public class MIODecoder
    {
        MIO_INFO_HEADER		m_mioih ;				// ‰¹ºî•ñƒwƒbƒ_

        uint		m_nBufLength ;			
        byte[]				m_ptrBuffer1 ;			
        byte[]				m_ptrBuffer2 ;			
        byte[]				m_ptrBuffer3 ;			
        byte[]				m_ptrDivisionTable ;	
        byte[]				m_ptrRevolveCode ;		
        uint[]			m_ptrWeightCode ;		
        int[]				m_ptrCoefficient ;		
        uint[]			m_ptrMatrixBuf ;		
        uint[]			m_ptrInternalBuf ;		
        uint[]			m_ptrWorkBuf ;			
        uint[]			m_ptrWorkBuf2 ;
        uint[]			m_ptrWeightTable ;		
        uint[]			m_ptrLastDCT ;			

        byte[]				m_ptrNextDivision ;		
        byte[]				m_ptrNextRevCode ;		
        uint[]			m_ptrNextWeight ;		
        int[]				m_ptrNextCoefficient ;	
        int[]				m_ptrNextSource ;		
        float[]			m_ptrLastDCTBuf ;		
        uint		m_nSubbandDegree ;		
        uint		m_nDegreeNum ;
        ERI_SIN_COS		m_pRevolveParam ;
            /// <summary>
            /// 7 * int
            /// </summary>
        int[]					m_nFrequencyPoint;

        public MIODecoder()
        {
            throw new NotImplementedException();
        }

        ~MIODecoder()
        {
            throw new NotImplementedException();
        }
    
        public virtual void Initalize(MIO_INFO_HEADER infhdr)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete()
        {
            throw new NotImplementedException();
        }

        public virtual void DecodeSound(ERISADecodeContext context, MIO_DATA_HEADER datahdr, byte[] ptrWaveBuf)
        {
            throw new NotImplementedException();
        }

        protected void DecodeSoundPCM8(ERISADecodeContext context, MIO_DATA_HEADER datahdr, byte[] ptrWaveBuf)
        {
            throw new NotImplementedException();
        }

        protected void DecodeSoundPCM16(ERISADecodeContext context, MIO_DATA_HEADER datahdr, byte[] ptrWaveBuf)
        {
            throw new NotImplementedException();
        }

        protected void InitalizeWithDegree(uint nSubbandDegree)
        {
            throw new NotImplementedException();
        }

        protected void DecodeSoundDCT(ERISADecodeContext context, MIO_DATA_HEADER datahdr, byte[] ptrWaveBuf)
        {
            throw new NotImplementedException();
        }

        protected void DecodeInternalBlock(int[] ptrDst, uint nSamples)
        {
            throw new NotImplementedException();
        }

        protected void DecodeLeadBlock()
        {
            throw new NotImplementedException();
        }

        protected void DecodePostBlock(int[] ptrDst, uint nSamples)
        {
            throw new NotImplementedException();
        }

        protected void IQuantumize(float[] ptrDestination, int[] ptrQuantumized, int nDegreeNum, int[] nWeightCode, int nCoefficient)
        {
            throw new NotImplementedException();
        }

        protected void DecodeSoundDCT_MSS(ERISADecodeContext context, MIO_DATA_HEADER datahdr, byte[] ptrWaveBuf)
        {
            throw new NotImplementedException();
        }

        protected void DecodeInternalBlock_MSS(int[] ptrDst, uint nSamples)
        {
            throw new NotImplementedException();
        }

        protected void DecodeLeadBlock_MSS()
        {
            throw new NotImplementedException();
        }

        protected void DecodePostBlock_MSS(int[] ptrDst, uint nSamples)
        {
            throw new NotImplementedException();
        }

    }
}
