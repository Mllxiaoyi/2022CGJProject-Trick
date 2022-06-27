using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Game.Audio;
using Sirenix.OdinInspector;

namespace Game
{

    public class AudioManager : MonoSingleton<AudioManager>
    {
        [Header("音量设置")]
        [Space]


        [ReadOnly]
        [SerializeField]
        [Range(0, 1)]
        [Header("主音量")]
        private float _audioVolume = 1;
        public float AudioVolume
        {
            get { return _audioVolume; }
            set
            {
                if (value < 0 || value > 1) { Debug.Log("格式有误"); return; }
                SetVolume(ref _audioVolume, value);
                OnVolumeChange("BGM", _audioVolume);
            }
        }


        [ReadOnly]
        [SerializeField]
        [Range(0, 1)]
        [Header("音效音量")]
        private float _soundEffectsVolume = 1;
        public float SoundEffectsVolume
        {
            get { return _soundEffectsVolume; }
            set
            {
                if (value < 0 || value > 1) { Debug.Log("格式有误"); return; }
                SetVolume(ref _soundEffectsVolume, value);
                OnVolumeChange("SFX", _soundEffectsVolume);
            }
        }


        public AudioMixer audioMixer;

        public AudioMixerGroup bgmMixer;

        public AudioMixerGroup sfxMixer;

        public AudioMixerGroup UI_sfxMixer;

        public AudioMixerGroup GamePlay_SfxMixer;

        [Header("池管理")]
        [SerializeField] private AudioPlayerPoolSO _audioSourcePool;

        [SerializeField] private int _poolMemberInitNum = 1;

        private AudioPlayer bgmPlayer;
        /// <summary>
        /// 存储所有已经创建好的AudioSource组件
        /// </summary>
        private Dictionary<AudioSO, AudioSource> dictAudios;

        protected override void Awake()
        {
            base.Awake();
            /*
            GameObject temp = new GameObject("bgmPlayer");
            temp.transform.SetParent(this.transform);
            bgmPlayer = temp.AddComponent<AudioSource>();
            */
            Init();
        }


        public void Start()
        {
            //_audioSourcePool.OnAudioFinishingPlay += 

        }



        #region Public Methods

        public void PlayAudio(string audioName)
        {

        }

        public void PlayAudioOnce(AudioClip clip, AudioCueType type)
        {
            if (clip == null) return;
            AudioPlayer audioPlayer = FindUsableAudioPlayer();
            audioPlayer.PlayAudioClip(clip, GetMixerGroup(type));
        }

        /// <summary>
        /// 从AudioSO拿取并播放一个音频
        /// </summary>
        /// <param name="audioSO"></param>
        /// <param name="isWait"></param>
        public void PlayAudio(AudioSO audioSO, bool isWait = false)
        {
            if (audioSO == null) return;
            AudioPlayer audioPlayer = FindUsableAudioPlayer();
            if (audioSO.audioType == AudioCueType.Bgm)
            {
                bgmPlayer = audioPlayer;
            }
            audioPlayer.PlayAudioClip(audioSO, GetMixerGroup(audioSO.audioType));
            //bgmPlayer.PlayOneShot(audioSO.audios[0].clip);
        }

        public void StopPlayAudio(AudioSO audioSO)
        {
            //To Do:
        }

        public void StopAllPlayingAudio()
        {
            _audioSourcePool.ClearPool();
        }
        #endregion


        #region Private Methods
        private void Init()
        {

            _audioSourcePool.SetPoolRoot(this.transform);

            _audioSourcePool.InitFillPool(_poolMemberInitNum, "AudioPlayer");
        }

        private void SetVolume(ref float setWhichVolume, float volume)
        {
            if (!(setWhichVolume == volume))
            {
                setWhichVolume = volume;
            }
        }


        private void OnVolumeChange(string audioChannel, float volume)
        {
            if (volume <= 0.01f)
            {
                audioMixer.SetFloat(audioChannel, -80);
                return;
            }
            audioMixer.SetFloat(audioChannel, Mathf.Lerp(-40, 0, Mathf.Sqrt(volume * 9) / 3));
        }

        private AudioPlayer FindUsableAudioPlayer()
        {
            return _audioSourcePool.Request();
        }

        private AudioMixerGroup GetMixerGroup(AudioCueType type)
        {
            switch (type)
            {
                case AudioCueType.Bgm:
                    return bgmMixer;
                case AudioCueType.Sfx:
                    return sfxMixer;
                case AudioCueType.UI_Sfx:
                    return UI_sfxMixer;
                case AudioCueType.GamePlay_Sfx:
                    return GamePlay_SfxMixer;
                default:
                    return bgmMixer;
            }
        }
        #endregion
    }

}