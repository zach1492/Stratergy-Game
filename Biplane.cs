using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biplane : Troops
{
    protected override int cost => 60;

    public Biplane(Player owner) : base(owner)  
    {
        this.name = "Biplane";
        this.attack = 6;
        this.defence = 3;
        this.movement = 15;
        this.range = 2;
        this.health = 20;
                this.maxHealth = health;

        this.isBoat = false;
        this.isAir = true;
    }
}
