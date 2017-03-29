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
        public List<string> texts = new List<string>();

        public void Register(string text)
        {
            if (!texts.Contains(text)){
                texts.Add(text);
            }
        }
        public bool Contain(string text){
            return texts.Contains(text);
        }
        public void Remove(string text)
        {
            if (texts.Contains(text))
            {
                texts.Remove(text);
            }
        }
        public string GetPath(string text)
        {
            if (texts.Contains(text)){
                return string.Format("{0}/{1}/{2}.audio",Application.streamingAssetsPath,StreamRoot,md5(text));
            }
            return null;
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