using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public string tileType;
    public string owner1;
    public Player owner;
    public District district;
    public Building building;
    public Troops unit;
    public bool isMoveable;
    public bool isAttackable;
    public bool isSelected;
    public bool isTroopSelected;
    public int x;
    public int y;
    //public int movementCost;
    public int maxCost;
    public string forestResource;
    public string resource;
    public bool hasWall;
    public bool hasRoad;
    public bool hasTrainTrack;
    public bool hasTradeRoute;
    public Wonder wonder;

    public Tile(string tileType, Player owner, int x, int y, string reasource, string reasource1){
        this.tileType = tileType;
        this.owner = owner;
        this.building = null;
        this.unit = null;
        isMoveable = false;
        isAttackable = false;
        this.x = x;
        this.y = y;
        this.maxCost = 1;
        //this.movementCost = Random.Range(1, 1);
        this.forestResource = reasource;
        this.resource = reasource1;
        this.hasWall = false;
        this.hasRoad=false;
        this.hasTrainTrack=false;
        this.wonder = null;
    }
    public float movementCost
    {
        get
        {
            //if ((tileType == "Ocean" || tileType == "Coast"))
            //    return 1;
                        if (hasTrainTrack) return 0;
            if (hasRoad) return 0.5f;

            if (tileType == "Mountain") return 2;
            if(tileType == "River") return 2;
            if (forestResource != null && forestResource != "") return 2; 
                return 1;
        }
    }

    public int defenceBonus(Player player){
        if(hasWall&&owner == player) return 2;
        if(building != null&&building is Fort) return 2;
        if(tileType == "District"&&owner == player) return 1;
        if (tileType == "Mountain" &&HasActivePolicy("Discipline",player)) return 1;
        if (forestResource != "" &&HasActivePolicy("Martial Tradition",player)) return 1;
        if (tileType == "Snow" &&HasActivePolicy("Skis",player)) return 1;
        if ((tileType == "Coast" || tileType == "River")&&HasActivePolicy("Press gangs",player)) return 1;
        if ((tileType == "Ocean")&&HasActivePolicy("Admirals",player)) return 1;
        return 0;
    }

    protected bool HasActivePolicy(string policyName, Player currentPlayer)
    {
        // Government slots
        foreach (var slot in currentPlayer.currentGovernment.policySlots)
            if (slot.activePolicy != null && slot.activePolicy.name == policyName)
                return true;

        // Ministry slots
        foreach (var ministry in currentPlayer.currentGovernment.ministrysPolicies)
            foreach (var slot in ministry.policySlots)
                if (slot.activePolicy != null && slot.activePolicy.name == policyName)
                    return true;

        // Government building slots
        foreach (var ministry in currentPlayer.currentGovernment.ministrysPolicies)
            foreach (var building in ministry.activeGovernmentBuildings)
                foreach (var slot in building.policySlots)
                    if (slot.activePolicy != null && slot.activePolicy.name == policyName)
                        return true;

        return false;
    }
}
