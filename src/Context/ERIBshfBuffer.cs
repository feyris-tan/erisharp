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
            throw new NotImplementedException();
        }

        ~ERIBshfBuffer()
        {
            throw new NotImplementedException();
        }

        public void EncodeBuffer()
        {
            throw new NotImplementedException();
        }

        public void DecodeBuffer()
        {
            throw new NotImplementedException();
        }
    }
}
