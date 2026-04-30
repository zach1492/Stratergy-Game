using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class District
{
    public Building building;

    public abstract int returnRevenue(int x, int y, Map map);

    public abstract int returnLevel(int x, int y, Map map);

    public abstract int returnCulture(int x, int y, Map map);

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

    protected bool HasActivePolicy(string policyName, Player currentPlayer)
    {
        // Government slots
        foreach (var slot in currentPlayer.currentGovernment.policySlots)
            if (slot.activePolicy != null && slot.activePolicy.name == policyName)
                return true;

        // Ministry slots
        foreach (var ministry in currentPlayer.currentGovernment.ministrysPolicies)
            foreach (var slot in ministry.policySlots)
                if (slot.activePolicy != null && slot.activePolicy.name == policyName)
                    return true;

        // Government building slots
        foreach (var ministry in currentPlayer.currentGovernment.ministrysPolicies)
            foreach (var building in ministry.activeGovernmentBuildings)
                foreach (var slot in building.policySlots)
                    if (slot.activePolicy != null && slot.activePolicy.name == policyName)
                        return true;

        return false;
    }
}
