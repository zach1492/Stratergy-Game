using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillary : Troops
{
    protected override int cost => 80;

    public Artillary(Player owner) : base(owner)  
    {
        this.name = "Artillary";
        this.attack = 25;
        this.defence = 3;
        this.movement = 2;
        this.range = 5;
        this.health = 10;
                this.maxHealth = health;

        this.isBoat = false;
    }
}

