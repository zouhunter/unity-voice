using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using IFLYSpeech;

[RequireComponent(typeof(TextAudioBehaiver))]
public class Demo : MonoBehaviour
{
    public Button m_Play;
    public Button m_Pause;
    public InputField m_texts;
    public Button m_ClearBtn;
    public Button downLandGroup;
    public Toggle[] toggles;
    private Params parma = new Params();
    IFLYSpeech.Txt2AudioCtrl ctrl { get { return IFLYSpeech.Txt2AudioCtrl.Instance; } }
    private TextAudioBehaiver audioBehaiver;
    void Start()
    {
        audioBehaiver = GetComponent<TextAudioBehaiver>();
        audioBehaiver.RegistCallBack(OnCallBack);
        m_ClearBtn.onClick.AddListener(RemoveLocal);
        m_Play.onClick.AddListener(Play);
        m_Pause.onClick.AddListener(Pause);
        downLandGroup.onClick.AddListener(GroupDownLand);
        ctrl.onError += OnError;
        for (int i = 0; i < toggles.Length; i++)
        {
            var index = i;
            toggles[index].onValueChanged.AddListener(x=> { if (x)  ActiveSpeaker(toggles[index].GetComponentInChildren<Text>().text); });
        }
    }

    private void OnCallBack(string arg0)
    {
        Debug.Log("Complete:" + arg0);
    }

    private void GroupDownLand()
    {
        var infos =  m_texts.text.Split(new Char[] { '|' });
        StartCoroutine(ctrl.Downland(infos, (x) => { Debug.Log("下载进度"+ x); }));
    }
    private void ActiveSpeaker(string speaker)
    {
        ctrl.defultParams.voice_name = speaker;
    }
    void RemoveLocal()
    {
        ctrl.CleanUpCatchs();
    }
    void OnError(string err)
    {
        Debug.LogError(err);
    }
    public void Play()
    {
        audioBehaiver.PlayAudio(m_texts.text);
    }
    public void Pause()
    {
        audioBehaiver.TogglePause(false);
    }
}

