using System;
using ERIShArp.X;
using ERIShArp.Matrix;
using ERIShArp.Context;

namespace ERIShArp.Sound
{
    public class MIOEncoder
    {
        protected MIO_INFO_HEADER m_mioih;

        protected uint		m_nBufLength ;			
        protected byte[] m_ptrBuffer1;			
        protected byte[] m_ptrBuffer2;			
        protected byte[] m_ptrBuffer3;			
        protected float m_ptrSamplingBuf;	
        protected float m_ptrInternalBuf;	
        protected float m_ptrDstBuf;		
        protected float m_ptrWorkBuf;			
        protected float m_ptrWeightTable;		
        protected float m_ptrLastDCT;			

        public enum PresetParameter
        {
            ppVBR235kbps, ppVBR176kbps, ppVBR156kbps,
            ppVBR141kbps, ppVBR128kbps, ppVBR117kbps,
            ppVBR94kbps, ppVBR78kbps, ppVBR70kbps,
            ppMax
        }

        public struct PARAMETER
        {
            double rLowWeight;		
            double rMiddleWeight;		
            double rPowerScale;		
            int nOddWeight;	
            int nPreEchoThreshold;	

            public void LoadPresetParam(PresetParameter ppIndex, MIO_INFO_HEADER infhdr)
            {
                throw new NotImplementedException();
            }
        }

        PARAMETER			m_parameter ;

    	byte[]				m_ptrNextDstBuf ;	
	    float[]			m_ptrLastDCTBuf ;	
	    uint		m_nSubbandDegree ;	
	    uint		m_nDegreeNum ;

        /// <summary>
        /// 7x 32bit-Integers
        /// </summary>
	    int[]					m_nFrequencyWidth ;	
        /// <summary>
        /// 7x 32-bit Integers
        /// </summary>
	    int[]					m_nFrequencyPoint ;
	    ERI_SIN_COS		m_pRevolveParam ;


        public MIOEncoder()
        {
            throw new NotImplementedException();
        }

        ~MIOEncoder()
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

        public virtual void EncodeSound(ERISAEncodeContext context, MIO_DATA_HEADER datahdr, byte[] ptrWaveBuf)
        {
            throw new NotImplementedException();
        }

        public void SetCompressionParameter(PARAMETER parameter)
        {
            throw new NotImplementedException();
        }

        protected void EncodeSoundPCM8(ERISAEncodeContext context,MIO_DATA_HEADER datahdr,byte[] ptrWaveBuf)
        {
            throw new NotImplementedException();
        }

        protected void EncodeSoundPCM16(ERISAEncodeContext context,MIO_DATA_HEADER datahdr,byte[] ptrWaveBuf)
        {
            throw new NotImplementedException();
        }

        protected void InitalizeWithDegree(uint nSubbandDegree)
        {
            throw new NotImplementedException();
        }

        protected double EvaluateVolume(float[] ptrWave,int nCount)
        {
            throw new NotImplementedException();
        }

        protected int GetDivisionCode(float[] ptrSamples)
        {
            throw new NotImplementedException();
        }

        protected void EncodeSoundDCT(ERISAEncodeContext context, MIO_DATA_HEADER datahdr, byte[] ptrWaveBuf)
        {
            throw new NotImplementedException();
        }

        protected void PerformLOT(ERISAEncodeContext context, float[] ptrSamples, float rPowerScale)
        {
            throw new NotImplementedException();
        }

        protected void EncodeInternalBlock(ERISAEncodeContext context,float[] ptrSamples,float rPowerScale)
        {
            throw new NotImplementedException();
        }

        protected void EncodeLeadBlock(ERISAEncodeContext context,float[] ptrSamples, float rPowerScale)
        {
            throw new NotImplementedException();
        }

        protected void EncodePostBlock(ERISAEncodeContext context,float rPowerScale)
        {
            throw new NotImplementedException();
        }

        void Quantumize(int[] ptrQuantumized, float[] ptrSource,int nDegreeNum,float rPowerScale,uint[] ptrWeightCode,int[] ptrCoefficient)
        {
            throw new NotImplementedException();
        }

        protected void EncodeSoundDCT_MSS(ERISAEncodeContext context,MIO_DATA_HEADER datahdr,byte[] ptrWaveBuf)
        {
            throw new NotImplementedException();
        }

        protected int GetRevolveCode(float[] ptrBuf1, float[] ptrBuf2)
        {
            throw new NotImplementedException();
        }

        protected void PerformLOT_MSS(float[] ptrDst, float[] ptrLapBuf, float[] ptrSrc)
        {
            throw new NotImplementedException();
        }

        void EncodeInternalBlock_MSS(ERISAEncodeContext context, float[] ptrSrc1, float[] ptrSrc2, float rPowerScale)
        {
            throw new NotImplementedException();
        }

        void EncodeLeadBlock_MSS(ERISAEncodeContext context, float[] ptrSrc1, float[] ptrSrc2, float rPowerScale)
        {
            throw new NotImplementedException();
        }

        void EncodePostBlock_MSS(ERISAEncodeContext context, float[] ptrSrc1, float[] ptrSrc2, float rPowerScale)
        {
            throw new NotImplementedException();
        }

        void Quantumize_MSS(int[] ptrQuantumized, float[] ptrSource,int nDegreeNum,float rPowerScale,uint[] ptrWeightCode,int[] ptrCoefficient)
        {
            throw new NotImplementedException();
        }

    }
}
