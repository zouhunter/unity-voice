using System;
using System.Collections;
using UnityEngine;
namespace VoiceRSS_SDK
{
	public interface IVoiceProvider
	{
		event SpeechReadyEventHandler SpeechReady;

		event SpeechFailedEventHandler SpeechFailed;

		string ApiKey
		{
			get;
			set;
		}

		bool IsSSL
		{
			get;
			set;
		}

        IEnumerator SpeechAsync(VoiceParameters voiceParams);
	}
}
