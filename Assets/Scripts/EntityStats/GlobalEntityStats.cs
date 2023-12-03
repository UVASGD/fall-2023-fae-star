using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEntityStats
{
    public static List<Character> characters = new List<Character>
    {
        new Character("Leoht", 20, 25, 6, 8, 4, 6, 0, 5, 0),
        new Character("Assassin", 22, 15, 9, 3, 8, 5, 0, 5, 0),
        new Character("Hacker", 27, 30, 3, 8, 12, 5, 0, 5, 0),
        new Character("Knight", 35, 10, 9, 4, 5, 12, 0, 5, 0),
    };

    public static List<Enemy> enemies = new List<Enemy>();


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

    public static int damageCalcAttack(int character, float attackPower, float critRate)
    {
        float baseValue = characters[character].attack * attackPower;
        float critChance = characters[character].technique * critRate / 2;

        if(Random.Range(0, 100) < critChance)
        {
            baseValue *= 2;
        }

        return (int) (baseValue * Random.Range(0.8f, 1.2f));
    }
}
