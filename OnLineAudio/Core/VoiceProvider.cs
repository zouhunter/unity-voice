using System;
using System.Text;
using UnityEngine;
using System.Collections;

namespace VoiceRSS_SDK
{
    //http://www.voicerss.org/api/documentation.aspx

    public class VoiceProvider : IVoiceProvider
    {
        private string _apiKey;

        private bool _isSSL;

        public event SpeechReadyEventHandler SpeechReady;

        public event SpeechFailedEventHandler SpeechFailed;

        public string ApiKey
        {
            get
            {
                return this._apiKey;
            }
            set
            {
                this._apiKey = value;
            }
        }

        public bool IsSSL
        {
            get
            {
                return this._isSSL;
            }
            set
            {
                this._isSSL = value;
            }
        }

        public VoiceProvider(string apiKey)
        {
            this._apiKey = apiKey;
        }

        public VoiceProvider(string apiKey, bool isSSL) : this(apiKey)
        {
            this._isSSL = isSSL;
        }

        private string buildParameters(VoiceParameters voiceParams)
        {
            return string.Format("key={0}&b64={1}&hl={2}&r={3}&c={4}&f={5}&ssml={6}&src={7}", new object[]
            {
                this._apiKey,
                voiceParams.IsBase64,
                voiceParams.Language,
                voiceParams.SpeedRate,
                voiceParams.AudioCodec,
                voiceParams.AudioFormat,
                voiceParams.IsSsml,
                voiceParams.Text
            });
        }

        private bool Validate(VoiceParameters voiceParams)
        {
            if (string.IsNullOrEmpty(this._apiKey))
            {
                if (SpeechFailed != null) SpeechFailed(new Exception("The API key is undefined"));
                return false;
            }
            if (string.IsNullOrEmpty(voiceParams.Text))
            {
                if (SpeechFailed != null) SpeechFailed(new Exception("The text is undefined"));
                return false;
            }
            if (string.IsNullOrEmpty(voiceParams.Language))
            {
                if (SpeechFailed != null) SpeechFailed(new Exception("The language is undefined"));
                return false;
            }
            return true;
        }

        public IEnumerator SpeechAsync(VoiceParameters voiceParams)
        {
            if (!this.Validate(voiceParams)) yield break;
            string hader = this._isSSL ? "https" : "http";
            string address = "api.voicerss.org";
            string par = this.buildParameters(voiceParams);
            string url = string.Format("{0}://{1}/?{2}", hader, address, par);
            System.Uri uri;
            System.Uri.TryCreate(url, System.UriKind.Absolute, out uri);
            WWW www = new WWW(uri.AbsoluteUri);/*mast be % escaped*/
            yield return www;
            if (www.error == null)
            {
                if (www.text.StartsWith("ERROR"))
                {
                    if (SpeechFailed != null) SpeechFailed(new Exception(www.text));
                    else
                    {
                        Debug.Log("[SpeechFailed]:" + www.text);
                    }
                }
                else
                {
                    if (SpeechReady != null) SpeechReady(www);
                    else
                    {
                        Debug.Log("[SpeechReady]:" + www.text);
                    }
                }
            }
            else
            {
                if (SpeechFailed != null) SpeechFailed(new Exception(www.error));
                else
                {
                    Debug.Log("[SpeechFailed]:" + www.error);
                }
            }
        }

    }
}
