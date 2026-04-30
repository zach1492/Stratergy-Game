using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyScrapers : Building
{
    public static int cost = 40;

    public override int returnRevenue(int x, int y, Map map)
    {
        int money = 0; 
        if (map.tiles[x, y].district != null)
        {
            money = map.tiles[x, y].district.returnLevel(x, y, map);
        }
        return money;
    }
}
