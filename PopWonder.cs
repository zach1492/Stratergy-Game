using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopWonder: Wonder
{
    protected override int cost => 40;
    public PopWonder(Player owner) : base(owner)
    {

    }
    public override int returnRevenue(int x, int y, Map map)
    {
        if(typeOwner.tribeType == "Rome"){
        int money = 0; 

            foreach (Tile adj in GetAdjacentTiles(x, y, map))
            {
                if (adj != null && adj.district !=null && adj.owner == map.tiles[x, y].owner)
                {
                    money += 5;
                }
            }

            return money;
        }
        return 0;
    }
}
