using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
namespace IFLYSpeech
{
    [RequireComponent(typeof(AudioSource))]
    public class TextAudioBehaiver : MonoBehaviour
    {
        private UnityAction<string> onComplete;
#if UNITY_STANDALONE
        public Windows.Txt2AudioCtrl ctrl { get { return Windows.Txt2AudioCtrl.Instance; } }
#elif UNITY_WEBGL
        public WebGL.Txt2AudioCtrl ctrl { get { return WebGL.Txt2AudioCtrl.Instance; } }
#endif

        private AudioSource audioSource;
        public bool IsOn { get; set; }
        public bool IsMan { get; set; }
        private AudioTimer audioTimer = new AudioTimer();

        void Awake()
        {
            IsOn = true;
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        private void Update()
        {
            if (!IsOn) return;
            audioTimer.Update();
        }
        /// <summary>
        /// 女声
        /// </summary>
        /// <param name="text"></param>
        public void PlayAudio(string text)
        {
            if (!IsOn) return;

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                audioTimer.Stop();
            }

            StartCoroutine(ctrl.GetAudioClip(text, (x) =>
            {
                if (x != null)
                {
                    audioSource.clip = x;
                    audioSource.Play();
                    audioTimer.Init(text, x.length, OnComplete);
                }
            }));
        }


        public void TogglePause(bool on)
        {
            if (!IsOn) return;

            if (audioTimer == null) return;

            if (!on)
            {
                audioSource.Pause();
                audioTimer.Pause();
            }
            else
            {
                audioSource.UnPause();
                audioTimer.UnPause();
            }
        }

        public void StopCurrent()
        {
            if (!IsOn) return;
            audioSource.Stop();
            audioTimer.Stop();
        }
        public void Stop(string data)
        {
            if (!IsOn || audioTimer.text != data) return;
            audioSource.Stop();
            audioTimer.Stop();
        }

        public void RegistCallBack(UnityAction<string> onComplete)
        {
            if (!IsOn) return;
            this.onComplete = onComplete;
        }

        private void OnComplete(string text)
        {
            if (onComplete != null) onComplete(text);
        }
    }

}
