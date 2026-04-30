using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GovernmentBuildings
{
        public enum PolicyType { Military, Economic, Social, Industrial, Wildcard}

    public string name;
    public int cost;
    public string description;
    public List<PolicySlot> policySlots = new List<PolicySlot>();

    public GovernmentBuildings(string name, int cost, string description,List<PolicySlot.PolicyType> policySlotTypes = null){
        this.name = name;
        this.cost = cost;
        this.description = description;
        //policySlots.Add(new PolicySlot { type = PolicySlot.PolicyType.Economic });
        if (policySlotTypes != null)
        {
            foreach (var type in policySlotTypes)
                policySlots.Add(new PolicySlot { type = type });
        }
    }

    public void AddPolicySlot(PolicySlot.PolicyType type, int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            policySlots.Add(new PolicySlot { type = type });
        }
    }
}
