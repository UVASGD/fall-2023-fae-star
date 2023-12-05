using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEntityStats
{
    public static List<Character> characters = new List<Character>
    {
        new Character("Leoht", 20, 25, 6, 8, 4, 6, 9, 0, 5, 0),
        new Character("Assassin", 22, 15, 9, 3, 8, 5, 4, 0, 5, 0),
        new Character("Hacker", 27, 30, 3, 8, 12, 5, 6, 0, 5, 0),
        new Character("Knight", 35, 10, 9, 4, 5, 12, 5, 0, 5, 0),
    };
    public static List<GameObject> characterObjects;

    public static List<Enemy> enemies = new List<Enemy>();
    public static GameObject[] enemyObjects;


    // This basically serves as a list of enemy types...
    public static void AddEnemy(string name, int level)
    {
        switch(name)
        {
            case "Wolf":
                enemies.Add(new Wolf(level));
                break;
            default:
                enemies.Add(new Wolf(level));
                break;
        }
    }

    private static float lowBound = 0.85f;
    private static float highBound = 1.15f;

    public static int damageCalcAttackCharacter(int character, float attackPower, bool critical)
    {
        // Please just ignore the fact that I am calling the transition manager to get a list of enemies. I know it makes no fucking sense but I am too tired to fix it
        float defense = enemies[Array.IndexOf(TransitionManager.enemies, GlobalStateTracker.targetEntity)].defense;
        float baseValue = characters[character].attack * 2 * attackPower - defense;

        if (critical)
        {
            baseValue *= 2;
        }

        return (int) (baseValue * UnityEngine.Random.Range(lowBound, highBound));
    }

    public static int damageCalcAttackEnemy(int enemy, float attackPower, bool critical)
    {
        // Please just ignore the fact that I am calling the transition manager to get a list of enemies. I know it makes no fucking sense but I am too tired to fix it
        float defense = characters[TransitionManager.characters.IndexOf(GlobalStateTracker.targetEntity)].defense;
        float baseValue = enemies[enemy].attack * 2 * attackPower - defense;

        if (critical)
        {
            baseValue *= 2;
        }

        return (int)(baseValue * UnityEngine.Random.Range(lowBound, highBound));
    }
}
