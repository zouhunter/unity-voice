using System;

namespace VoiceRSS_SDK
{
	public class VoiceParameters
	{
		public string Text
		{
			get;
			set;
		}

		public string Language
		{
			get;
			set;
		}

		public string AudioCodec
		{
			get;
			set;
		}

		public string AudioFormat
		{
			get;
			set;
		}

		public int SpeedRate
		{
			get;
			set;
		}

		public bool IsSsml
		{
			get;
			set;
		}

		public bool IsBase64
		{
			get;
			set;
		}

		public VoiceParameters(string text, string language)
		{
			this.Text = text;
			this.Language = language;
		}
        public VoiceParameters(string text, string language,string audioCodec)
        {
            this.Text = text;
            this.Language = language;
            this.AudioCodec = audioCodec;
        }
    }
}
