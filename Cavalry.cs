using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cavalry : Troops
{
    protected override int cost => 35;

    public Cavalry(Player owner) : base(owner)  
    {
        this.name = "Cavalry";
        this.attack = 5;
        this.defence = 3;
        this.movement = 6;
        this.range = 2;
        this.health = 15;
        this.maxHealth = health;

        this.isBoat = false;
    }
}
