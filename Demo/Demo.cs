using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using Speech;

public class Demo : MonoBehaviour
{
    public Button m_Play;
    public Button m_Pause;
    public InputField m_texts;
    public Button m_ClearBtn;
    public Button downLandGroup;
    public Toggle[] toggles;
    private TextAudioBehaiver audioBehaiver;

    void Start()
    {
        audioBehaiver = GetComponentInChildren<TextAudioBehaiver>();
        audioBehaiver.RegistCallBack(OnCallBack);
        m_ClearBtn.onClick.AddListener(RemoveLocal);
        m_Play.onClick.AddListener(Play);
        m_Pause.onClick.AddListener(Pause);
        downLandGroup.onClick.AddListener(GroupDownLand);
        audioBehaiver.ctrl.onError += OnError;
        for (int i = 0; i < toggles.Length; i++)
        {
            var index = i;
            toggles[index].onValueChanged.AddListener(x => { if (x) ActiveSpeaker(index); });
        }

#if UNITY_WEBGL
        m_ClearBtn.gameObject.SetActive(false);
#endif
    }
    private void InitToggles()
    {

    }

    private void OnCallBack(string arg0)
    {
        Debug.Log("Complete:" + arg0);
    }

    private void GroupDownLand()
    {
#if UNITY_STANDALONE
           var infos =  m_texts.text.Split(new Char[] { '|' });
        StartCoroutine(audioBehaiver.ctrl.Downland(infos, (x) => { Debug.Log("下载进度"+ x); }));
#endif

    }
    private void ActiveSpeaker(int index)
    {
#if UNITY_STANDALONE
        var speaker = toggles[index].GetComponentInChildren<Text>().text;
        audioBehaiver.ctrl.defultParams.voice_name = speaker;
#elif UNITY_WEBGL
        var name = Enum.GetNames(typeof(Speech.Tencent.Speaker))[index];
        var speaker = (int)(Speech.Tencent.Speaker)Enum.Parse(typeof(Speech.Tencent.Speaker), name);
        audioBehaiver.ctrl.defultParams.speaker = speaker;
#endif
    }
    void RemoveLocal()
    {
#if UNITY_STANDALONE
        audioBehaiver.ctrl.CleanUpCatchs();
#endif
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

