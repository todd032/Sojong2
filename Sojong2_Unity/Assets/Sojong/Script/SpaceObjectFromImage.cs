using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceObjectFromImage : SpaceObject {

    public Image Img;

    private void Awake()
    {
        Width = (int)Img.rectTransform.sizeDelta.x;
        Height = (int)Img.rectTransform.sizeDelta.y;
        LeftBotPos = (Vector2)transform.position - new Vector2(Width / 2f, Height / 2f);
    }
}
