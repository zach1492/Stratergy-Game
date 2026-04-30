using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commercial : District
{
    public static int cost = 32;

    public override int returnLevel(int x, int y, Map map)
    {
        double totalPoints = 0;

        foreach (Tile adj in GetAdjacentTiles(x, y, map))
        {
            if (adj != null && adj.district is City && adj.owner == map.tiles[x, y].owner)
            {
                totalPoints += 1;
            }
            if (adj != null && (adj.district is City ||  adj.district is Harbour) && adj.district.building != null && adj.owner == map.tiles[x, y].owner)
            {
                totalPoints += 1;
            }
            if (adj != null && adj.district is Harbour && adj.owner == map.tiles[x, y].owner)
            {
                totalPoints += 3;
            }
            /*if (adj != null && adj.district is City&&HasActivePolicy( "Free market",map.tiles[x, y].owner))
            {
                totalPoints += 1;
            }*/
        }
        return (int)Mathf.Floor((float)totalPoints);
    }

    public override int returnRevenue(int x, int y, Map map)
    {
        int money = returnLevel(x, y, map)*2 ;
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
        if(HasActivePolicy( "Radio",map.tiles[x, y].owner)){
            culture = returnLevel(x, y, map);
        }
        if(building != null)
            culture += building.returnCulture(x, y, map);
        return culture;
    }
}
