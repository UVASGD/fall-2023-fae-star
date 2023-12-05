using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public string name;
    public Sprite sprite;
    public int maxHealth;
    public int attack;
    public int magicAttack;
    public int technique;
    public int defense;
    public int magicDefense;
    public int erruptionMeter;
    public int level;

    public string[] moves;

    public void act()
    {
        // Do Nothing
    }
}
