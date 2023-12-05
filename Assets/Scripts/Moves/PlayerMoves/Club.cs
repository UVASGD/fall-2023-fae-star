using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : Move
{

    new void Awake()
    {
        power = 0.8f;
        critRate = 1.05f;
        enemyMove = false;
        base.Awake();
        keyFrames = new Dictionary<int, float> { { 7, 1 } };
        characterAnimator.SetTrigger("Physical");
    }

    new void Update()
    {
        if(act)
        {
            if (keyFrames.ContainsKey(counter))
            {
                Vector3 enemyPosition = GlobalStateTracker.targetEntity.transform.GetChild(0).position;
                Instantiate(hitAnimation, new Vector3(enemyPosition.x, enemyPosition.y, enemyPosition.z - 1), Quaternion.identity);
            }
            act = false;
        }
        
        base.Update();
    }
}