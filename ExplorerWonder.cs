using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerWonder : Wonder
{
    protected override int cost => 80;
    public ExplorerWonder(Player owner) : base(owner)
    {
    }
}
