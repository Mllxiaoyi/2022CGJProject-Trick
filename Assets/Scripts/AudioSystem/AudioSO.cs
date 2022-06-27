using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/AudioCue", fileName = "New AudioCue")]
    public class AudioSO : ScriptableObject
    {
        public AudioCuePlayMode audioCuePlayMode;

        public AudioCueType audioType;

        public List<AudioCue> audioList;
        
    }



    [System.Serializable]
    public class AudioCue
    {
        /// <summary>
        /// “Ù∆µ∆¨∂Œ
        /// </summary>
        public AudioClip clip;
        /// <summary>
        /// “Ù∆µ“Ù¡ø
        /// </summary>
        [Range(0, 1)]
        public float volume = 1;
        /// <summary>
        ///  «∑Ò—≠ª∑≤•∑≈
        /// </summary>
        public bool loop;

    }

    public enum AudioCuePlayMode
    {
        Normal,
        Sequence,
        Random
    }

    public enum AudioCueType
    {
        Bgm,
        Sfx,
        UI_Sfx,
        GamePlay_Sfx
    }
}
