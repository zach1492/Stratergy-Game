using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine : Troops
{
    protected override int cost => 80;

    public Submarine(Player owner) : base(owner)  
    {
        this.name = "Submarine";
        this.attack = 20;
        this.defence = 10;
        this.movement = 18;
        this.range = 2;
        this.health = 60;
                this.maxHealth = health;

        this.isBoat = true;
    }
}
