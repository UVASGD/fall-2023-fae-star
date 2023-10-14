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
    private int manaCost;
    private ActionTypes actionType;

    public void Move(int manaCost, ActionTypes actionType) {
        this.manaCost = manaCost;
        this.actionType = actionType;
    }

    //to be implemented for each subclass
    public abstract void invoke();

    public int getManaCost() {
        return manaCost;
    }

    public ActionTypes getActionType() { 
        return actionType;
    }

}