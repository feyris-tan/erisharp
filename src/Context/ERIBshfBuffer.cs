using System;

namespace ERIShArp.Context
{
    public class ERIBshfBuffer
    {
        public string m_strPassword;
        public uint m_dwPassOffset;
        /// <summary>
        /// 32 bytes
        /// </summary>
        public byte[] m_bufBSHF;
        /// <summary>
        /// 32 bytes
        /// </summary>
        public byte[] m_srcBSHF;
        /// <summary>
        /// 32 bytes
        /// </summary>
        public byte[] m_maskBSHF;

        public ERIBshfBuffer()
        {
            m_dwPassOffset = 0;
        }

        ~ERIBshfBuffer()
        {
        }

        public void EncodeBuffer()
        {
            throw new NotImplementedException();
        }

        public void DecodeBuffer()
        {
            int i;
            int iPos = (int)m_dwPassOffset++;
            int nPassLen = m_strPassword.Length;
            byte[] pszPass = System.Text.Encoding.ASCII.GetBytes(m_strPassword);
            if ((int)m_dwPassOffset >= nPassLen)
            {
                m_dwPassOffset = 0;
            }
            for (i = 0; i < 32; i++)
            {
                m_bufBSHF[i] = 0;
                m_maskBSHF[i] = 0;
            }

            int iBit = 0;
            for (i = 0; i < 256; i++)
            {
                iBit = (iBit + pszPass[iPos++]) & 0xFF;
                if (iPos >= nPassLen)
                {
                    iPos = 0;
                }
                int iOffset = (iBit >> 3);
                int iMask = (0x80 >> (iBit & 0x07));
                while (m_maskBSHF[iOffset] == 0xFF)
                {
                    iBit = (iBit + 8) & 0xFF;
                    iOffset = (iBit >> 3);
                }
                while ((m_maskBSHF[iOffset] & iMask) != 0)
                {
                    iBit++;
                    iMask >>= 1;
                    if (iMask == 0)
                    {
                        iBit = (iBit + 8) & 0xFF;
                        iOffset = (iBit >> 3);
                        iMask = 0x80;
                    }
                }
                ESLAssert(iMask != 0);
                m_maskBSHF[iOffset] |= (byte)iMask;
                if ((m_srcBSHF[(i >> 3)] & (0x80 >> (i & 0x07))) != 0)
                {
                    m_bufBSHF[iOffset] |= (byte)iMask;
                }
            }
        }

        private void ESLAssert(bool condition)
        {
            if (!condition)
                throw new Exception();
        }
    }
}
