using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpentryWorkshop : Building
{
    public static int cost = 28;

    public override int returnCulture(int x, int y, Map map)
    {
        int culture = 0; 

        foreach (Tile adj in GetAdjacentTiles(x, y, map))
        {
            if (adj != null && adj.building is LumberHut && adj.owner == map.tiles[x, y].owner)
            {
                culture += 2; 
            }
        }

        return culture;
    }
}
