using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatProcessingPlant : Building
{
    public static int cost = 40;

    public override int returnRevenue(int x, int y, Map map)
    {
        int money = 0; 

        foreach (Tile adj in GetAdjacentTiles(x, y, map))
        {
            if (adj != null && adj.building is Pasture && adj.owner == map.tiles[x, y].owner)
            {
                money += 4; 
            }
        }

        return money;
    }
}
