using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Enemy
{
    public Wolf(int level)
    {
        this.name = "Wolf";
        this.level = level;
        this.xp = 5;
        this.sprite = Resources.Load<Sprite>("Sprites/Enemies/Wolf");
        this.maxHealth = level * 5;
        this.attack = (int) (level * 2);
        this.magicAttack = 0;
        this.technique = level;
        this.defense = (int)(level * 0.3f);
        this.magicDefense = 0;
        this.erruptionMeter = 5;
        this.moves = new string[1];
        this.moves[0] = "Wolf Bite";
    }
}
