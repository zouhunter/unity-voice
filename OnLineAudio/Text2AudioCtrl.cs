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
    public class Text2AudioCtrl : ITextToAudio
    {
        #region 单例
        private static Text2AudioCtrl instance = default(Text2AudioCtrl);
        private static object lockHelper = new object();
        public static bool mManualReset = false;
        protected Text2AudioCtrl()
        {
            musicCache = new ClassCacheCtrl(Application.dataPath);
            localAudio = musicCache.LoadClassFromLocal<LocalAudio>("audiorecord") ?? new LocalAudio();
        }

        public static Text2AudioCtrl Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {

                            instance = new Text2AudioCtrl();
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
            if (localAudio.Contain(text))
            {
                yield return LoadFromFile(text, (x) =>
                {
                    if (x != null) {
                        OnGet(x);
                    }
                });
            }
            if (!localAudio.Contain(text))
                yield return DownLandFromWeb(text, OnGet);
        }

        IEnumerator DownLandFromWeb(string text, UnityAction<AudioClip> OnGet)
        {
            VoiceProvider vp = DefultVP;
            VoiceParameters par = DefultPar(text);
            vp.SpeechReady += (result) =>
            {
                OnGet(result.GetAudioClip(false, false, AudioType.OGGVORBIS));
                localAudio.Register(text);
                SaveToLocal(text, result.bytes);
            };
            vp.SpeechFailed += (err) =>
            {
                Debug.Log(err);
                OnGet(null);
                localAudio.Remove(text);
            };
            yield return vp.SpeechAsync(DefultPar(text));
        }

        /// <summary>
        /// 从本地下载
        /// </summary>
        /// <param name="text"></param>
        /// <param name="OnGet"></param>
        /// <returns></returns>
        IEnumerator LoadFromFile(string text, UnityAction<AudioClip> OnGet)
        {
            string path = localAudio.GetPath(text);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                WWW www = new WWW("file:///" + path);
                yield return www;
                if (www.error != null)
                {
                    OnGet(null);
                    Debug.Log(www.error);
                }
                else
                {
                    OnGet(www.GetAudioClip(false,false,AudioType.OGGVORBIS));
                }
            }
            else
            {
                OnGet(null);
            }
        }
        /// <summary>
        /// 保存信息到本地
        /// </summary>
        /// <param name="text"></param>
        /// <param name="bytes"></param>
        void SaveToLocal(string text,byte[] bytes)
        {
            string path = localAudio.GetPath(text);
            File.WriteAllBytes(path, bytes);
            UpdateLocalRecord();
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
            for (int i = 0; i < localAudio.texts.Count; i++){
                string path = localAudio.GetPath(localAudio.texts[i]); 
                if (File.Exists(path)){
                    File.Delete(path);
                }
            }
            localAudio.texts = new List<string>();
        }
    }
}