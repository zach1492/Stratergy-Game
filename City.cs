using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : District
{
    private static int cost = 16;

public int Cost(Player player)
{
    int finalCost = cost;

    if (HasActivePolicy("Immigration", player))
    {
        finalCost = Mathf.RoundToInt(finalCost * 0.5f);
    }

    return Mathf.Max(0, finalCost);
}
    public override int returnLevel(int x, int y, Map map)
    {
        double totalFood = 0;

        foreach (Tile adj in GetAdjacentTiles(x, y, map))
        {
            if (adj != null && adj.building != null && adj.owner == map.tiles[x, y].owner)
            {
                totalFood += adj.building.returnFood(x, y, map);
                if(adj.district!=null && adj.district.building !=null){
                    totalFood += adj.district.building.returnFood(x, y, map);
                }
            }
        }
        if(building != null)
            totalFood += building.returnFood(x, y, map);

        return (int)Mathf.Floor((float)totalFood);
    }

    public override int returnRevenue(int x, int y, Map map)
    {

        int money = returnLevel(x, y, map);
        if (map.tiles[x, y].owner.currentGovernment != null &&map.tiles[x, y].owner.currentGovernment.name == "Theocracy")
        {
            money += 2;
        }
        if(building != null){
            money += building.returnRevenue(x, y, map);
            if(HasActivePolicy("Subway",map.tiles[x, y].owner)){
                money +=2;
            }
        }
        return money;
    }

    public override int returnCulture(int x, int y, Map map){
        int culture = returnLevel(x, y, map)/2;
        if (map.tiles[x, y].owner.currentGovernment != null &&map.tiles[x, y].owner.currentGovernment.name == "Theocracy")
        {
            culture += 2;
        }
        if(HasActivePolicy("Literature",map.tiles[x, y].owner)){
            culture += 1;
        }
        if(building != null){
            culture += building.returnCulture(x,y,map);
            if(HasActivePolicy("Subway",map.tiles[x, y].owner)){
                culture += 1;
            }
            /*if(HasActivePolicy("Sculptures",map.tiles[x, y].owner) && (building is Monument||building is Palace)){
                culture += 2;
            }*/
        }
        return culture;
    }
    /*private List<Tile> GetAdjacentTiles(int x, int y, Map map)
    {
        List<Tile> list = new List<Tile>();

        int[,] evenOffsets =
        {
            { +1,  0 },
            {  0, +1 },
            { -1, +1 },
            { -1,  0 },
            { -1, -1 },
            {  0, -1 }
        };

        int[,] oddOffsets =
        {
            { +1,  0 },
            { +1, +1 },
            {  0, +1 },
            { -1,  0 },
            {  0, -1 },
            { +1, -1 }
        };

        int[,] offsets = (y % 2 == 0) ? evenOffsets : oddOffsets;

        for (int i = 0; i < 6; i++)
        {
            int nx = x + offsets[i, 0];
            int ny = y + offsets[i, 1];

            if (nx >= 0 && ny >= 0 && nx < map.width && ny < map.height)
            {
                list.Add(map.tiles[nx, ny]);
            }
        }

        return list;
    }*/
}