using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Character
{
    public string name;
    public int maxHealth;
    public int maxMana;
    public int attack;
    public int magicAttack;
    public int technique;
    public int defense;
    public int magicDefense;
    public int affinity;
    public int level;
    public int totalXP;

    public Character(string name,
                    int maxHealth,
                    int maxMana,
                    int attack,
                    int magicAttack,
                    int technique,
                    int defense,
                    int magicDefense,
                    int affinity,
                    int level,
                    int totalXP)
    {
        this.name = name;
        this.maxHealth = maxHealth;
        this.maxMana = maxMana;
        this.attack = attack;
        this.magicAttack = magicAttack;
        this.technique = technique;
        this.defense = defense;
        this.magicDefense = magicDefense;
        this.affinity = affinity;
        this.level = level;
        this.totalXP = totalXP;
    }
}
