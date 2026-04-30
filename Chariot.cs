using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chariot : Troops
{
    protected override int cost => 15;

    public Chariot(Player owner) : base(owner)  
    {
        this.name = "Chariot";
        this.attack = 4;
        this.defence = 3;
        this.movement = 4;
        this.range = 1;
        this.health = 15;
        this.maxHealth = health;

        this.isBoat = false;
    }
}
