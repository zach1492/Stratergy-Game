using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Wonder
{
    protected abstract int cost { get; }
    public Player owner;
    public Player typeOwner;
    public Wonder(Player owner){
        this.owner = owner;
        this.typeOwner = owner;
    }
    public int Cost
    {
        get
        {
            int finalCost = cost;

            if(owner.currentGovernment.name == "Monarchy"){
                finalCost /= 2;
            }
            return Mathf.Max(0, finalCost);
        }
    }
    public virtual int returnRevenue(int x, int y, Map map)
    {
        return 0;
    }
    protected List<Tile> GetAdjacentTiles(int x, int y, Map map)
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
    }
}
