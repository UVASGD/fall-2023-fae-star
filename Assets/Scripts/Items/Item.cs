using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    /*
     * Item Type codex
     * SE = Single Target Enemy Focused Item
     * ME = AOE Target Enemy Focused Item
     * SA = Single Target Ally Focused Item
     * MA = AOE Target Ally Focused Item
     * PB = Personal Buff
     */
    public enum ActionTypes
    {
        SE,
        ME,
        SA,
        MA,
        PB,
    }

    // Instance variables
    private int amount;
    private ActionTypes actionType;

    public Item(int amount, ActionTypes actionType) {
        this.amount = amount;
        this.actionType = actionType;
    }

    //to be implemented for each subclass
    public abstract void invoke();

    public int getAmount() {
        return amount;
    }

    public ActionTypes getActionType() { 
        return actionType;
    }

}