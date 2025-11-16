using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reroll
{
    public int usePriority;
    public int rerollsLeft;

    public Reroll(int usePriority, int rerollsLeft)
    {
        this.usePriority = usePriority;
        this.rerollsLeft = rerollsLeft;
    }
}
