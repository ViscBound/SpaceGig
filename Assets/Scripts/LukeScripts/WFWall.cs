using UnityEngine;
using UnityEngine.Tilemaps;

public class WFWall : MonoBehaviour
{
    public Tilemap vineWall;

    public void RemoveWall()
    {
        if (vineWall != null)
        {
            vineWall.ClearAllTiles();
        }
    }
}
