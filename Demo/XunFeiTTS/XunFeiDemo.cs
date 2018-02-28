using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using XunFeiSpeech.TTS;

public class XunFeiDemo : MonoBehaviour
{
    public Button m_Play;
    public Toggle m_Pause;
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
        m_Pause.onValueChanged.AddListener(Pause);
        downLandGroup.onClick.AddListener(GroupDownLand);
        audioBehaiver.ctrl.onError += OnError;
        for (int i = 0; i < toggles.Length; i++)
        {
            var index = i;
            toggles[index].onValueChanged.AddListener(x => { if (x) ActiveSpeaker(index); });
        }

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            m_ClearBtn.gameObject.SetActive(false);
        }
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
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            var infos = m_texts.text.Split(new Char[] { '|' });
            StartCoroutine(audioBehaiver.ctrl.Downland(infos, (x) => { Debug.Log("下载进度" + x); }));
        }

    }
    private void ActiveSpeaker(int index)
    {
        var speaker = toggles[index].GetComponentInChildren<Text>().text;
        audioBehaiver.ctrl.defultParams.voice_name = speaker;
    }
    void RemoveLocal()
    {
        audioBehaiver.ctrl.CleanUpCatchs();
    }
    void OnError(string err)
    {
        Debug.LogError(err);
    }
    public void Play()
    {
        audioBehaiver.PlayAudio(m_texts.text);
    }
    public void Pause(bool isOn)
    {
        audioBehaiver.TogglePause(isOn);
    }
}

