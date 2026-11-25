using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class WFWall : MonoBehaviour
{
    public Tilemap vineWall;

    public void RemoveWall()
    {
        if (vineWall != null)
        {
            vineWall.gameObject.SetActive(false);
        }
    }

    public void RestoreWall()
    {
        vineWall.gameObject.SetActive(true);
    }
}
