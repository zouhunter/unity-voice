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

namespace TencentSpeech
{
    //https://ai.qq.com/doc/aaitts.shtml
    /// <summary>
    /// 腾讯在线语音（没有dll）
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
        private string urlTemp = "https://api.ai.qq.com/fcgi-bin/aai/aai_tts";
        public const string appkey = "j3vp5X6cKyitlwi0";
        private static string _audipPath;
        private static string AudioPath
        {
            get
            {
                if (_audipPath == null)
                {
                    _audipPath = Application.streamingAssetsPath + "/TencentAudio";

                    if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                    {
                        if (!Directory.Exists(_audipPath))
                        {
                            Directory.CreateDirectory(_audipPath);
                        }
                    }
                }
                return _audipPath;
            }
        }
        [System.Serializable]
        public class JsonTemp0
        {
            [System.Serializable]
            public class JsonTemp1
            {
                public string speech;
                public string md5sum;
            }
            public string ret;
            public string msg;
            public JsonTemp1 data;
        }

        /// <summary>
        /// 获取音频
        /// </summary>
        /// <param name="text"></param>
        /// <param name="OnGet"></param>
        /// <returns></returns>
        public IEnumerator GetAudioClip(string text, UnityAction<AudioClip> OnGet, Params paramss = null)
        {
            text = text.Replace(" ", "");//腾讯好像不支持空格
            if (paramss == null)
            {
                paramss = defultParams;
            }
            waitSpeekQueue.Enqueue(new KeyValuePair<string, Params>(text, paramss));
            if (waitSpeekQueue.Count > 1) yield break;

            while (waitSpeekQueue.Count > 0)//有其他协程在工作
            {
                var item = waitSpeekQueue.Dequeue();
                item.Value.text = item.Key;
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                long timeStamp = (long)(DateTime.Now - startTime).TotalSeconds; // 相差毫秒数
                item.Value.time_stamp = (int)timeStamp;
                item.Value.text = text;
                item.Value.format = (int)Format.WAV;//暂时只支持这种格式
                item.Value.sign = "";
                item.Value.sign = getReqSign(item.Value.GetParams(), appkey);
                var url = string.Format("{0}?{1}", urlTemp, item.Value.ToParamsString());
                var audioName = AudioHelper.md5(text + item.Value.speaker + item.Value.speed) + ".wav";
                //WebGL直接从指定的目录加载对应的文件(缓存)
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    bool local = false;
                    yield return AudioHelper.LoadFromFile(AudioPath, audioName, new UnityAction<AudioClip>(x =>
                    {
                        if (x != null)
                        {
                            local = true;
                            OnGet.Invoke(x);
                        }
                    }));

                    if (!local)
                    {
                        yield return IntenalDownLand(url, audioName, data => {
                            var audio = AudioHelper.CreateWavAudio(audioName, data);
                            OnGet(audio);
                        });
                    }
                }
                else
                {
                    var path = AudioPath + "/" + audioName;
                    if (!File.Exists(path))
                    {
                        //下载并缓存
                        yield return IntenalDownLand(url, audioName, new UnityAction<byte[]>(data =>
                        {
                            var audio = AudioHelper.CreateWavAudio(audioName, data);
                            OnGet(audio);
                            File.WriteAllBytes(AudioPath + "/" + audioName, data);
                        }));
                    }
                    else
                    {
                        yield return AudioHelper.LoadFromFile(AudioPath, audioName, OnGet);
                    }
                }
            }
        }

        private IEnumerator IntenalDownLand(string url, string audioName, UnityAction<byte[]> OnGet)
        {
            Debug.Log("IntenalDownLand");

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
                    try
                    {
                        var back = JsonUtility.FromJson<JsonTemp0>(json_request.downloadHandler.text);
                        var base64Str = back.data.speech;
                        if (string.IsNullOrEmpty(base64Str))
                        {
                            if (onError != null)
                                onError.Invoke(back.msg);
                        }
                        else
                        {
                            var data = System.Convert.FromBase64String(base64Str);
                            OnGet(data);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e);
                    }

                }
            }
        }



        private string getReqSign(List<KeyValuePair<string, string>> m_params, string appkey /* 字符串*/)
        {
            // 1. 字典升序排序
            m_params.Sort(Compire);

            // 2. 拼按URL键值对
            var str = "";
            foreach (var keyvalue in m_params)
            {
                if (keyvalue.Value != "")
                {
                    str += keyvalue.Key + '=' + keyvalue.Value + '&';
                }
            }
            // 3. 拼接app_key
            str += "app_key=" + appkey;

            // 4. MD5运算+转换大写，得到请求签名
            var sign = (AudioHelper.md5(str)).ToUpper();
            return sign;
        }

        private int Compire(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
        {
            return string.Compare(x.Key, y.Key);
        }

    }

}
