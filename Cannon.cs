using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Troops
{
    protected override int cost => 30;

    public Cannon(Player owner) : base(owner)  
    {
        this.name = "Cannon";
        this.attack = 6;
        this.defence = 2;
        this.movement = 2;
        this.range = 4;
        this.health = 15;
        this.maxHealth = health;

        this.isBoat = false;
    }
}
