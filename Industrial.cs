using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Industrial : District
{
    public static int cost = 35;

    public override int returnLevel(int x, int y, Map map)
    {
        double totalPoints = 0;

        foreach (Tile adj in GetAdjacentTiles(x, y, map))
        {
            if(adj.owner == map.tiles[x, y].owner){
            if (adj != null && (adj.building is Mine ||  adj.building is Quarry)&& adj.owner == map.tiles[x, y].owner)
            {
                totalPoints += 1;
            }
            if (adj != null && (adj.building is Sawmill ||  adj.building is Forge|| adj.building is Windmill)&& adj.owner == map.tiles[x, y].owner)
            {
                totalPoints += 5;
            }
            if (adj != null && adj.district is Harbour)
            {
                totalPoints += 5;
            }
            if (adj != null && adj.district is City&&HasActivePolicy( "Child labour",map.tiles[x, y].owner))
            {
                totalPoints += 3;
            }
            }
        }
        return (int)Mathf.Floor((float)totalPoints);
    }

    public override int returnRevenue(int x, int y, Map map)
    {
        int money = returnLevel(x, y, map);
        if (map.tiles[x, y].owner.currentGovernment != null &&
    map.tiles[x, y].owner.currentGovernment.name == "Oligarchy")
{
    money *= 2;
}
        if(building != null)
            money += building.returnRevenue(x, y, map);
        return money;
    }
    public override int returnCulture(int x, int y, Map map){
        int culture = 0;
        if(HasActivePolicy( "Workers Rights",map.tiles[x, y].owner)) culture = returnLevel(x, y, map);
        if(building != null)
            culture += building.returnCulture(x, y, map);
        return culture;
    }
}
