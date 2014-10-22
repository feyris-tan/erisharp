﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERIShArp
{
    public static class Constants
    {
        const uint EFH_STANDARD_VERSION = 0x00020100;
        const uint	EFH_ENHANCED_VERSION =	0x00020200;

        const uint	EFH_CONTAIN_IMAGE		=0x00000001;
        const uint	EFH_CONTAIN_ALPHA		=0x00000002;
        const uint	EFH_CONTAIN_PALETTE		=0x00000010;
        const uint	EFH_CONTAIN_WAVE		=0x00000100;
        const uint	EFH_CONTAIN_SEQUENCE	=0x00000200;

        const uint	ERI_RGB_IMAGE			=0x00000001;
        const uint	ERI_RGBA_IMAGE			=0x04000001;
        const uint	ERI_GRAY_IMAGE			=0x00000002;
        const uint	ERI_TYPE_MASK			=0x00FFFFFF;
        const uint	ERI_WITH_PALETTE		=0x01000000;
        const uint	ERI_USE_CLIPPING		=0x02000000;
        const uint	ERI_WITH_ALPHA			=0x04000000;
        const uint	ERI_SIDE_BY_SIDE		=0x10000000;

        const uint	CVTYPE_LOSSLESS_ERI		=0x03020000;
        const uint	CVTYPE_DCT_ERI			=0x00000001;
        const uint	CVTYPE_LOT_ERI			=0x00000005;
        const uint	CVTYPE_LOT_ERI_MSS		=0x00000105;

        const uint	ERI_ARITHMETIC_CODE		=32;
        const uint	ERI_RUNLENGTH_GAMMA		=0xFFFFFFFF;
        const uint	ERI_RUNLENGTH_HUFFMAN	=0xFFFFFFFC;
        const uint	ERISA_NEMESIS_CODE		=0xFFFFFFF0;

        const uint	ERISF_YUV_4_4_4			=0x00040404;
        const uint	ERISF_YUV_4_2_2			=0x00040202;
        const uint	ERISF_YUV_4_1_1			=0x00040101;

        const byte MIO_LEAD_BLOCK	=0x01;

        const uint ERINA_CODE_FLAG			= 0x80000000U;
        const uint ERINA_HUFFMAN_ESCAPE	        = 0x7FFFFFFF;
        const uint ERINA_HUFFMAN_NULL		= 0x8000U;
        const uint ERINA_HUFFMAN_MAX		= 0x4000;
        const uint ERINA_HUFFMAN_ROOT = 0x200;

        const int ERISA_TOTAL_LIMIT	= 0x2000;	
        const int ERISA_SYMBOL_SORTS	= 0x101;	
        const int ERISA_SUB_SORT_MAX	= 0x80;
        const int ERISA_PROB_SLOT_MAX	= 0x800;	
        const int ERISA_ESC_CODE = (-1);

        const uint NEMESIS_BUF_SIZE = 0x10000;
	    const uint NEMESIS_BUF_MASK	= 0xFFFF;
	    const uint NEMESIS_INDEX_LIMIT	= 0x100;
	    const uint NEMESIS_INDEX_MASK	= 0xFF;
    }
}
