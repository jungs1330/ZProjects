using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ZUtility.DataModel
{
    public class FileDirectory
    {
        public FileDirectory(FileInfo fileInfo)
        {
            this.CreationTime = fileInfo.CreationTime;
            this.DirectoryName = fileInfo.DirectoryName;
            this.Extension = fileInfo.Extension;
            this.IsReadOnly = fileInfo.IsReadOnly;
            this.LastWriteTime = fileInfo.LastWriteTime;
            this.Length = fileInfo.Length;
            this.Name = fileInfo.Name;
        }

        
        public string Name { get; set; }
        public string NewName { get; set; }
        public string DirectoryName { get; set; }
        public string Extension { get; set; }
        public bool IsReadOnly { get; set; }
        public long Length { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
    }
}
