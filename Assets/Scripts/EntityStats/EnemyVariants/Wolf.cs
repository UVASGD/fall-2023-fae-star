using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Enemy
{
    public Wolf(int level)
    {
        this.name = "Wolf";
        this.level = level;
        this.sprite = Resources.Load<Sprite>("Sprites/Enemies/Wolf");
        this.maxHealth = level * 5;
        this.attack = (int) (level * 0.5f);
        this.magicAttack = 0;
        this.technique = level;
        this.attack = (int)(level * 0.3f);
        this.erruptionMeter = 5;
    }
}
