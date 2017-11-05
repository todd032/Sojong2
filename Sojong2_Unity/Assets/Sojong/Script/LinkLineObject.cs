using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkLineObject : MonoBehaviour {

    public RectTransform MyRectTransform;
    public ParticleSystem MyParticle;

    public Vector2 StartPos;
    public Vector2 EndPos;

    private void Start()
    {
        //SetLine(StartPos, EndPos);
    }

    public void SetLine(Vector2 _startpos, Vector2 _endpos)
    {
        float distance = Vector2.Distance(_startpos, _endpos);
        float angle = Constant.GetAngle(Vector2.right, _endpos - _startpos);

        MyRectTransform.eulerAngles = Vector3.forward * angle;
        Vector3 newpos = _startpos + (_endpos - _startpos) / 2f;
        newpos.z = -50f;
        transform.position = newpos;

        Vector2 newsize= MyRectTransform.sizeDelta;
        newsize.x = distance ;
        MyRectTransform.sizeDelta = newsize;

        ParticleSystem.ShapeModule shape = MyParticle.shape;
        shape.radius = distance / 2f;
    }
}
