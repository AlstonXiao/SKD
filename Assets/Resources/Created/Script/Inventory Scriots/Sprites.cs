using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// This class is used to for generating icons for the inventory
/// </para>
/// Author: Roland, Yan Xiao<para/>
/// Attached object: Static class<para/>
/// updated: 3/15/2020: increase performance by caching the sprites<para/>
/// </summary>
public static class Sprites
{
    private static Sprite cuttedSprite = Resources.Load<Sprite>("Created/Icons/CutableIcon");
    private static Sprite pickUpSprite = Resources.Load<Sprite>("Created/Icons/PickUpIcon");

    /// <summary>
    /// This will return the sprite we want. 
    /// 0 for cutted, 1 for pickup
    /// </summary>
    /// <param name="index">0 for cutted, 1 for pickup</param>
    /// <returns></returns>
    public static Sprite getSprite(int index)
    {
        if (index == 0) return cuttedSprite;
        if (index == 1) return pickUpSprite;
        return null;
    }
}
