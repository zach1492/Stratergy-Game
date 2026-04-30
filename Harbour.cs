using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harbour : District
{
    public static int cost = 20;

    public override int returnLevel(int x, int y, Map map)
    {
        double totalPoints = 0;

        foreach (Tile adj in GetAdjacentTiles(x, y, map))
        {
            if (adj != null && adj.building is WhalingShip && adj.owner == map.tiles[x, y].owner)
            {
                totalPoints += 2;
            }
            if (adj != null && adj.building is FishingBoats && adj.owner == map.tiles[x, y].owner)
            {
                totalPoints += 2;
            }
        }


        return (int)Mathf.Floor((float)totalPoints);
    }

    public override int returnRevenue(int x, int y, Map map)
    {
        int money = returnLevel(x, y, map);
        if(HasActivePolicy("Tariffs",map.tiles[x, y].owner)==true){
            money*=2;
        }
        if(building != null)
            money += building.returnRevenue(x,y,map);
        return money;
    }
    public override int returnCulture(int x, int y, Map map){
        int culture = 0;
        if(building != null)
            culture += building.returnCulture();
        return culture;
    }
}
