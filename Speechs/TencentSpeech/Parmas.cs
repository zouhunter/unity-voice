using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace TencentSpeech
{
    public enum Format : int
    {
        PCM = 1,
        WAV = 2,
        MP3 = 3,
    }

    public enum Speaker : int
    {
        普通话男声 = 1,
        静琪女声 = 5,
        欢馨女声 = 6,
        碧萱女声 = 7,
    }

    [System.Serializable]
    public class Params
    {
        public int app_id = 1106669713;// 是   int 正整数 	1000001 	应用标识（AppId）
        public int time_stamp = 1493468759;// 是   int 正整数 	1493468759 	请求时间戳（秒级）
        public string nonce_str = "fa577ce340859f9fe";// 是   string 非空且长度上限32字节     fa577ce340859f9fe 随机字符串
        public string sign = "";//是   string 非空且长度固定32字节     B250148B284956EC5218D4B0503E7F8A 签名信息，详见接口鉴权
        public int speaker = (int)Speaker.普通话男声;//   是 int 正整数 	1 	语音发音人编码，定义见下文描述
        public int format = (int)Format.WAV;//  是 int 正整数 	2 	合成语音格式编码，定义见下文描述
        public int volume = 5;// 是 int[-10, 10] 	0 	合成语音音量，取值范围[-10, 10]，如-10表示音量相对默认值小10dB，0表示默认音量，10表示音量相对默认值大10dB
        public int speed = 100;//   是 int[50, 200] 	100 	合成语音语速，默认100
        public string text ="Hellow World";//    是 string UTF-8编码，非空且长度上限150字节 腾讯，你好！ 	待合成文本
        public int aht = 0;//   是 int[-24, 24] 	0 	合成语音降低/升高半音个数，即改变音高，默认0
        public int apc = 58;// 是 	int 	[0, 100] 	58 	控制频谱翘曲的程度，改变说话人的音色，默认58

        public Dictionary<string, string> ToParamsDic()
        {
            var pairs = new Dictionary<string, string>();
            var fields = typeof(Params).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance);

            for (int i = 0; i < fields.Length; i++)
            {
                var value = fields[i].GetValue(this).ToString();
                pairs.Add(fields[i].Name, value);
            }
            return pairs;
        }

        public string ToParamsString()
        {
            var fields = typeof(Params).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance);
            var param = new string[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                var value = fields[i].GetValue(this).ToString();
                value = System.Uri.EscapeDataString(value);
                param[i] = fields[i].Name + "=" + value;
            }
            return string.Join("&", param);
        }

        public List<KeyValuePair<string,string>> GetParams()
        {
            var pairs = new List<KeyValuePair<string, string>>();
            var fields = typeof(Params).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++) {
                var value = System.Uri.EscapeDataString(fields[i].GetValue(this).ToString());
                pairs.Add(new KeyValuePair<string, string>(fields[i].Name, value));
            }
            return pairs;
        }
       
    }
}
