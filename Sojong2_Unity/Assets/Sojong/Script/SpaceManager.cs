using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManager {

    public int Width = 1280;
    public int Height = 720;

    private static SpaceManager instance;
    public static SpaceManager Instance
    {
       get
        {
            if(instance == null)
            {
                instance = new SpaceManager();
            }
            return instance;
        }
    }


    public List<Space> SpaceList = new List<Space>();

    protected List<Space> FreeSpaceList = new List<Space>();        
    public void AddSpace(Space _space)
    {
        SpaceList.Add(_space);
    }

    public void UpdateFreeSpace()
    {
        FreeSpaceList.Clear();

        //[x][y]
        bool[,] SpaceMatrix = new bool[Width, Height];
        //List<List<bool>> SpaceMatrix = new List<new List<bool>(1280)>(720);
        
        //check spaces and fill matrix
        for(int iter = 0; iter < SpaceList.Count; iter++)
        {
            Space curspace = SpaceList[iter];
            if(curspace.SpaceType == SpaceType.INTERACTABLE || curspace.SpaceType == SpaceType.NULL)
            {
                //fill matrix
                for(int x = (int)curspace.LeftDownPos.x; x < (int)curspace.LeftDownPos.x + curspace.With; x++)
                {
                    for (int y = (int)curspace.LeftDownPos.y; y < (int)curspace.LeftDownPos.y + curspace.Height; y++)
                    {
                        SpaceMatrix[x,y] = true;
                    }
                }
            }
        }

        //space check
        //check right and then to the top
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if(!SpaceMatrix[x,y])
                {
                    //start adding free space
                    Space freespace = new Space();
                    freespace.SpaceType = SpaceType.FREE;
                    freespace.With = 1;
                    freespace.Height = 0;
                    freespace.LeftDownPos = new Vector2(x, y);
                    
                    for(int spacey = y; spacey < Height ;spacey++)
                    {
                        for(int spacex = x; spacex < Width; spacex++)
                        {
                            if(!SpaceMatrix[spacex,spacey])
                            {
                                //SpaceMatrix[spacex, spacey] = true;
                                if (spacey == y)
                                {
                                    freespace.With = Mathf.Max(spacex - x + 1, freespace.With);
                                }
                            }else
                            {
                                if(spacex - x + 1 > freespace.With)
                                {
                                    //ignore
                                    break;
                                }else
                                {
                                    //cut 
                                    freespace.Height = spacey - y + 1;
                                    break;
                                }
                            }
                        }

                        if (freespace.Height != 0)
                            break;
                    }
                    if(freespace.Height == 0)
                    {
                        freespace.Height = Height - y ;
                    }
                    //spaces set. 
                    //fill matrix
                    for(int freex = 0; freex < freespace.With; freex++)
                    {
                        for(int freey = 0; freey < freespace.Height; freey++)
                        {
                            int w = freex + (int)freespace.LeftDownPos.x;
                            int h = freey + (int)freespace.LeftDownPos.y;
                            if (w >= Width || h >= Height)
                            {
                                Debug.Log("WTF:  w: " + w + " h: " + h + " pos: " +freespace.LeftDownPos);
                            }
                            else
                            {
                                SpaceMatrix[w,h] = true;
                            }
                        }
                    }
                    FreeSpaceList.Add(freespace);
                    if(freespace.Height == 0)
                    {
                        Debug.LogError("error height 0");
                        return;
                    }
                }
            }
        }

        //check spaces
        Debug.Log("total free space: " + FreeSpaceList.Count);
        for(int iter = 0; iter < FreeSpaceList.Count; iter++)
        {
            Space curspace = FreeSpaceList[iter];
            GameObject newgo = new GameObject();
            SpaceObject spaceobj = newgo.AddComponent<SpaceObject>();
            spaceobj.Width = curspace.With;
            spaceobj.Height = curspace.Height;
            spaceobj.LeftBotPos = curspace.LeftDownPos;
            spaceobj.SpaceType = SpaceType.FREE;
        }
    }
}

public enum SpaceType
{
    FREE = 0,
    INTERACTABLE,
    NULL,
    USABLE,
}

public class Space
{
    public SpaceType SpaceType;
    public int With;
    public int Height;
    public Vector2 LeftDownPos;
}
