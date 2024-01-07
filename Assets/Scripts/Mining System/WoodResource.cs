using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodResource : MineableResource
{
    /// <summary>
    /// number of hits to chop or get wood
    /// </summary>
    public int hitsToChop = 5;

    public override void OnInteract()
    {
        Debug.Log("This function is called from the wood resource");
    }
}
