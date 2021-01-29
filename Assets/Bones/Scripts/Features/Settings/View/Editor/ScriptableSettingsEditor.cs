using Bones.Scripts.Features.Settings.Services;
using UnityEditor;
using UnityEngine;

namespace Bones.Scripts.Features.Settings.View.Editor
{
    public class ScriptableSettingsEditor : EditorWindow
    {
        private readonly SettingsRepository repository = new LocalSettingsRepository();

        private void OnGUI()
        {
            var settings = repository.LoadSettings();
            GUILayout.BeginHorizontal();
            GUILayout.Label("AudioSettings.MusicVol : ");
            GUILayout.Label(settings.AudioSettings.MusicVol + "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("AudioSettings.SFXVol : ");
            GUILayout.Label(settings.AudioSettings.SFXVol + "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("AudioSettings.MasterVol : ");
            GUILayout.Label(settings.AudioSettings.MasterVol + "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("GameSettings.Difficulty : ");
            GUILayout.Label(settings.GameSettings.Difficulty + "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("VideoSettings.Quality : ");
            GUILayout.Label(settings.VideoSettings.Quality + "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("VideoSettings.FullScreen : ");
            GUILayout.Label(settings.VideoSettings.FullScreen + "");
            GUILayout.EndHorizontal();
        }

        [MenuItem(Const.GameNameMenu+"/Settings Repo Status")]
        private static void Init()
        {
            var window = (ScriptableSettingsEditor) GetWindow(typeof(ScriptableSettingsEditor));
            window.Show();
        }
    }
}