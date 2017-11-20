using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControlObject_Play : AudioControlObject {

    public GameObject PlayingImage;
    public GameObject PauseImage;

    public override void OnClick()
    {
        base.OnClick();
        Audio.Play(!Audio.IsPlaying);

        PlayingImage.gameObject.SetActive(!Audio.IsPlaying);
        PauseImage.gameObject.SetActive(Audio.IsPlaying);
    }
}
