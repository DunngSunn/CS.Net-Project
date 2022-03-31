using System;
using System.Collections.Generic;
using DunnGSunn;
using UnityEngine;

namespace Manager
{
    // ------------------------------- //
    [Serializable]
    public enum AudioGroupType
    {
        Music,
        SFX
    }
    
    // ------------------------------- //
    [Serializable]
    public class Sound
    {
        // ======================== //
        #region Fields

        [Header("Audio group")]
        [SerializeField] private AudioGroupType audioGroup = AudioGroupType.Music;
        
        [Header("Clip")]
        [SerializeField] private string clipName;
        [SerializeField] private AudioClip clip;
        
        [Header("Clip fields")]
        [Range(0f, 1f), SerializeField] private float volume = 1f;
        [Range(0f, 3f), SerializeField] private float pitch = 1f;
        [SerializeField] private bool loop;
        [SerializeField] private bool playOnAwake;
        
        private AudioSource _audioSource;

        public string ClipName
        {
            get => clipName;
            set => clipName = value;
        }

        public AudioGroupType AudioGroup
        {
            get => audioGroup;
            set => audioGroup = value;
        }
        
        #endregion

        // ======================== //
        #region Sound control functions

        // Khởi tạo các thuộc tính cơ bản của âm thanh
        public void InitializeSource(AudioSource audioSource)
        {
            _audioSource = audioSource;
            _audioSource.clip = clip;
            _audioSource.pitch = pitch;
            _audioSource.volume = volume;
            _audioSource.playOnAwake = playOnAwake;
            _audioSource.loop = loop;
        }

        public void Play()
        {
            if (_audioSource.isPlaying) return;
            _audioSource.Play();
        }

        public void SetVolume(float value)
        {
            _audioSource.volume = value;
        }

        #endregion
    }
    
    public class AudioManager : SunMonoSingleton<AudioManager>
    {
        // ======================== //
        #region Fields

        // Biến giá trị của âm thanh loại Music
        public static float MusicValue
        {
            get => PlayerPrefs.GetFloat("MusicValue", 1f);
            set => PlayerPrefs.SetFloat("MusicValue", value);
        }

        // Biến giá trị của âm thanh loại SFX
        public static float SFXValue
        {
            get => PlayerPrefs.GetFloat("SFXValue", 1f);
            set => PlayerPrefs.SetFloat("SFXValue", value);
        }

        [Header("Sounds")]
        [SerializeField] private List<Sound> sounds;

        #endregion

        // ======================== //
        #region Unity callback functions

        protected override void LoadInAwake()
        {
            // Nghe các sự kiện giá trị của âm thanh loại Music và SFX được thay đổi
            SunEventManager.StartListening(EventID.MusicValueChanged, OnMusicValueChanged);
            SunEventManager.StartListening(EventID.SFXValueChanged, OnSFXValueChanged);
        }
        
        private void Start()
        {
            // Khởi tạo tất cả âm thanh có trong game
            if (sounds.Count > 0)
            {
                foreach (var sound in sounds)
                {
                    var go = new GameObject($"Sound_{sound.ClipName}");
                    go.transform.SetParent(transform);
                    sound.InitializeSource(go.AddComponent<AudioSource>());
                }   
            }
        }

        private void OnDestroy()
        {
            // Kết thúc nghe các sự kiện giá trị của âm thanh loại Music và SFX được thay đổi
            SunEventManager.StopListening(EventID.MusicValueChanged, OnMusicValueChanged);
            SunEventManager.StopListening(EventID.SFXValueChanged, OnSFXValueChanged);
        }

        #endregion

        // ======================== //
        #region Sound control functions
        
        public void PlaySound(string nameClip)
        {
            // Phát âm thanh có tên được truyền vào
            foreach (var sound in sounds)
            {
                if (sound.ClipName == nameClip)
                {
                    sound.Play();
                    Debug.Log($"{sound.ClipName} play.");
                    return;
                }
            }
        }

        #endregion

        // ======================== //
        #region Sun events

        // Sự kiện giá trị âm thanh loại SFX được thay đổi
        private void OnSFXValueChanged()
        {
            // Lấy giá trị đã được thay đổi
            var value = (float)SunEventManager.GetSender(EventID.SFXValueChanged);
            
            // Kiểm tra tất cả âm thanh đang có, nếu là loại SFX thì sẽ set lại volume 
            foreach (var sound in sounds)
            {
                if (sound.AudioGroup == AudioGroupType.SFX)
                {
                    sound.SetVolume(value);
                    Debug.Log($"{sound.ClipName} volume changed.");
                }
            }
        }

        // Sự kiện giá trị âm thanh loại Music được thay đổi
        private void OnMusicValueChanged()
        {
            // Lấy giá trị đã được thay đổi
            var value = (float)SunEventManager.GetSender(EventID.MusicValueChanged);
            
            // Kiểm tra tất cả âm thanh đang có, nếu là loại SFX thì sẽ set lại volume 
            foreach (var sound in sounds)
            {
                if (sound.AudioGroup == AudioGroupType.Music)
                {
                    sound.SetVolume(value);
                    Debug.Log($"{sound.ClipName} volume changed.");
                }
            }
        }

        #endregion
    }
}