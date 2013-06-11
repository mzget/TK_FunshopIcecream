using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioDescribeManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void PlayOnecSound(AudioClip sound)
    {
        this.audio.Stop();
        this.audio.PlayOneShot(sound);
    }

    public void PlayOnecWithOutStop(AudioClip sound)
    {
        this.audio.PlayOneShot(sound);
    }
}

[System.Serializable]
public class ExtendAudioDescribeData
{
    public Dictionary<string, AudioClip> dict_audios = new Dictionary<string,AudioClip>();

    public ExtendAudioDescribeData() { }
}
