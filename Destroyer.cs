using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : Troops
{
    protected override int cost => 200;

    public Destroyer(Player owner) : base(owner)  
    {
        this.name = "Destroyer";
        this.attack = 35;
        this.defence = 5;
        this.movement = 20;
        this.range = 2;
        this.health = 70;
        this.isBoat = true;
    }
}
