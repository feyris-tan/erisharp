using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERIShArp.X;

namespace ERIShArp.Image
{
    /// <summary>
    /// This class is not present in the original ERISA Library, however, it is a struct from the Entis Graphic Library. 
    /// This class and all of its functions are, in this form, not present in the original ERISA Library.
    /// </summary>
    public class EGL_IMAGE_INFO
    {
        public uint dwInfoSize, fdwFormatType, ptrOffsetPixel;
        public Pointer ptrImageArray;
        public uint pPaletteEntries, dwPaletteCount, dwImageWidth, dwImageHeight, dwBitsPerPixel;
        public int dwBytesPerLine, dwSizeOfImage, dwClippedPixel;

        public EGL_IMAGE_INFO()
        {
        }

        public EGL_IMAGE_INFO(uint width, uint height, uint bpp)
        {
            dwImageWidth = width;
            dwImageHeight = height;
            dwBitsPerPixel = bpp;
            dwBytesPerLine = (int)(BytesPerPixel * dwImageWidth);
            dwSizeOfImage = (int)(dwBytesPerLine * dwImageHeight);
            ptrImageArray = new Pointer(new byte[dwSizeOfImage], 0);
        }

        public EGL_IMAGE_INFO(ERI_INFO_HEADER eri)
        {
            dwBitsPerPixel = eri.dwBitsPerPixel;
            dwImageWidth = (uint)eri.nImageWidth;
            dwImageHeight = (uint)eri.nImageHeight;
            dwBytesPerLine = (int)(BytesPerPixel * dwImageWidth);
            dwSizeOfImage = (int)(dwBytesPerLine * dwImageHeight);
            ptrImageArray = new Pointer(new byte[dwSizeOfImage], 0);
            dwClippedPixel = (int)eri.dwClippedPixel;
        }

        private int BytesPerPixel
        {
            get
            {
                return (int)(dwBitsPerPixel / 8);
            }
        }
    }
}
