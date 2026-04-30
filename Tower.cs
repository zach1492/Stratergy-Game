using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building
{
    public static int cost = 20;

    public override int returnRevenue(int x, int y, Map map)
    {
        int money = 0; 

        foreach (Tile adj in GetAdjacentTiles(x, y, map))
        {
            if (adj != null  && adj.owner == map.tiles[x, y].owner && adj.district is City && adj.district !=null)
            {
                money += 2; 
            }
        }

        return money;
    }
}
