using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Govenments 
{
    public enum PolicyType { Military, Economic, Social, Industrial, Wildcard}

    public string name;
    public int cost;
    public string description;

    public int ministrySlots;
    //public Dictionary<PolicyType, int> policySlots = new();
    //public List<Policy> activePolicies = new List<Policy>();
    public List<Ministrys> ministrysPolicies = new List<Ministrys>();
    public List<PolicySlot> policySlots = new List<PolicySlot>();
    public Govenments(string name, int cost, string description, int ministrySlots,List<PolicySlot.PolicyType> policySlotTypes = null)
    {
        this.name = name;
        this.cost = cost;
        this.description = description;
        this.ministrySlots = ministrySlots;//ministrySlots;

        for (int i = 0; i < ministrySlots; i++)
        {
            ministrysPolicies.Add(CreateEmptyMinistry());
        }

        if (policySlotTypes != null)
        {
            foreach (var type in policySlotTypes)
                policySlots.Add(new PolicySlot { type = type });
        }
    }


    public Ministrys CreateEmptyMinistry()
    {
        return new Ministrys(
            "Empty Ministry",
            0,
            "No ministry assigned",
            0
        );
    }

    // Optional helper to add policy slots
    public void AddPolicySlot(PolicySlot.PolicyType type, int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            policySlots.Add(new PolicySlot { type = type });
        }
    }
}
