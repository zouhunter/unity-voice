using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using VoiceRSS_SDK;

namespace Txt2Audio
{
    //测试通过http://api.voicerss.org/?key=ceb04420d03d434a91e81608de6d1576&c=OGG&hl=en-us&src=你好Hello,%20world!123"
    public class Txt2AudioCtrl : ITextToAudio
    {
        #region 单例
        private static Txt2AudioCtrl instance = default(Txt2AudioCtrl);
        private static object lockHelper = new object();
        public static bool mManualReset = false;
        protected Txt2AudioCtrl()
        {
            musicCache = new ClassCacheCtrl(Application.dataPath);
            localAudio = musicCache.LoadClassFromLocal<LocalAudio>("audiorecord") ?? new LocalAudio();
        }

        public static Txt2AudioCtrl Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {

                            instance = new Txt2AudioCtrl();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
        //本地文件路径
        ClassCacheCtrl musicCache;
        LocalAudio localAudio;
        /// <summary>
        /// 获取默认的网络音频下载器
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        VoiceProvider DefultVP
        {
            get
            {
                VoiceProvider defultVP = new VoiceProvider("ceb04420d03d434a91e81608de6d1576");
                return defultVP;
            }

        }
        /// <summary>
        /// 获取默认参数
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        VoiceParameters DefultPar(string text)
        {
            VoiceParameters defultPar = new VoiceParameters(text, Languages.Chinese_China);
            defultPar.AudioCodec = AudioCodec.OGG;
            defultPar.SpeedRate = 1;
            defultPar.AudioFormat = AudioFormat.Format_alaw.AF_alaw_22khz_stereo;
            return defultPar;
        }

        /// <summary>
        /// 获取音频
        /// </summary>
        /// <param name="text"></param>
        /// <param name="OnGet"></param>
        /// <returns></returns>
        public IEnumerator GetAudioClip(string text, UnityAction<AudioClip> OnGet)
        {
            AudioData audioData = localAudio.GetAudioData(text);
            if (audioData != null)
            {
                Debug.Log(audioData.lengthSamples);

                AudioClip clip = AudioClip.Create(audioData.key, audioData.lengthSamples, audioData.channels, audioData.frequency, audioData.stream);
                clip.SetData(audioData.data,0);
                OnGet(clip);
            }
            else{
                yield return DownLandFromWeb(text, OnGet);
            }
        }

        IEnumerator DownLandFromWeb(string text, UnityAction<AudioClip> OnGet)
        {
            VoiceProvider vp = DefultVP;
            VoiceParameters par = DefultPar(text);
            vp.SpeechReady += (result) =>
            {
                AudioClip clip = result.GetAudioClip(false, false, AudioType.OGGVORBIS);
                OnGet(clip);
                localAudio.Register(text, clip);
                UpdateLocalRecord();
            };
            vp.SpeechFailed += (err) =>
            {
                Debug.Log(err);
                OnGet(null);
                localAudio.Remove(text);
            };
            yield return vp.SpeechAsync(par);
        }

        /// <summary>
        /// 更新本地记录
        /// </summary>
        void UpdateLocalRecord()
        {
            musicCache.SaveClassToLocal(localAudio, "audiorecord");
        }
        /// <summary>
        /// 清除所有本地信息
        /// </summary>
        public void CleanUp()
        {
            localAudio.audioData.Clear();
            UpdateLocalRecord();
        }
    }
}