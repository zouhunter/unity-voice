using UnityEngine;
using System.Collections;

namespace XunFeiSpeech.Internal
{
    //事件委托
    public delegate void TTS_SpeakFinished(string text,byte[] bytes);
    public delegate void TTS_SpeakError( string err);
}