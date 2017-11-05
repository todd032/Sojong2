using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour {

    public int Width;
    public int Height;

    public Vector2 LeftBotPos;
    public SpaceType SpaceType;

    /*
    private void OnDrawGizmos()
    {
        Color selectedcolor = Color.red;
        if (SpaceType == SpaceType.FREE)
        {
            selectedcolor = Color.blue;
        }else if(SpaceType == SpaceType.INTERACTABLE)
        {
            selectedcolor = new Color(255f / 255f, 165f / 255f, 0f);
        }else if(SpaceType == SpaceType.NULL)
        {
            selectedcolor = new Color(0f, 0f, 0f);
        }

        Gizmos.color = selectedcolor;
        Gizmos.DrawCube(new Vector3(Width / 2 + LeftBotPos.x, Height / 2 + LeftBotPos.y, - 10f), new Vector3(Width - 5f, Height - 5f, 10f));
        //Gizmos.DrawCube(transform.position, new Vector3(Width, Height, 10f));
    }

    private void OnDrawGizmosSelected()
    {
        Color selectedcolor = Color.red;

        Gizmos.color = selectedcolor;
        Gizmos.DrawCube(new Vector3(Width / 2 + LeftBotPos.x, Height / 2 + LeftBotPos.y, -10f), new Vector3(Width, Height, 10f));
    }*/
}
