using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dreadnort : Troops
{
    protected override int cost => 200;

    public Dreadnort(Player owner) : base(owner)  
    {
        this.name = "Dreadnort";
        this.attack = 20;
        this.defence = 15;
        this.movement = 16;
        this.range = 3;
        this.health = 120;
        this.maxHealth = health;

        this.isBoat = true;
    }
}
