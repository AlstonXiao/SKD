using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Sprites
{

    public static Sprite getSprite(int index)
    {
        if (index == 0) return Resources.Load<Sprite>("Created/Icons/CutableIcon");
        if (index == 1) return Resources.Load<Sprite>("Created/Icons/PickUpIcon");
        return null;
    }
}
