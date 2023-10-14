using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Move {
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
        none,
    }
    int manaCost;
    ActionTypes actionType;

    //to be implemented for each subclass
    public abstract void invoke();

    public void setManaCost(int manaCostValue) {
        manaCost = manaCostValue;
    }

    public void setActionOnSelect (ActionTypes actionValue) { 
        actionType = actionValue;
    }

}