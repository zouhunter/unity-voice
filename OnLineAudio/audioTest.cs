using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class audioTest : MonoBehaviour {
    public Button btn;
    public InputField input;
    public Text2AudioCtrl ctrl;
    public AudioSource audio;
	void Start () {
        btn.onClick.AddListener(PlayAudio);
        ctrl = new global::Text2AudioCtrl();
    }
	
	void PlayAudio () {
        StartCoroutine(ctrl.GetAudioClip(input.text, Text2AudioCtrl.Formats.veryGood, OnGET));
	}
    void OnGET(AudioClip clip)
    {
        audio.PlayOneShot(clip);
    }
}
