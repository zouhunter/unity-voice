using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using Txt2Audio;
using VoiceRSS_SDK;

public class audioTest : MonoBehaviour {
    public Text2AudioCtrl ctrl;
	void Start () {
        ctrl = Text2AudioCtrl.Instance;
        StartCoroutine(ctrl.GetAudioClip("我们在当前的一个U3D项目中使用了StriveEngine作为通信组件与服务端进行通信，在U3D环境中，编译运行一切正常，但在打包发布（Build）为PC版本可执行文件时，却出现错误：“ArgumentException: The Assembly System.Management is referenced by StriveEngine. But the dll is not allowed to be included or could not be found.", OnGet));
    }
    void OnGet(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
	
}
