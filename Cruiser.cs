using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cruiser : Troops
{
    protected override int cost => 150;

    public Cruiser(Player owner) : base(owner)  
    {
        this.name = "Cruiser";
        this.attack = 18;
        this.defence = 12;
        this.movement = 18;
        this.range = 2;
        this.health = 80;
        this.maxHealth = health;

        this.isBoat = true;
    }
}
