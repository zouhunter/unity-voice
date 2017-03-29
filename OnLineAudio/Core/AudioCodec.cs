using System;
using System.Runtime.InteropServices;

namespace VoiceRSS_SDK
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct AudioCodec
	{
		public static string MP3
		{
			get
			{
				return "MP3";
			}
		}

		public static string WAV
		{
			get
			{
				return "WAV";
			}
		}

		public static string AAC
		{
			get
			{
				return "AAC";
			}
		}

		public static string OGG
		{
			get
			{
				return "OGG";
			}
		}

		public static string CAF
		{
			get
			{
				return "CAF";
			}
		}
	}
}
