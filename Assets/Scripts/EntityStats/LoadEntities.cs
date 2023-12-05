using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadEntities : MonoBehaviour
{
    [SerializeField] TextAsset enemyStatFile;

    //[SerializeField] List<GameObject> characterLocations;
    [SerializeField] List<GameObject> characterPortraits;

    [SerializeField] List<Transform> enemyLocations;
    [SerializeField] List<GameObject> enemyPortraits;

    [SerializeField] GameObject enemyPrefab;

    [SerializeField] string[] enemyNames;

    void Awake()
    {
        // Read from the level file
        string[] levelFile = enemyStatFile.text.Split("\n");

        // Spawn Enemies
        GlobalEntityStats.enemies = new List<Enemy>();
        for(int i = 0; i < 4; i++)
        {
            string[] enemyInfo = levelFile[i].Split(":");
            GlobalEntityStats.AddEnemy(enemyInfo[0], int.Parse(enemyInfo[1]));

            GameObject enemy = Instantiate(enemyPrefab, enemyLocations[i].position, Quaternion.identity);
            enemy.transform.SetParent(enemyLocations[i].parent);
            Destroy(enemyLocations[i].gameObject);

            enemy.name = "Enemy" + (i + 1);
            enemy.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GlobalEntityStats.enemies[i].sprite;

            Transform info = enemyPortraits[i].transform.GetChild(2);
            Slider health = info.GetChild(0).GetComponent<Slider>();
            health.maxValue = GlobalEntityStats.enemies[i].maxHealth;
            health.value = health.maxValue;
            TextMeshProUGUI name = info.GetChild(1).GetComponent<TextMeshProUGUI>();
            name.text = GlobalEntityStats.enemies[i].name;
            TextMeshProUGUI level = info.GetChild(2).GetComponent<TextMeshProUGUI>();
            level.text = "LV " + GlobalEntityStats.enemies[i].level;
            TextMeshProUGUI healthNumbers = info.GetChild(3).GetComponent<TextMeshProUGUI>();
            healthNumbers.text = " " + health.value + "/" + health.maxValue;
        }

        // Set the Character stats
        for (int i = 0; i < characterPortraits.Count; i++)
        {
            Transform info = characterPortraits[i].transform.GetChild(2);
            Slider health = info.GetChild(0).GetComponent<Slider>();
            health.maxValue = GlobalEntityStats.characters[i].maxHealth;
            health.value = health.maxValue;
            TextMeshProUGUI healthNumbers = info.GetChild(5).GetComponent<TextMeshProUGUI>();
            healthNumbers.text = " " + health.value + "/" + health.maxValue;
            Slider mana = info.GetChild(1).GetComponent<Slider>();
            mana.maxValue = GlobalEntityStats.characters[i].maxMana;
            mana.value = mana.maxValue;
            TextMeshProUGUI name = info.GetChild(3).GetComponent<TextMeshProUGUI>();
            name.text = GlobalEntityStats.characters[i].name;
            TextMeshProUGUI level = info.GetChild(4).GetComponent<TextMeshProUGUI>();
            level.text = "LV " + GlobalEntityStats.characters[i].level;
        }

        Destroy(this.gameObject);
    }
}
