using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tech
{
    public string Name;
    private int cost;
    public string description;
    // Store prerequisites as string names
    public List<string> prerequisiteNames;

public int Cost(Player player, Map map)
{
    float finalCost = cost;

    int universityCount = CountUniversities(player, map);
    float universityMultiplier = Mathf.Pow(0.9f, universityCount);
    finalCost *= universityMultiplier;
    
    if (HasActivePolicy("Research Grants", player))
    {
        finalCost = finalCost * 0.8f;
    }

    return Mathf.Max(0,  Mathf.RoundToInt(finalCost));
}
private int CountUniversities(Player player, Map map)
{
    int count = 0;

    for (int x = 0; x < map.width; x++)
    {
        for (int y = 0; y < map.height; y++)
        {
            Tile tile = map.tiles[x, y];
            if (tile.owner == player && tile.building is University)
            {
                count++;
            }
        }
    }

    return count;
}

    public Tech(string name, int cost, List<string> prerequisites = null, string description = "")
    {
        Name = name;
        this.cost = cost;
        this.description = description;
        prerequisiteNames = prerequisites ?? new List<string>();
    }

    // Checks if player has unlocked all prerequisites
    public bool CanUnlock(Player currentPlayer)
    {
        if (prerequisiteNames == null || prerequisiteNames.Count == 0)
            return true;

        foreach (string prereqName in prerequisiteNames)
        {
            bool unlocked = currentPlayer.unlockedTechs.Exists(t => t.Name == prereqName);
            if (!unlocked)
                return false;
        }

        return true;
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
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tech
{
    public string Name;
    public int cost;
    public List<Tech> prerequisites;
    //public bool isUnlocked = false;

    public Tech(string name, int cost)
    {
        Name = name;
        this.cost = cost;
        prerequisites = new List<Tech>();
    }

    public bool CanUnlock(Player currentPlayer)
    {
        if (prerequisites == null || prerequisites.Count == 0)
           return true;

        foreach (Tech t in prerequisites)
        {
            if (!currentPlayer.unlockedTechs.Contains(t)){
                return false;
            }
        }
        return true;
    }
}
*/