using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace TencentSpeech
{
    public static class AudioHelper
    {

        /// <summary>
        /// 从本地路径下载音效
        /// </summary>
        /// <param name="audioPath"></param>
        /// <param name="audioName"></param>
        /// <param name="OnGet"></param>
        /// <returns></returns>
        public static IEnumerator LoadFromFile(string audioPath, string audioName, UnityAction<AudioClip> OnGet)
        {
            Debug.Log("LoadFromFile");
            string url = audioPath + "/" + audioName;

            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                url = "file:///" + url;
            }
            Debug.Log(url);
            WWW www = new WWW(url);
            yield return www;
            if (www.error != null)
            {
                OnGet(null);
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www);

                OnGet(CreateWavAudio(audioName,www.bytes));
            }
        }

        public static AudioClip CreateWavAudio(string audioName, byte[] bytes)
        {
            WAV wav = new WAV(bytes);
            AudioClip audioClip = AudioClip.Create(audioName, wav.SampleCount, 1, wav.Frequency, false);
            audioClip.SetData(wav.LeftChannel, 0);
            return audioClip;
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

}