using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public enum StatusName { Might, Focus, Haste, Clarity, Barrier, Frailty, Slowed, Confusion, Exposed};
    public StatusName statusName;
    public int statusDuration = 0;
    public int statusCurrentDuration = 0;
}
