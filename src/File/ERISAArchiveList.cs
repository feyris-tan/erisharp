using System;
using System.Collections.Generic;
using System.IO;

namespace ERIShArp.File
{
    public class ERISAArchiveList
    {
	    public ERISAArchiveList()
        {
            m_pCurDir = m_dirRoot;
        }
        ~ERISAArchiveList()
        {
            DeleteContents();
        }

	    public class	EFileEntry
	    {
		    public string			m_strFileName ;
		    public uint			m_dwAttribute ;
		    public uint			m_dwEncodeType ;
		    public string			m_strPassword ;
		    public EDirectory 	m_pSubDir ;
		    EFileEntry()
            {
                m_dwAttribute = 0;
                m_dwEncodeType = 0;
                m_pSubDir = null;
            }

		    ~EFileEntry()
			{
			}
	    }
	    public class EDirectory	: List<EFileEntry>
	    {
		    public EDirectory m_pParentDir ;
		    public EDirectory()
            {
                m_pParentDir = null;
            }
	    }

	    protected EDirectory		m_dirRoot ;
	    protected EDirectory 	m_pCurDir ;

	    public void LoadFileList(Stream file )
        {
            throw new NotImplementedException();
        }
	    public void SaveFileList(Stream file )
        {
            throw new NotImplementedException();
        }
	    public void DeleteContents()
        {
            throw new NotImplementedException();
        }


	    protected static void ReadListOnDirectory( EDirectory flist, byte[] desc )
        {
            throw new NotImplementedException();
        }
        
	    protected static void WriteListOnDirectory( byte[]  desc, EDirectory flist )
        {
            throw new NotImplementedException();
        }

	    public void AddFileEntry( string pszFilePath,uint dwEncodeType = (uint)ERISAArchive.EncodeType.etRaw,string pszPassword = null )
        {
            throw new NotImplementedException();
        }
	    public void AddSubDirectory( string pszDirName )
        {
            throw new NotImplementedException();
        }
	    public EDirectory GetRootFileEntries()
        {
            throw new NotImplementedException();
        }
	    public EDirectory GetCurrentFileEntries()
        {
            throw new NotImplementedException();
        }
	    public void DescendDirectory( string pszDirName )
        {
            throw new NotImplementedException();
        }
	    public void AscendDirectory()
        {
            throw new NotImplementedException();
        }
	    public string GetCurrentDirectoryPath()
        {
            throw new NotImplementedException();
        }
	    public void MoveCurrentDirectory( string pszDirPath )
        {
            throw new NotImplementedException();
        }

    }
}
