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

    Txt2Audio.Text2AudioCtrl ctrl;
    void Start()
    {
        tranBtn.onClick.AddListener(RequireAudio);
        clearBtn.onClick.AddListener(RemoveLocal);
        ctrl = Txt2Audio.Text2AudioCtrl.Instance;
    }
    void RequireAudio()
    {
        StartCoroutine(ctrl.GetAudioClip(textbox.text, OnGet));
    }

    void RemoveLocal()
    {
        ctrl.CleanUp();
    }

    void OnGet(AudioClip clip)
    {
        audio.PlayOneShot(clip);
    }
}
