using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEngine.Events;

namespace Game.Audio
{
    [CreateAssetMenu(menuName = "Game/Audio/AudioPlayerPool", fileName = "AudioPlayerPool")]
    public class AudioPlayerPoolSO : GameObjectPoolSO<AudioPlayer>
    {
        public override IFactory<AudioPlayer> Factory
        { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
         

        protected override AudioPlayer Create()
        {
            AudioPlayer audioSource = new GameObject().AddComponent<AudioPlayer>();
            audioSource.gameObject.name = "AudioPlayer";
            audioSource.OnSoundFinishedPlaying += OnAudioFinishingPlay;
            return audioSource;
        }

        private void OnAudioFinishingPlay(AudioPlayer audioPlayer)
        {
            audioPlayer.gameObject.SetActive(false);
        }


    }
}
