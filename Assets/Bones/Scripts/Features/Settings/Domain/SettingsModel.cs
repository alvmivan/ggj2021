namespace Bones.Scripts.Features.Settings.Domain
{
    public struct SettingsModel
    {
        public VideoSettingsModel VideoSettings;
        public AudioSettingsModel AudioSettings;
        public GameSettingsModel GameSettings;
    }

    public struct GameSettingsModel
    {
        public DifficultyLevel Difficulty;
    }

    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Hard
    }

    public struct AudioSettingsModel
    {
        public float MasterVol;
        public float MusicVol;
        public float SFXVol;
    }

    public enum VideoQuality
    {
        Low,
        Mid,
        High
    }

    public struct VideoSettingsModel
    {
        public VideoQuality Quality;
        public bool FullScreen;
    }
}