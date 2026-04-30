using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civic
{
    public string name;
    private int cost;

    // Store prerequisites as string names
    public List<string> prerequisiteNames;

    public List<Policy> unlockedPolicies = new();
    public List<Govenments> unlockedGovernments = new();
    public List<Ministrys> unlockedMinistrys = new();
    public List<GovernmentBuildings> unlockedGovernmentBuildings = new();

public int Cost(Player player)
{
    int finalCost = cost;

    if (HasActivePolicy("Education", player))
    {
        finalCost = Mathf.RoundToInt(finalCost * 0.8f);
    }

    return Mathf.Max(0, finalCost);
}
    public Civic(string name, int cost, List<string> prerequisites = null, List<Policy> policies = null, List<Govenments> governments = null, List<Ministrys> ministrys = null, List<GovernmentBuildings> governmentBuildings = null)
    {
    this.name = name;
    this.cost = cost;
    this.prerequisiteNames = prerequisites ?? new List<string>();

    // Initialize unlocked lists safely
    this.unlockedPolicies = policies ?? new List<Policy>();
    this.unlockedGovernments = governments ?? new List<Govenments>();
    this.unlockedMinistrys = ministrys ?? new List<Ministrys>();
    this.unlockedGovernmentBuildings = governmentBuildings ?? new List<GovernmentBuildings>();

    }

    // Checks if player has unlocked all prerequisites
    public bool CanUnlock(Player currentPlayer)
    {
        if (prerequisiteNames == null || prerequisiteNames.Count == 0)
            return true;

        foreach (string prereqName in prerequisiteNames)
        {
            bool unlocked = currentPlayer.unlockedCivics.Exists(t => t.name == prereqName);
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
