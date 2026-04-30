using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : Troops
{
    protected override int cost => 80;

    public Artillery(Player owner) : base(owner)  
    {
        this.name = "Artillery";
        this.attack = 25;
        this.defence = 3;
        this.movement = 2;
        this.range = 5;
        this.health = 10;
                this.maxHealth = health;

        this.isBoat = false;
    }
}
