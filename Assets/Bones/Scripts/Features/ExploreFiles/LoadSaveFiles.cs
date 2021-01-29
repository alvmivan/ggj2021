using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Bones.Scripts.Features.ExploreFiles
{
    
    public class LoadSaveFiles
    {

        public bool ValidateDirectory(string path, out DirectoryInfo directoryInfo)
        {
            directoryInfo = new DirectoryInfo("");
            if (Directory.Exists(path))
            {
                directoryInfo = new DirectoryInfo(path);
                return true;
            }

            return false;
        }
        
        public bool Save(FileInfo fileInfo, string content, bool append)
        {
            if (!File.Exists(fileInfo.FilePath)) return false;
            File.WriteAllText(fileInfo.FilePath, content);
            return true;
        }

        public string Load(FileInfo fileInfo)
        {
            if (!File.Exists(fileInfo.FilePath))
            {
                return "";
            }
            return File.ReadAllText(fileInfo.FilePath);
        }
        
        public FileInfo Create(DirectoryInfo directoryInfo, string fileName)
        {
            var fullPath = Path.Combine(directoryInfo.DirectoryPath, fileName);
            var fileInfo = new FileInfo(fullPath);
            if (!File.Exists(fileInfo.FilePath))
            {
                File.Create(fileInfo.FilePath).Close();
            }
            return fileInfo;
        }
        
        public DirectoryInfo Root()
        {
            //return new DirectoryInfo("C:/");
            return new DirectoryInfo(Application.persistentDataPath);
        }
        
        public DirectoryInfo GetParentDirectory(DirectoryInfo directory)
        {
            return new DirectoryInfo(Directory.GetParent(directory.DirectoryPath).FullName);
        }
        
        public List<FileInfo> GetFiles(DirectoryInfo directory, string end, string search)
        {
            var path = directory.DirectoryPath;
            return Directory
                .EnumerateFiles(path)
                .Where(s => s.EndsWith(end))
                .Select(s => new FileInfo(s))
                .Where(fi=>GetFileName(fi).ToLower().Contains(search.ToLower()))
                .ToList();
        }
        
        public List<DirectoryInfo> GetChildrenDirectories(DirectoryInfo directory, string search)
        {
            var path = directory.DirectoryPath;
            return Directory
                .EnumerateDirectories(path)
                .Select(dirPath => new DirectoryInfo(dirPath))
                .Where(di=>GetDirectoryName(di).ToLower().Contains(search.ToLower()))
                .ToList();
        }

        public string GetFileName(FileInfo fileInfo)
        {
            return File.Exists(fileInfo.FilePath) ? Path.GetFileName(fileInfo.FilePath) : string.Empty;
        }

        public string GetDirectoryName(DirectoryInfo directory)
        {
            return Directory.Exists(directory.DirectoryPath) ? Path.GetFileName(directory.DirectoryPath) : string.Empty;
        }
    }


    public struct FileInfo
    {
        public string FilePath;

        public FileInfo(string filePath)
        {
            FilePath = filePath;
        }
    }
    
    public struct DirectoryInfo
    {
        public string DirectoryPath;

        public DirectoryInfo(string directoryPath)
        {
            DirectoryPath = directoryPath;
        }
    }
}