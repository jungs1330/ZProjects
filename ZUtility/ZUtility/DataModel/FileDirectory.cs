using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;

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
            this.DateTaken = this.LastWriteTime;

            try
            {
                using (FileStream fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
                using (Image myImage = Image.FromStream(fs, false, false))
                {
                    PropertyItem propItem = null;
                    try
                    {
                        propItem = myImage.GetPropertyItem(36867);
                    }
                    catch { }
                    if (propItem != null)
                    {
                        string dateTaken = new Regex(":").Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                        this.DateTaken = DateTime.Parse(dateTaken);
                    }
                }
            }
            catch (Exception ex) { }
        }


        public string Name { get; set; }
        public string NewName { get; set; }
        public string DirectoryName { get; set; }
        public string Extension { get; set; }
        public bool IsReadOnly { get; set; }
        public long Length { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public DateTime DateTaken { get; set; }
    }
}
