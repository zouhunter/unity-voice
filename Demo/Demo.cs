using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class Demo : MonoBehaviour
{
    public InputField textbox;
    public Button tranBtn;
    public Button clearBtn;
    public AudioSource audio;

    IFLYSpeech.Txt2AudioCtrl ctrl;
    void Start()
    {
        tranBtn.onClick.AddListener(RequireAudio);
        clearBtn.onClick.AddListener(RemoveLocal);
        ctrl = IFLYSpeech.Txt2AudioCtrl.Instance;
        ctrl.onError += OnError;
    }
    void RequireAudio()
    {
        StartCoroutine(ctrl.GetAudioClip(textbox.text, OnGet));
    }

    void RemoveLocal()
    {
        ctrl.CleanUpCatchs();
    }
    void OnError(string err)
    {
        Debug.LogError(err);
    }

    void OnGet(AudioClip clip)
    {
        audio.PlayOneShot(clip);
    }
}
