using System;
using System.Drawing;
using ERIShArp.Image;

namespace ERIShArp.Play
{
    public class ESLAnimationOutputInterface
    {
        public virtual WAVEFORMATEX GetWaveFormat()
        {
            return new WAVEFORMATEX();
        }

        public virtual void BeginStream()
        {
        }

        public virtual void WritePaletteTable(uint[] paltbl, uint nLength)
        {
        }

        public virtual void WriteWaveData(byte[] ptrWaveBuf, uint dwSampleCount)
        {
        }

        public virtual void WriteImageData(EGL_IMAGE_INFO infImage)
        {
        }

        public virtual void EndStream(uint dwTotaltime)
        {
        }
    }

    public struct WAVEFORMATEX
    {
        ushort wFormatTag, nChannels;
        uint nSamplesPerSec, nAvgBytesPerSec;
        ushort nBlockAlign, wBitsPerSample, cbSize;
    }
}
