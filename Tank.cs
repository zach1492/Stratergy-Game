using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Troops
{
    protected override int cost =>100;

    public Tank(Player owner) : base(owner)  
    {
        this.name = "Tank";
        this.attack = 7;
        this.defence = 5;
        this.movement = 1;
        this.range = 2;
        this.health = 40;
        this.maxHealth = health;

        this.isBoat = false;
    }
}
