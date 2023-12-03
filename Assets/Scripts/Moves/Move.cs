using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public static bool animFinished;

    /*
     * Action Type codex
     * SE = Single Target Enemy Focused Action
     * ME = AOE Target Enemy Focused Action
     * SA = Single Target Ally Focused Action
     * MA = AOE Target Ally Focused Action
     * PB = Personal Buff
     */
    public enum ActionTypes
    {
        SE,
        ME,
        SA,
        MA,
        PB,
        NA,
    }

    protected float power;
    protected float critRate;

    protected void Update()
    {
        Destroy(this.gameObject);
        animFinished = false;
        TransitionManager.ResetFSM();
    }
}