using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : Troops
{
    protected override int cost => 60;

    public MachineGun(Player owner) : base(owner)  
    {
        this.name = "MachineGun";
        this.attack = 6;
        this.defence = 3;
        this.movement = 2;
        this.range = 2;
        this.health = 10;
        this.maxHealth = health;

        this.isBoat = false;
    }
}
