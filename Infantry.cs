using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infantry : Troops
{
    protected override int cost => 35;

    public Infantry(Player owner) : base(owner)  
    {
        this.name = "Infantry";
        this.attack = 5;
        this.defence = 2;
        this.movement = 2;
        this.range = 2;
        this.health = 12;
        this.maxHealth = health;

        this.isBoat = false;
    }
}
