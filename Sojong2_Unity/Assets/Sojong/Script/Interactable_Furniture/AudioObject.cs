using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour {

    public bool IsPlaying;
    public AudioSource AudioSource;
    public int AudioIndex = 0;
    public List<AudioData> AudioDataList = new List<AudioData>();
    public int Volume = 5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play(bool _play)
    {
        IsPlaying = _play;
        if (IsPlaying)
        {
            AudioSource.Play();
        }else
        {
            AudioSource.Pause();
        }
    }

    public void ChangeAudio(int _add)
    {
        AudioIndex += _add;
        if(AudioIndex >= AudioDataList.Count)
        {
            AudioIndex = 0;
        }else if(AudioIndex < 0)
        {
            AudioIndex = AudioDataList.Count - 1;
        }

        AudioSource.clip = AudioDataList[AudioIndex].Clip;
        AudioSource.Stop();
        Play(IsPlaying);
    }

    public void ChangeVol(int _add)
    {
        Volume += _add;
        Volume = Mathf.Clamp(Volume, 0, 10);
        AudioSource.volume = (float)Volume / 10f;
    }
}

[System.Serializable]
public class AudioData
{
    public string SongName;
    public AudioClip Clip;
}
