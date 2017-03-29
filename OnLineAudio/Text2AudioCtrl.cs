using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class Text2AudioCtrl : SingleLengon<Text2AudioCtrl>, ITextToAudio
{
    //本地文件路径
    public string wavFilePath;
    public string md5FileName = "AudioMD5.csv";
    //FileMD5Table table;
    public Dictionary<string, string> SavedWav = new Dictionary<string, string>();
    //http://www.voicerss.org/api/documentation.aspx
    //测试通过http://api.voicerss.org/?key=ceb04420d03d434a91e81608de6d1576&c=OGG&hl=en-us&src=你好Hello,%20world!123"
    private static string tempAudio = "http://api.voicerss.org/?key=ceb04420d03d434a91e81608de6d1576&c=OGG&hl={0}&f={1}&src=";

    public static class Languages
    {
        public static string china = "zh-cn";
        public static string english = "en-us";
    }
    public static class Formats
    {
        public static string verylow = "11khz_8bit_mono";
        public static string normal = "alaw_22khz_stereo";
        public static string veryGood = "ulaw_44khz_stereo";
    }
    private static string GetConnectionString(string langluage, string formats)
    {
        return string.Format(tempAudio, langluage, formats);
    }

    public Text2AudioCtrl()
    {
        wavFilePath = Application.streamingAssetsPath + "/Audio";
        //CsvConfigManager.GetInstance().LoadConfigAsync<FileMD5Table>(md5FileName, (x) =>
        //{
        //    table = x;
        //    foreach (var item in x.GetRowList())
        //    {
        //        if (item.md5 == null)
        //        {
        //            continue;
        //        }
        //        SavedWav.Add(item.md5, item.fileName);
        //    }
        //});
    }

    public IEnumerator GetAudioClip(string text, string format, UnityAction<AudioClip> OnGet)
    {
        WWW www;
        string key = FileUtility.md5(text) + format;//!!!!!!!!!!!!!!!!!!!!!!增加权
        string filePath = Path.Combine(wavFilePath, key + ".ogg");
        if (!SavedWav.ContainsKey(key))
        {
            Debug.Log("没有文件");
            string url = GetConnectionString(Languages.china, format) + text;
            www = new WWW(url);
            yield return www;
            if (www.error != null)
            {
                Debug.LogWarning(www.error);
            }
            else
            {
                //File.WriteAllBytes(filePath, www.bytes);
                OnGet(www.GetAudioClip(false,false,AudioType.OGGVORBIS));
                //更新csv文件
                //if (table != null)
                //{
                //    FileMD5Table.Row rowx = new global::FileMD5Table.Row();
                //    rowx.fileName = filePath;
                //    rowx.md5 = key;
                //    table.GetRowList().Add(rowx);
                //    SavedWav.Add(rowx.md5, rowx.fileName);
                //    CsvConfigManager.SaveConfig(md5FileName, table);
                //}
            }
        }

        if (!FileUtility.IsFileExist(filePath)) yield break;

        www = new WWW("file:///" + filePath);
        yield return www;

        if (www.error != null)
        {
            Debug.LogError(www.error);
        }

        if (www.audioClip != null && OnGet != null)
        {
            OnGet(www.audioClip);
        }
        else
        {
            Debug.LogError("转换失败");
        }
    }
}

/*Parameter name 	Parameter description
key 	The API key (mandatory)
src 	The textual content for converting to speech (length limited by 100KB) (mandatory).
hl 	The textual content language. Allows values: see Languages. (mandatory)
r 	The speech rate (speed). Allows values: from -10 (slowest speed) up to 10 (fastest speed). Default value: 0 (normal speed). (optional)
c 	The speech audio codec. Allows values: see Audio Codecs. Default value: MP3. (optional)
f 	The speech audio formats. Allows values: see Audio Formats. Default value: 8khz_8bit_mono. (optional)
ssml 	The SSML textual content format. Allows values: true and false. Default value: false. (optional)
b64 	Defines output as a Base64 string format (for an internet browser playing). Allows values: true and false. Default value: false. (optional) */

