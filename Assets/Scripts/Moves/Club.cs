using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Club : Move
{
    private Animator characterAnimator;

    void Awake()
    {
        power = 0.8f;
        critRate = 1.05f;
        characterAnimator = GlobalStateTracker.currentEntity.transform.GetChild(0).GetComponent<Animator>();
        characterAnimator.SetTrigger("Physical");
    }

    new void Update()
    {
        if(animFinished)
        {
            Transform info = TransitionManager.portraits[GlobalStateTracker.targetEntity].transform.Find("EntityInfo");
            Slider health = info.GetChild(0).GetComponent<Slider>();
            health.value = Math.Max(0, health.value - GlobalEntityStats.damageCalcAttack(0, power, critRate));
            base.Update();
        }
    }
}