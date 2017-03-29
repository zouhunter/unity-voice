using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


namespace Txt2Audio
{
    public interface ITextToAudio
    {
        IEnumerator GetAudioClip(string text, UnityAction<AudioClip> OnGet);
        void CleanUp();
    }
}
