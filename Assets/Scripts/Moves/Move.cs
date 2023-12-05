using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Move : MonoBehaviour
{
    public static bool animFinished;
    public static int counter;
    public static bool spawnDamageNumbers;
    protected static bool act;
    protected Dictionary<int, float> keyFrames;

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
        NA,
    }

    protected static int character;
    protected static int enemy;
    protected static int damageVal;
    protected static float power;
    protected static float critRate;
    protected static bool enemyMove;
    protected static bool critical;
    protected Animator characterAnimator;
    [SerializeField] protected GameObject hitAnimation;
    [SerializeField] protected GameObject displayNumbers;

    protected void Awake()
    {
        characterAnimator = GlobalStateTracker.currentEntity.transform.GetChild(0).GetComponent<Animator>();
        if (!enemyMove)
        {
            character = TransitionManager.characters.IndexOf(GlobalStateTracker.currentEntity);
            enemy = Array.IndexOf(TransitionManager.enemies, GlobalStateTracker.targetEntity);
        }
        else
        {
            enemy = Array.IndexOf(TransitionManager.enemies, GlobalStateTracker.currentEntity);
            character = TransitionManager.characters.IndexOf(GlobalStateTracker.targetEntity);
        }
        critical = UnityEngine.Random.Range(0, 100) < GlobalEntityStats.characters[character].technique * critRate / 2;
    }

    protected void Update()
    {
        if (animFinished)
        { 
            Destroy(this.gameObject);
            animFinished = false;
            counter = 0;
            if (!enemyMove)
                TransitionManager.ResetFSM();
            else
                EnemyDance.startReverse();
        }

        if(spawnDamageNumbers)
        {
            Vector3 enemyPosition = GlobalStateTracker.targetEntity.transform.GetChild(0).position;
            GameObject damageObject = Instantiate(displayNumbers, new Vector3(enemyPosition.x, enemyPosition.y, enemyPosition.z - 2), Quaternion.identity);
            DamageNumberAnimation damageScript = damageObject.GetComponent<DamageNumberAnimation>();
            damageScript.critical = critical;
            damageScript.value = damageVal;
            spawnDamageNumbers = false;
        }
    }

    public static void keyFrameCall()
    {
        counter++;
        act = true;
    }

    public static void damage()
    {
        // This check could've been dealt with better but oh well....
        if (enemyMove)
            return;
        Transform info = TransitionManager.portraits[GlobalStateTracker.targetEntity].transform.Find("EntityInfo");
        Slider health = info.GetChild(0).GetComponent<Slider>();
        TextMeshProUGUI healthValues = info.GetChild(3).GetComponent<TextMeshProUGUI>();
        damageVal = Math.Max(0, GlobalEntityStats.damageCalcAttackCharacter(character, power, critical));
        health.value = Math.Max(0, health.value - damageVal);
        if(health.value == 0)
        {
            TransitionManager.setLock(6, 10 + enemy + 1, true);
        }
        healthValues.text = health.value + "/" + health.maxValue;
        spawnDamageNumbers = true;
    }

    public static void enemyDamage()
    {
        // This check could've been dealt with better but oh well....
        if (!enemyMove)
            return;
        Transform info = TransitionManager.portraits[GlobalStateTracker.targetEntity].transform.Find("EntityInfo");
        Slider health = info.GetChild(0).GetComponent<Slider>();
        TextMeshProUGUI healthValues = info.GetChild(5).GetComponent<TextMeshProUGUI>();
        damageVal = Math.Max(0, GlobalEntityStats.damageCalcAttackEnemy(enemy, power, critical));
        health.value = Math.Max(0, health.value - damageVal);
        if (health.value == 0)
        {
            TransitionManager.setLock(0, 10 + character + 1, true);
        }
        healthValues.text = health.value + "/" + health.maxValue;
        spawnDamageNumbers = true;
    }
}