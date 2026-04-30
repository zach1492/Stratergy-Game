using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musketman : Troops
{
    protected override int cost => 20;

    public Musketman(Player owner) : base(owner)  
    {
        this.name = "Musketman";
        this.attack = 5;
        this.defence = 2;
        this.movement = 2;
        this.range = 2;
        this.health = 12;
        this.maxHealth = health;

        this.isBoat = false;
    }
}
