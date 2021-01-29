using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bones.Scripts.Features.ExploreFiles
{
    public class ExplorerView : MonoBehaviour
    {
        private const int X = 0;
        private const int Y = 1;
        public Button goUpBUtton;

        public TextMeshProUGUI currentDirectoryLabel;
        public TextMeshProUGUI currentFileLabel;
        public TextMeshProUGUI currentFileContent;
        
        public DataButton directoryButtonPrefab;
        public DataButton fileButtonPrefab;

        public RectTransform directoriesContent;
        public RectTransform filesContent;

        public TMP_InputField searchBar;
        
        
        
        const string LastDirectoryStoredKey = "Bones.FileExplorer.LastDirectoryStored";
        LoadSaveFiles explorer = new LoadSaveFiles();
    
        public string extension = ".meta";

        private DirectoryInfo LastDirectoryStored()
        {
            var stored = PlayerPrefs.GetString(LastDirectoryStoredKey);
            if (string.IsNullOrEmpty(stored) || !explorer.ValidateDirectory(stored, out var dir))
            {
                return explorer.Root();
            }
            return dir;
        }

        private DirectoryInfo current;
        private void Start()
        {
            DisplayDirectory(LastDirectoryStored());
            goUpBUtton.onClick.AddListener(() =>
            {
                DisplayDirectory(explorer.GetParentDirectory(current));
            });
            searchBar.onValueChanged.AddListener(v=>DisplayDirectory(current));
        }

        private void DisplayDirectory(DirectoryInfo directory)
        {
            current = directory;
            StoreDirectory(directory);
            
            
            const float verticalPadding = 10;
            
            ClearChildren(directoriesContent.gameObject);
            SetSizeDelta(directoriesContent, verticalPadding, Y);
            var directoriesSpacing = directoriesContent.GetComponent<VerticalLayoutGroup>().spacing;
                
            
            ClearChildren(filesContent.gameObject);
            SetSizeDelta(filesContent, verticalPadding, Y);
            var filesSpacing = filesContent.GetComponent<VerticalLayoutGroup>().spacing;
            
            
            var directories = explorer.GetChildrenDirectories(directory, searchBar.text);
            Debug.Log(directories.Count+ " directories");
            foreach (var dirInfo in directories)
            {
                var directoryButton  = Instantiate(directoryButtonPrefab, directoriesContent);
                AddSizeDelta(directoriesContent, (directoriesSpacing + directoryButton.RectTransform.sizeDelta.y) * Vector2.up);
                directoryButton.Text = explorer.GetDirectoryName(dirInfo);
                directoryButton.OnCLick(() => DisplayDirectory(dirInfo));
            }
            var files = explorer.GetFiles(directory, extension,searchBar.text);
            Debug.Log(files.Count+" files");
            foreach (var fileInfo in files)
            {
                var fileButton  = Instantiate(fileButtonPrefab, filesContent);
                AddSizeDelta(directoriesContent, (filesSpacing + fileButton.RectTransform.sizeDelta.y) * Vector2.up);
                fileButton.GetComponentInChildren<TextMeshProUGUI>().text = explorer.GetFileName(fileInfo);
                fileButton.OnCLick(() => OpenFile(fileInfo));
            }

            currentDirectoryLabel.text = directory.DirectoryPath;
        }

        private void OpenFile(FileInfo fileInfo)
        {
            var content = explorer.Load(fileInfo);
            currentFileContent.text = content;
            currentFileLabel.text = explorer.GetFileName(fileInfo);
        }

        private void SetSizeDelta(RectTransform rectTransform, float value, int vectorComponent)
        {
            var sizeDelta = rectTransform.sizeDelta;
            sizeDelta[vectorComponent] = value;
            rectTransform.sizeDelta = sizeDelta;
        }
        private void AddSizeDelta(RectTransform rectTransform, Vector2 value)
        {
            rectTransform.sizeDelta += value;
        }

        private void ClearChildren(GameObject go)
        {
            var childCount = go.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                childrenQueue.Enqueue(go.transform.GetChild(i));
            }

            while (childrenQueue.Count > 0)
            {
                Destroy(childrenQueue.Dequeue().gameObject);
            }
        }

        readonly Queue<Transform> childrenQueue = new Queue<Transform>();

        private void StoreDirectory(DirectoryInfo directory)
        {
            PlayerPrefs.SetString(LastDirectoryStoredKey, directory.DirectoryPath);
            PlayerPrefs.Save();
        }
    }
}