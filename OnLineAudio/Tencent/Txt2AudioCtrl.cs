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
                var url = string.Format("{0}?{1}",urlTemp,item.Value.ToParamsString());
                Debug.Log(url);
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
                            Debug.Log(json_request.downloadHandler.text);
                            var back = JsonUtility.FromJson<JsonTemp0>(json_request.downloadHandler.text);
                            var base64Str = back.data.speech;
                            if (string.IsNullOrEmpty(base64Str))
                            {
                                if (onError != null)
                                    onError.Invoke(back.msg);
                            }
                            else
                            {
                                Debug.Log(base64Str);
                                var data = System.Convert.FromBase64String(base64Str);
                                var audio = CreateAudio(data, Format.WAV);
                                OnGet(audio);
                            }

                        }
                        catch (Exception e)
                        {
                            Debug.LogWarning(e);
                        }
                       
                    }
                }
            }
        }

        private AudioClip CreateAudio(byte[] bytes, Format format)
        {
            WAV wav = new WAV(bytes);
            AudioClip audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false);
            audioClip.SetData(wav.LeftChannel, 0);
            return audioClip;
        }

        public string getReqSign(List<KeyValuePair<string, string>> m_params, string appkey /* 字符串*/)
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
            var sign = (md5(str)).ToUpper();
            return sign;
        }

        private int Compire(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
        {
            return string.Compare(x.Key, y.Key);
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

        public static string md5(byte[] data)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
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

    public class WAV
    {
        // convert two bytes to one float in the range -1 to 1
        static float bytesToFloat(byte firstByte, byte secondByte)
        {
            // convert two bytes to one short (little endian)
            short s = (short)((secondByte << 8) | firstByte);
            // convert to range from -1 to (just below) 1
            return s / 32768.0F;
        }

        static int bytesToInt(byte[] bytes, int offset = 0)
        {
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                value |= ((int)bytes[offset + i]) << (i * 8);
            }
            return value;
        }

        private static byte[] GetBytes(string filename)
        {
            return File.ReadAllBytes(filename);
        }
        // properties
        public float[] LeftChannel { get; internal set; }
        public float[] RightChannel { get; internal set; }
        public int ChannelCount { get; internal set; }
        public int SampleCount { get; internal set; }
        public int Frequency { get; internal set; }

        // Returns left and right double arrays. 'right' will be null if sound is mono.
        public WAV(string filename) :
            this(GetBytes(filename))
        { }

        public WAV(byte[] wav)
        {
            // Determine if mono or stereo
            ChannelCount = wav[22];     // Forget byte 23 as 99.999% of WAVs are 1 or 2 channels

            // Get the frequency
            Frequency = bytesToInt(wav, 24);

            // Get past all the other sub chunks to get to the data subchunk:
            int pos = 12;   // First Subchunk ID from 12 to 16

            // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
            while (!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97))
            {
                pos += 4;
                int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
                pos += 4 + chunkSize;
            }
            pos += 8;

            // Pos is now positioned to start of actual sound data.
            SampleCount = (wav.Length - pos) / 2;     // 2 bytes per sample (16 bit sound mono)
            if (ChannelCount == 2) SampleCount /= 2;        // 4 bytes per sample (16 bit stereo)

            // Allocate memory (right will be null if only mono sound)
            LeftChannel = new float[SampleCount];
            if (ChannelCount == 2) RightChannel = new float[SampleCount];
            else RightChannel = null;

            // Write to double array/s:
            int i = 0;
            while (pos < wav.Length)
            {
                LeftChannel[i] = bytesToFloat(wav[pos], wav[pos + 1]);
                pos += 2;
                if (ChannelCount == 2)
                {
                    RightChannel[i] = bytesToFloat(wav[pos], wav[pos + 1]);
                    pos += 2;
                }
                i++;
            }
        }

        public override string ToString()
        {
            return string.Format("[WAV: LeftChannel={0}, RightChannel={1}, ChannelCount={2}, SampleCount={3}, Frequency={4}]", LeftChannel, RightChannel, ChannelCount, SampleCount, Frequency);
        }
    }
}
