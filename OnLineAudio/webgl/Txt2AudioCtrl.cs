using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

namespace IFLYSpeech.WebGL
{
    /// <summary>
    /// 在线语音，不用缓存
    /// </summary>
    public class Txt2AudioCtrl
    {
        #region 单例
        private static Txt2AudioCtrl instance = default(Txt2AudioCtrl);
        private static object lockHelper = new object();
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
        public event UnityAction<string> onError;
        private Params _defultParmas;
        public Params defultParams { get { if (_defultParmas == null) _defultParmas = new Params(); return _defultParmas; } }
        private Queue<KeyValuePair<string, Params>> waitSpeekQueue = new Queue<KeyValuePair<string, Params>>();
        private bool connectError;
        private string urlTemp = "http://www.xfyun.cn/herapi/solution/synthesis";
        public class Back
        {
            public string data;
        }
        /// <summary>
        /// 获取音频
        /// </summary>
        /// <param name="text"></param>
        /// <param name="OnGet"></param>
        /// <returns></returns>
        public IEnumerator GetAudioClip(string text, UnityAction<AudioClip> OnGet, Params paramss = null)
        {
            if (paramss == null)
            {
                paramss = defultParams;
            }
            waitSpeekQueue.Enqueue(new KeyValuePair<string, Params>(text, paramss));
            if (waitSpeekQueue.Count > 1) yield break;
            while (waitSpeekQueue.Count > 0)//有其他协程在工作
            {
                var item = waitSpeekQueue.Dequeue();
                var url = string.Format("{0}?{1}&textPut={2}", urlTemp, item.Value, item.Key);
                using (UnityWebRequest json_request = UnityWebRequest.Get(url))
                {
                    yield return json_request.Send();

                    if (json_request.isError)
                    {
                        if (onError != null)
                            onError.Invoke(json_request.error);
                    }
                    else
                    {

                        var back = JsonUtility.FromJson<Back>(json_request.downloadHandler.text);
                        using (WWW audio_request =new WWW(back.data))
                        {
                            yield return audio_request;
                            if (audio_request.error != null)
                            {
                                if (onError != null)
                                    onError.Invoke(audio_request.error);
                            }
                            else
                            {
                                var clip = audio_request.GetAudioClipCompressed(false, AudioType.MPEG);
                                OnGet(clip);
                            }
                        }
                    }
                }
            }
        }

    }
}
