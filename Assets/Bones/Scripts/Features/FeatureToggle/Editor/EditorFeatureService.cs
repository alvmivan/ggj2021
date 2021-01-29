using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Bones.Scripts.Features.FeatureToggle.Editor
{
    public class EditorFeatureService : LocalFeatureService
    {
        public List<string> GetFeatures()
        {
            return dict.Keys.ToList();
        }

        public Dictionary<string, bool> GetDict()
        {
            return dict;
        }

        public void Save(Dictionary<string, bool> newDict)
        {
            var keys = GetFeatures();

            foreach (var feature in keys) dict[feature] = newDict[feature];

            var dto = new FeaturesDTO
            {
                features = dict
                    .Select(pair => new FeaturePairDTO {featureName = pair.Key, isActive = pair.Value})
                    .ToList()
            };

            var json = JsonUtility.ToJson(dto, true);

            var dataPath = Application.dataPath;
            dataPath = dataPath.Substring(0, dataPath.Length - "Assets/".Length);
            var path = dataPath + "/" + AssetDatabase.GetAssetPath(textAsset);

            File.WriteAllLines(path, new[] {json});

            Debug.Log(path);
        }
    }
}