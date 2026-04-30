using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Policy
{
    public enum PolicyType { Military, Economic, Social, Industrial, Wildcard}

    public string name;
    public PolicyType type;
    public int cost;
    public string description;

    public Policy(string name, PolicyType type, int cost, string description)
    {
        this.name = name;
        this.type = type;
        this.cost = cost;
        this.description = description;
    }
}
