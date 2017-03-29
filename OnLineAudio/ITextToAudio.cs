using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public interface ITextToAudio {
    IEnumerator GetAudioClip(string text, string format, UnityAction<AudioClip> OnGet);
    
}
