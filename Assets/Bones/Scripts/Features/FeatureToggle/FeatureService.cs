using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Bones.Scripts.Features.FeatureToggle
{
    [UsedImplicitly]
    public class LocalFeatureService : FeatureService
    {
        protected const string FeaturesFileName = "features";

        protected readonly Dictionary<string, bool> dict = new Dictionary<string, bool>();

        protected TextAsset textAsset;

        public LocalFeatureService()
        {
            LoadFile();
        }

        public bool this[string featureName] => ReadValue(featureName);

        private void LoadFile()
        {
            textAsset = Resources.Load<TextAsset>(FeaturesFileName);
            var text = textAsset.text;
            var featuresDTO = JsonUtility.FromJson<FeaturesDTO>(text);

            foreach (var featureDTO in featuresDTO.features)
            {
                dict[featureDTO.featureName] = featureDTO.isActive;
            }
        }

        protected bool ReadValue(string featureName)
        {
            return dict.TryGetValue(featureName, out var isOn) && isOn;
        }
    }

    [Serializable]
    public struct FeaturesDTO
    {
        public List<FeaturePairDTO> features;
    }

    [Serializable]
    public struct FeaturePairDTO
    {
        public string featureName;
        public bool isActive;
    }

    public interface FeatureService
    {
        bool this[string featureName] { get; }
    }
}