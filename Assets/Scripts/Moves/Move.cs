using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Move {

    // For now I am going to hard code this, but we might want to change it sometime in the future.
    // The setup for this list is (int, Action, int) => (Mana Cost, ActionType, Action on Select, Position in Selection List)
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

    abstract static void invoke() {
        //to be implemented for each subclass
    }
    protected static void setManaCost(int manaCostValue) {
        manaCost = manaCostValue;
    }


    protected static void setActionOnSelect (Action actionValue) { 
        actionType = actionValue;
    }

}