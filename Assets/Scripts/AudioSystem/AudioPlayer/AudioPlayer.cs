using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
namespace Game.Audio
{
    /// <summary>
    /// AudioManager框架内的音频播放器具有的组件
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;

        public event UnityAction<AudioPlayer> OnSoundFinishedPlaying;

        private void Awake()
        {
            _audioSource = this.GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;

        }

        public void PlayAudioClip(AudioClip audioClip, AudioMixerGroup mixerGroup, Vector3 position = default)
        {
            _audioSource.clip = audioClip;
            _audioSource.outputAudioMixerGroup = mixerGroup;
            _audioSource.time = 0f;
            _audioSource.Play();
            StartCoroutine(FinishPlaying(_audioSource.clip.length));
        }

        public void PlayAudioClip(AudioSO audioSO,AudioMixerGroup mixerGroup , Vector3 position = default)
        {
            if (audioSO.audioList.Count == 0 || audioSO.audioList[0] == null)
                return;

            int num=0;
            switch (audioSO.audioCuePlayMode)
            {
                case AudioCuePlayMode.Normal:
                    num = 0;
                    break;
                case AudioCuePlayMode.Sequence:
                    num = 0;
                    break;
                case AudioCuePlayMode.Random:
                    num= Random.Range(0, audioSO.audioList.Count);
                    
                    break;
                default:
                    break;
            }

            if (audioSO.audioList[num].clip==null)
                return;
            _audioSource.clip = audioSO.audioList[num].clip;
            //_audioSource.transform.position = position;
            _audioSource.outputAudioMixerGroup = mixerGroup;
            _audioSource.volume = audioSO.audioList[num].volume;
            _audioSource.loop = audioSO.audioList[num].loop;
            _audioSource.time = 0f; //Reset in case this AudioSource is being reused for a short SFX after being used for a long music track
            _audioSource.Play();
            StartCoroutine(FinishPlaying(_audioSource.clip.length));
        }

        
        private IEnumerator FinishPlaying(float time)
        {
            yield return new WaitForSecondsRealtime(time);
            OnSoundFinishedPlaying.Invoke(this);                               //Then this will be back to pool
        }
    }
}
