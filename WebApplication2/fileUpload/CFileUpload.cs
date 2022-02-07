using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebApplication2.fileUpload
{

    public class _BASEfileUpload
    {
        public  string _id;
        public int _status;
        public string _origname;
        public bool _isComlite;
    }

    public class UploadInfoFile :_BASEfileUpload{
        public long _size;
        public string _realName;
  
        public UploadInfoFile(string id, string origname="", long size=0 , string realName=""){
            _size = size;
            base._origname=origname;
            base._isComlite = false;
            base._id = Guid.NewGuid().ToString();
            base._status = 0;
            base._id = id;
            _realName = realName;
        }

    }
    public class UploadInfoFolder : _BASEfileUpload
    {
        public List<UploadInfoFile> _files;
        public long _blockId;
        public string _folderPath;

        public string addFile(string origFileName, long fileSize, string folderPath)
        {
                var id= Guid.NewGuid().ToString();
                var fileToSave = Path.Combine(folderPath, origFileName);


                if (File.Exists(fileToSave))
                {
                    fileToSave = System.IO.Path.GetDirectoryName(fileToSave) + "\\" + System.IO.Path.GetFileNameWithoutExtension(fileToSave) + "_" + WebApplication2.handlers.utils.ToUnixTimestamp(DateTime.Now) + System.IO.Path.GetExtension(fileToSave);
                }

                using (var fs = new FileStream(fileToSave, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    fs.SetLength(fileSize);
                }

                lock (_files)
                {
                    _files.Add(new UploadInfoFile(id, origFileName, fileSize, System.IO.Path.GetFileName(fileToSave)));
                }

                return id.ToString();
            }
        public  UploadInfoFolder(string id, long blockId,  string origname = "" )
        {
          
            base._origname = origname;
            base._isComlite = false;
            base._id = Guid.NewGuid().ToString();
            base._status = 0;
            base._id = id;
            _blockId = blockId;
            _files = new List<UploadInfoFile>();

            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                {
                   
                        var folderPath = dc.vWeb_Settings.Where(d => d.Key=="FolderToUpload").First().value;
                        folderPath = Path.Combine(folderPath, DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM_yyyy"), DateTime.Now.ToString("dd_MM_yyyy"), _id.ToString());


                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);
                        _folderPath = folderPath;
 
            }

           
        }

    }


    public class UploadInfo
    {
        private List<UploadInfoFolder> _folder;
        public UploadInfo()
        {
            _folder = new List<UploadInfoFolder>(); 
        }
        public string addFolder( long blockId, string foldername)
        {
            var folderId = foldername;// Guid.NewGuid();
            lock (_folder)
            {
                try
                {
                    _folder.Add(new UploadInfoFolder(folderId, blockId));
                }
                catch (Exception ex)
                {
                    return "-1";
                }
            }
            return folderId.ToString();
        }
        public string addFile(string folderId, string origFileName, long fileSize)
        {
            lock (_folder)
            {
                var folder = _folder.Single(f => f._id.ToString() == folderId);
                return folder.addFile(origFileName, fileSize, folder._folderPath);
            }
        }
        public string getFilePath(string fileId)
        {
            string ret = "";
            lock (_folder)
            {
                _folder.ForEach(fld => {
                    fld._files.ForEach(f =>
                    {
                        if(f._id.ToString()==fileId)
                        {
                            ret= System.IO.Path.Combine(fld._folderPath, f._realName);
                        }
                    });

                });
              
            }
            return ret;
        }
        public string getFileOriginal(string fileId)
        {
            string ret = "";
            lock (_folder)
            {
                _folder.ForEach(fld =>
                {
                    fld._files.ForEach(f =>
                    {
                        if (f._id.ToString() == fileId)
                        {
                            ret =  f._realName;
                        }
                    });

                });

            }
            return ret;
        }
        public long getBlockId(string fileId)
        {
            long ret = 0;
            lock (_folder)
            {
                _folder.ForEach(fld =>
                {
                    fld._files.ForEach(f =>
                    {
                        if (f._id.ToString() == fileId)
                        {
                            ret = fld._blockId;
                        }
                    });

                });

            }
            return ret;
        }
        public void fileComplited(string fileId)
        {
            long ret = 0;
            lock (_folder)
            {
                _folder.ForEach(fld =>
                {
                    fld._files.ForEach(f =>
                    {
                        if (f._id.ToString() == fileId)
                        {
                            fld._files.Remove(f);
                        }
                    });
                    if(fld._files.Count==0)
                    {
                        _folder.Remove(fld);
                        /////// FOLDER COMPLITE EVENT !!!
                    }
                });

            }
            return;
        }
    }
}