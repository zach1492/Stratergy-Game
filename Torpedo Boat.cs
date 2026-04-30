using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoBoat : Troops
{
    protected override int cost => 40;

    public TorpedoBoat(Player owner) : base(owner)  
    {
        this.name = "TorpedoBoat";
        this.attack = 20;
        this.defence = 8;
        this.movement = 22;
        this.range = 3;
        this.health = 30;
        this.isBoat = true;
    }
}
