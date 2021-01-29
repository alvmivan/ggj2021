using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Bones.Scripts.Features.FeatureToggle.Editor
{
    public class FeatureTogglesEditor : EditorWindow
    {
        private bool addingFeature;
        private string newFeatureName = "";

        private EditorFeatureService service;

        private void Awake()
        {
            service = new EditorFeatureService();
        }

        private void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                GUILayout.Label("Only touch that when game is not running :) ");
                return;
            }

            if (service == null) service = new EditorFeatureService();

            GUILayout.Space(10);
            GUILayout.Label("Features Toggles");
            GUILayout.Space(10);

            var dict = service.GetDict();
            var features = dict.Keys.ToList();
            var changes = false;
            foreach (var feature in features)
            {
                GUILayout.BeginHorizontal();
                var newValue = GUILayout.Toggle(dict[feature], feature);
                if (newValue != dict[feature])
                {
                    dict[feature] = newValue;
                    changes = true;
                }

                if (GUILayout.Button("X", GUILayout.Width(30)))
                {
                    dict.Remove(feature);
                    changes = true;
                }

                GUILayout.EndHorizontal();
            }


            GUILayout.Space(10);

            if (GUILayout.Button("Add Feature", GUILayout.Width(150)))
            {
                if (addingFeature) CreateFeature(dict);
                addingFeature = !addingFeature;
                changes = true;
            }

            GUILayout.Space(10);
            if (addingFeature) newFeatureName = GUILayout.TextField(newFeatureName);

            if (changes) service.Save(dict);
        }

        [MenuItem(Const.GameNameMenu+"Features")]
        public static void ShowWindow()
        {
            // Opens the window, otherwise focuses it if it’s already open.
            var window = GetWindow<FeatureTogglesEditor>();

            // Adds a title to the window.
            window.titleContent = new GUIContent("Features");

            // Sets a minimum size to the window.
            window.minSize = new Vector2(250, 50);
        }

        private void CreateFeature(IDictionary<string, bool> dict)
        {
            dict[newFeatureName] = false;
        }
    }
}