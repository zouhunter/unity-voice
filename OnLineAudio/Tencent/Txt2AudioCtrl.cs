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

namespace Speech.Tencent
{
    //https://ai.qq.com/doc/aaitts.shtml
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
        private string urlTemp = "https://api.ai.qq.com/fcgi-bin/aai/aai_tts";
        public const string appkey = "j3vp5X6cKyitlwi0";

        [System.Serializable]
        public class JsonTemp0
        {
            [System.Serializable]
            public class JsonTemp1
            {
                public string speech;
                public string md5sum;
            }
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
                item.Value.sign = getReqSign(item.Value.GetParams(), appkey);
                item.Value.text = text;
                var url = string.Format("{0}?{1}", urlTemp, item.Value.ToParamsString());
                Debug.Log(url);
                item.Value.format = (int)Format.WAV;

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
                        Debug.Log(json_request.downloadHandler.text);
                        //var reg = new Regex("speech\": \"(.*)\"");
                        //var match = reg.Match(json_request.downloadHandler.text);
                        //var base64Str = match.Groups[1].Value;
                        var back = JsonUtility.FromJson<JsonTemp0>(json_request.downloadHandler.text).data;
                        var base64Str = back.speech;
                        Debug.Log(base64Str);
                        var data = Base64Decode(base64Str);
                        Debug.Log(md5(data)+":" +back.md5sum);
                        var path = "C:/back.mp3";
                        System.IO.File.WriteAllText(path, data, System.Text.UTF8Encoding.UTF8);
                        Application.OpenURL(path);
                    }
                }
            }
        }

        public string getReqSign(List<KeyValuePair<string, string>> m_params, string appkey /* 字符串*/)
        {
            // 1. 字典升序排序
            //m_params.Sort();
            // 2. 拼按URL键值对
            var str = "";
            foreach (var keyvalue in m_params)
            {
                if (keyvalue.Value != "")
                {
                    var value = System.Uri.EscapeDataString(keyvalue.Value);
                    str += keyvalue.Key + '=' + value + '&';
                }
            }
            // 3. 拼接app_key
            str += "app_key=" + appkey;

            // 4. MD5运算+转换大写，得到请求签名
            var sign = (md5(str)).ToUpper();
            return sign;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        /// <summary>
        /// 计算字符串的MD5值
        /// </summary>
        public static string md5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();

            string destString = "";
            for (int i = 0; i < md5Data.Length; i++)
            {
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            }
            destString = destString.PadLeft(32, '0');
            return destString;
        }
    }
}
