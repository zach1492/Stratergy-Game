using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicySlot
{
    public enum PolicyType { Military, Economic, Social, Industrial, Wildcard}
    public PolicyType type;
    public Policy activePolicy;
}
