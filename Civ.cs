using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civ
{
    public string name;
    public Color primaryColor;
    public Color secondaryColor;

    public Civ(string name, Color primaryColor, Color secondaryColor){
        this.name = name;
        this.primaryColor = primaryColor;
        this.secondaryColor = secondaryColor;
    }
}
