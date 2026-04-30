using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeWonder : Wonder
{
    protected override int cost => 60;
    public TradeWonder(Player owner) : base(owner)
    {
    }
}
