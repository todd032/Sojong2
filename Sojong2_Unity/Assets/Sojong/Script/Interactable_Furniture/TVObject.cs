using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVObject : MonoBehaviour {

    public VideoPlayer Player;
    public List<VideoClip> ClipList = new List<VideoClip>();

    public GameObject PowerObject;
    public TextMesh VolText;
    public AudioSource Audio;

    public bool Power;
    public int Volume;
    public int Ch;
    
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        voltimer -= Time.deltaTime;
        if(voltimer < 0f)
        {
            VolText.gameObject.SetActive(false);
        }
	}

    public void PowerOn(bool _flag)
    {
        Power = _flag;
        PowerObject.SetActive(Power);
    }

    float voltimer = 0f;
    public void AddVolume(int _val)
    {
        Volume += _val;
        Volume = Mathf.Clamp(Volume, 0, 10);
        Audio.volume = Volume / 10f;
        voltimer = 1f;
        VolText.gameObject.SetActive(true);
        VolText.text = "VOL " + Volume.ToString();
    }

    public void ChangeChanel(int _val)
    {
        Ch += _val;
        if(Ch >= ClipList.Count)
        {
            Ch = 0;
        }
        if(Ch < 0)
        {
            Ch = ClipList.Count - 1;
        }
        Player.clip = ClipList[Ch];
        //Player.Stop();
        Player.Play();
    }
}
