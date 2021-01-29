using System.Linq;
using Bones.Scripts.Features.Audio.Music;
using UnityEditor;
using UnityEngine;

namespace Bones.Scripts.Features.Audio.Core.Editor
{
    [CustomEditor(typeof(ClipsCollection))]
    public class MusicTrackEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var asset = target as ClipsCollection;
            if (asset == null || asset.pairs == null)
            {
                Debug.LogError(asset);
                Debug.LogError(asset?.pairs);
                return;
            }

            for (var i = 0; i < asset.pairs.Count; i++)
            {
                var pair = asset.pairs[i];
                if (pair.clip && string.IsNullOrEmpty(pair.id)) pair.id = pair.clip.name;


                EditorGUILayout.BeginHorizontal();
                pair.clip = (AudioClip) EditorGUILayout.ObjectField(pair.clip, typeof(AudioClip), false);
                EditorGUILayout.Space(5);
                GUILayout.Label("id:");
                pair.id = EditorGUILayout.TextArea(pair.id);
                asset.pairs[i] = pair;
                EditorGUILayout.Space(5);
                if (GUILayout.Button("X", GUILayout.Height(30), GUILayout.Width(30)))
                {
                    asset.pairs.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                    return; // to avoid weird errors
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(5);
            }


            var clips = Selection.objects.OfType<AudioClip>().ToList();

            if (clips.Any())
                if (GUILayout.Button("Add Clips"))
                {
                    var pairs = clips.Select(clip => new PairTrackID
                    {
                        clip = clip,
                        id = clip.name
                    });
                    asset.pairs.AddRange(pairs);
                    AssetDatabase.SaveAssets();
                }
            //todo : si hay ids repetidos imprimir un error en consola y en el editor
            // usar UnityEngine.Debug.LogError() para imprimir errores en la consola
            // usar GUILayout.Label() para imprimir errores en el editor (PD: googlear sobre GUIStyle para poder imprimirlos en rojo)

            if (GUILayout.Button("Save")) AssetDatabase.SaveAssets();
        }
    }
}