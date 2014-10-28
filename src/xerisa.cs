using System;


namespace ERIShArp.X
{
    public struct ERI_FILE_HEADER
    {
        public uint dwVersion;
        public uint dwContainedFlag;
        public uint dwKeyFrameCount;
        public uint dwFrameCount;
        public uint dwAllFrameTime;
    }

    public class ERI_INFO_HEADER
    {
        public ERI_INFO_HEADER()
        {
            dwQuantumizedBits = new int[2];
            dwAllottedBits = new uint[2];
        }
	    public uint	dwVersion ;
        public uint fdwTransformation;
        public uint dwArchitecture;
        public uint fdwFormatType;
        public int nImageWidth;
        public int nImageHeight;
        public uint dwBitsPerPixel;
        public uint dwClippedPixel;
        public uint dwSamplingFlags;
        /// <summary>
        /// Two Integers
        /// </summary>
        public int[] dwQuantumizedBits;
        /// <summary>
        /// Two unsigned intergers.
        /// </summary>
        public uint[] dwAllottedBits;
        public uint dwBlockingDegree;
        public uint dwLappedBlock;
        public uint dwFrameTransform;
        public uint dwFrameDegree;
    }

    public struct MIO_INFO_HEADER
    {
        public uint dwVersion;
        public uint fdwTransformation;
        public uint dwArchitecture;
        public uint dwChannelCount;
        public uint dwSamplesPerSec;
        public uint dwBlocksetCount;
        public uint dwSubbandDegree;
        public uint dwAllSampleCount;
        public uint dwLappedDegree;
        public uint dwBitsPerSample;
    }

    public struct MIO_DATA_HEADER
    {
        byte bytVersion;
        byte bytFlags;
        byte bytReserved1;
        byte bytReserved2;
        uint dwSampleCount;
    } ;
}
