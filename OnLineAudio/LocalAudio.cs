using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Txt2Audio
{
    /// <summary>
    /// 本地音乐
    /// </summary>
    [System.Serializable]
    public class LocalAudio
    {
        public string StreamRoot
        {
            get{
                return "Audio";
            }
        }
        public List<AudioData> audioData = new List<AudioData>();
        public void Register(string text,AudioClip data)
        {
            AudioData old = audioData.Find(x => x.key == text);
            if (old == null){

                audioData.Add(new AudioData(text,data));
            }
            else
            {
                old.SetData(data);
            }
        }
        public void Remove(string text)
        {
            AudioData old = audioData.Find(x => x.key == text);
            if (old != null)
            {
                audioData.Remove(old);
            }
        }
        public AudioData GetAudioData(string text)
        {
            AudioData old = audioData.Find(x => x.key == text);
            return old;
        }
    }

    [System.Serializable]
    public class AudioData
    {
        public string key;
        public int lengthSamples;
        public int channels;
        public int frequency;
        public bool stream;
        public float[] data;

        public AudioData(string key,AudioClip clip)
        {
            this.key = key;
            this.lengthSamples = clip.samples;
            this.channels = clip.channels;
            this.frequency = clip.frequency;
            this.stream = false;
            SetData(clip);
        }

        public void SetData(AudioClip clip)
        {
            float[] samples = new float[clip.samples * clip.channels];
            clip.GetData(samples, 0);
            data = samples;
        }
    }
}