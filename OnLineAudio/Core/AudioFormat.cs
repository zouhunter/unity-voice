using System;
using System.Runtime.InteropServices;

namespace VoiceRSS_SDK
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct AudioFormat
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Format_8KHZ
		{
			public static string AF_8khz_8bit_mono
			{
				get
				{
					return "8khz_8bit_mono";
				}
			}

			public static string AF_8khz_8bit_stereo
			{
				get
				{
					return "8khz_8bit_stereo";
				}
			}

			public static string AF_8khz_16bit_mono
			{
				get
				{
					return "8khz_16bit_mono";
				}
			}

			public static string AF_8khz_16bit_stereo
			{
				get
				{
					return "8khz_16bit_stereo";
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Format_11KHZ
		{
			public static string AF_11khz_8bit_mono
			{
				get
				{
					return "11khz_8bit_mono";
				}
			}

			public static string AF_11khz_8bit_stereo
			{
				get
				{
					return "11khz_8bit_stereo";
				}
			}

			public static string AF_11khz_16bit_mono
			{
				get
				{
					return "11khz_16bit_mono";
				}
			}

			public static string AF_11khz_16bit_stereo
			{
				get
				{
					return "11khz_16bit_stereo";
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Format_12KHZ
		{
			public static string AF_12khz_8bit_mono
			{
				get
				{
					return "12khz_8bit_mono";
				}
			}

			public static string AF_12khz_8bit_stereo
			{
				get
				{
					return "12khz_8bit_stereo";
				}
			}

			public static string AF_12khz_16bit_mono
			{
				get
				{
					return "12khz_16bit_mono";
				}
			}

			public static string AF_12khz_16bit_stereo
			{
				get
				{
					return "12khz_16bit_stereo";
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Format_16KHZ
		{
			public static string AF_16khz_8bit_mono
			{
				get
				{
					return "16khz_8bit_mono";
				}
			}

			public static string AF_16khz_8bit_stereo
			{
				get
				{
					return "16khz_8bit_stereo";
				}
			}

			public static string AF_16khz_16bit_mono
			{
				get
				{
					return "16khz_16bit_mono";
				}
			}

			public static string AF_16khz_16bit_stereo
			{
				get
				{
					return "16khz_16bit_stereo";
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Format_22KHZ
		{
			public static string AF_22khz_8bit_mono
			{
				get
				{
					return "22khz_8bit_mono";
				}
			}

			public static string AF_22khz_8bit_stereo
			{
				get
				{
					return "22khz_8bit_stereo";
				}
			}

			public static string AF_22khz_16bit_mono
			{
				get
				{
					return "22khz_16bit_mono";
				}
			}

			public static string AF_22khz_16bit_stereo
			{
				get
				{
					return "22khz_16bit_stereo";
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Format_24KHZ
		{
			public static string AF_24khz_8bit_mono
			{
				get
				{
					return "24khz_8bit_mono";
				}
			}

			public static string AF_24khz_8bit_stereo
			{
				get
				{
					return "24khz_8bit_stereo";
				}
			}

			public static string AF_24khz_16bit_mono
			{
				get
				{
					return "24khz_16bit_mono";
				}
			}

			public static string AF_24khz_16bit_stereo
			{
				get
				{
					return "24khz_16bit_stereo";
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Format_32KHZ
		{
			public static string AF_32khz_8bit_mono
			{
				get
				{
					return "32khz_8bit_mono";
				}
			}

			public static string AF_32khz_8bit_stereo
			{
				get
				{
					return "32khz_8bit_stereo";
				}
			}

			public static string AF_32khz_16bit_mono
			{
				get
				{
					return "32khz_16bit_mono";
				}
			}

			public static string AF_32khz_16bit_stereo
			{
				get
				{
					return "32khz_16bit_stereo";
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Format_44KHZ
		{
			public static string AF_44khz_8bit_mono
			{
				get
				{
					return "44khz_8bit_mono";
				}
			}

			public static string AF_44khz_8bit_stereo
			{
				get
				{
					return "44khz_8bit_stereo";
				}
			}

			public static string AF_44khz_16bit_mono
			{
				get
				{
					return "44khz_16bit_mono";
				}
			}

			public static string AF_44khz_16bit_stereo
			{
				get
				{
					return "44khz_16bit_stereo";
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Format_48KHZ
		{
			public static string AF_48khz_8bit_mono
			{
				get
				{
					return "48khz_8bit_mono";
				}
			}

			public static string AF_48khz_8bit_stereo
			{
				get
				{
					return "48khz_8bit_stereo";
				}
			}

			public static string AF_48khz_16bit_mono
			{
				get
				{
					return "48khz_16bit_mono";
				}
			}

			public static string AF_48khz_16bit_stereo
			{
				get
				{
					return "48khz_16bit_stereo";
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Format_alaw
		{
			public static string AF_alaw_8khz_mono
			{
				get
				{
					return "alaw_8khz_mono";
				}
			}

			public static string AF_alaw_8khz_stereo
			{
				get
				{
					return "alaw_8khz_stereo";
				}
			}

			public static string AF_alaw_11khz_mono
			{
				get
				{
					return "alaw_11khz_mono";
				}
			}

			public static string AF_alaw_11khz_stereo
			{
				get
				{
					return "alaw_11khz_stereo";
				}
			}

			public static string AF_alaw_22khz_mono
			{
				get
				{
					return "alaw_22khz_mono";
				}
			}

			public static string AF_alaw_22khz_stereo
			{
				get
				{
					return "alaw_22khz_stereo";
				}
			}

			public static string AF_alaw_44khz_mono
			{
				get
				{
					return "alaw_44khz_mono";
				}
			}

			public static string AF_alaw_44khz_stereo
			{
				get
				{
					return "alaw_44khz_stereo";
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Format_ulaw
		{
			public static string AF_ulaw_8khz_mono
			{
				get
				{
					return "ulaw_8khz_mono";
				}
			}

			public static string AF_ulaw_8khz_stereo
			{
				get
				{
					return "ulaw_8khz_stereo";
				}
			}

			public static string AF_ulaw_11khz_mono
			{
				get
				{
					return "ulaw_11khz_mono";
				}
			}

			public static string AF_ulaw_11khz_stereo
			{
				get
				{
					return "ulaw_11khz_stereo";
				}
			}

			public static string AF_ulaw_22khz_mono
			{
				get
				{
					return "ulaw_22khz_mono";
				}
			}

			public static string AF_ulaw_22khz_stereo
			{
				get
				{
					return "ulaw_22khz_stereo";
				}
			}

			public static string AF_ulaw_44khz_mono
			{
				get
				{
					return "ulaw_44khz_mono";
				}
			}

			public static string AF_ulaw_44khz_stereo
			{
				get
				{
					return "ulaw_44khz_stereo";
				}
			}
		}
	}
}
