using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using TencentSpeech;

public class TencentDemo : MonoBehaviour {
    public Params parmas;
    private TencentSpeech.Txt2AudioCtrl ctrl;
    private void Awake()
    {
        ctrl = new Txt2AudioCtrl();
    }
    private void OnGUI()
    {
        parmas.text = GUILayout.TextField(parmas.text);
        if (GUILayout.Button("Play"))
        {
            StartCoroutine(ctrl.GetAudioClip(parmas.text,(x)=> {
                if(x != null)
                {
                    AudioSource.PlayClipAtPoint(x, transform.position);
                }
                else
                {
                    Debug.Log("clip:" + x);
                }
            },parmas));

        }
    }
}
