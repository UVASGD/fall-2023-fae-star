using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFinish : MonoBehaviour
{
    [SerializeField] GameObject outTransition;
    private DataPersistenceManager dataPersistenceManager;
    private float timer = 0;
    
    void Awake()
    {
        int exp = 0;
        foreach(Enemy e in GlobalEntityStats.enemies)
        {
            exp += e.xp * e.level;
        }
        foreach(Character c in GlobalEntityStats.characters)
        {
            c.totalXP += exp;
            int lvl = c.level;
            c.level = LevelThresholds.determineLevel(c.totalXP);
            int inc = c.level - lvl;
            c.maxHealth += 5 * inc;
            c.maxMana += 3 * inc;
            c.attack += 2 * inc;
            c.magicAttack += 2 * inc;
            c.technique += 3 * inc;
            c.defense += 2 * inc;
            c.magicDefense += 2 * inc;
        }
        dataPersistenceManager = GameObject.Find("SaveDataManager").GetComponent<DataPersistenceManager>();
        dataPersistenceManager.getSaveData().CurrentScene = SceneListings.nextScene[dataPersistenceManager.getSaveData().CurrentScene];
        dataPersistenceManager.SaveGame();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 2f)
        {
            outTransition.SetActive(true);
        }
    }
}
