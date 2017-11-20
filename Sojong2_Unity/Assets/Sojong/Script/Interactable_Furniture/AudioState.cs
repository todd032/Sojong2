using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioState : MonoBehaviour {
    public AudioObject Audio;

    public Text TitleText;
    public Text VolText;
    private void Update()
    {
        TitleText.text = Audio.AudioDataList[Audio.AudioIndex].SongName;
        VolText.text = "Vol." + Audio.Volume.ToString();
    }
}
