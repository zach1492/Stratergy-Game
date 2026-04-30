using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string tribeType;
    public int money;
    public int culture;
    public bool isPlayer;
    public bool[,] exploredTiles; 
    public Color playerColor;
    public Color SecondaryColor;
    public List<Tech> unlockedTechs;
    public Govenments currentGovernment;
    public List<Policy> unlockedPolicys;
    public List<Govenments> unlockedGovernments;
    public List<Ministrys> unlockedMinistrys;
    public List<GovernmentBuildings> unlockedGovernmentBuildings;
    public List<Civic> unlockedCivics;
    public bool popWonderBuilt;
    public bool tradeWonderBuilt;
    public bool expWonderBuilt;
    public bool popWonderUnlocked;
    public bool tradeWonderUnlocked;
    public bool expWonderUnlocked;
    public int cultureMade;
    public int moneyMade;
    public int enemeysKilled;


    public Player(string tribeType, int money, bool isPlayer, Color color, Color SecondaryColor, int culture){
        this.tribeType=tribeType;
        this.money=money;
        this.isPlayer=isPlayer;
        this.playerColor = color;
        this.SecondaryColor = SecondaryColor;
        unlockedTechs = new List<Tech>();
        unlockedCivics= new List<Civic>();
        this.culture = culture;
        unlockedPolicys = new List<Policy>();
        unlockedGovernments = new List<Govenments>();
        unlockedMinistrys = new List<Ministrys>();
        unlockedGovernmentBuildings = new List<GovernmentBuildings>();
        if(isPlayer == true){
        currentGovernment = new Govenments("Chieftain", 0, "Starter government with 1 slot of each type", 1);
        // Add 1 slot of each type
        currentGovernment.AddPolicySlot(PolicySlot.PolicyType.Military, 1);
        currentGovernment.AddPolicySlot(PolicySlot.PolicyType.Economic, 1);
        currentGovernment.AddPolicySlot(PolicySlot.PolicyType.Social, 1);
        currentGovernment.AddPolicySlot(PolicySlot.PolicyType.Industrial, 1);
        }else{
            currentGovernment = new Govenments("AI Government", 0, "Starter government with 1 slot of each type", 1);
            currentGovernment.AddPolicySlot(PolicySlot.PolicyType.Military, 1);
            currentGovernment.AddPolicySlot(PolicySlot.PolicyType.Military, 1);
            currentGovernment.AddPolicySlot(PolicySlot.PolicyType.Military, 1);
        currentGovernment.AddPolicySlot(PolicySlot.PolicyType.Economic, 1);

        }
        this.popWonderBuilt = false;
        this.tradeWonderBuilt = false;
        this.expWonderBuilt = false;

            popWonderUnlocked = false;
    tradeWonderUnlocked =false ;
    expWonderUnlocked = false;
    this.cultureMade=3;
    this.moneyMade=4;
    this.enemeysKilled=0;
    

    }

    public void intializeClouds(int height, int width){
        exploredTiles = new bool[width, height];

    }
}
