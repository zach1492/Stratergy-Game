using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zeppelin : Troops
{
    protected override int cost =>  100;

    public Zeppelin(Player owner) : base(owner)  
    {
        this.name = "Zeppelin";
        this.attack = 6;
        this.defence = 1;
        this.movement = 12;
        this.range = 1;
        this.health = 120;
        this.maxHealth = health;

        this.isBoat = false;
        this.isAir = true;
    }
}
