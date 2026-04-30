using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ministrys
{
    public enum PolicyType { Military, Economic, Social, Industrial, Wildcard}

    public string name;
    public int cost;
    public string description;
    public List<PolicySlot> policySlots = new List<PolicySlot>();


    public int buildingSlots;
    public List<GovernmentBuildings> activeGovernmentBuildings = new List<GovernmentBuildings>();
    
    public Ministrys(string ministryName, int cost, string description, int buildingSlots,List<PolicySlot.PolicyType> policySlotTypes = null)
    {
        this.name = ministryName;
        this.cost = cost;
        this.description = description;
        this.buildingSlots = buildingSlots;
        
        //policySlots.Add(new PolicySlot { type = PolicySlot.PolicyType.Military });
        //policySlots.Add(new PolicySlot { type = PolicySlot.PolicyType.Economic });

        activeGovernmentBuildings = new List<GovernmentBuildings>();
        for (int i = 0; i < buildingSlots; i++)
        {
            activeGovernmentBuildings.Add(CreateEmptyGovenmentBuildings());
        }
        if (policySlotTypes != null)
        {
            foreach (var type in policySlotTypes)
                policySlots.Add(new PolicySlot { type = type });
        }
    }


    private GovernmentBuildings CreateEmptyGovenmentBuildings()
    {
        return new GovernmentBuildings(
            "Empty Building",
            0,
            "No Building assigned"
        );
    }

    public void AddPolicySlot(PolicySlot.PolicyType type, int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            policySlots.Add(new PolicySlot { type = type });
        }
    }
}
 