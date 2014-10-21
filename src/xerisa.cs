using System;


namespace ERIShArp.X
{
    public struct ERI_FILE_HEADER
    {
        uint dwVersion;
        uint dwContainedFlag;
        uint dwKeyFrameCount;
        uint dwFrameCount;
        uint dwAllFrameTime;
    }

    public struct ERI_INFO_HEADER
    {
	    uint	dwVersion ;
	    uint	fdwTransformation ;
	    uint	dwArchitecture ;
	    uint	fdwFormatType ;
	    int	nImageWidth ;
	    int	nImageHeight ;
	    uint	dwBitsPerPixel ;
	    uint	dwClippedPixel ;
	    uint	dwSamplingFlags ;
        /// <summary>
        /// Two Integers
        /// </summary>
	    int[]	dwQuantumizedBits;
        /// <summary>
        /// Two unsigned intergers.
        /// </summary>
	    uint[]	dwAllottedBits;
	    uint	dwBlockingDegree ;
	    uint	dwLappedBlock ;
	    uint	dwFrameTransform ;
	    uint	dwFrameDegree ;
    }

    public struct MIO_INFO_HEADER
    {
        uint dwVersion;
        uint fdwTransformation;
        uint dwArchitecture;
        uint dwChannelCount;
        uint dwSamplesPerSec;
        uint dwBlocksetCount;
        uint dwSubbandDegree;
        uint dwAllSampleCount;
        uint dwLappedDegree;
        uint dwBitsPerSample;
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
