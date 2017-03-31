using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using Txt2Audio;
using VoiceRSS_SDK;

public class audioTest : MonoBehaviour {
    public Txt2AudioCtrl ctrl;
	void Start () {
        ctrl = Txt2AudioCtrl.Instance;
        Debug.Log("开始请求：" + Time.time);
        StartCoroutine(ctrl.GetAudioClip("我们在当前的一个U3D项目中使用了StriveEngine作为通信组件与服务端进行通信，在U3D环境中，编译运行一切正常，但在打包发布（Build）为PC版本可执行文件时，却出现错误：“ArgumentException: The Assembly System.Management is referenced by StriveEngine. But the dll is not allowed to be included or could not be found.", OnGet));
        StartCoroutine(ctrl.GetAudioClip("们", OnGet));

    }
    void OnGet(AudioClip clip)
    {
        Debug.Log("等到数据：" + Time.time);

        GetComponent<AudioSource>().PlayOneShot(clip);
    }
	
}
