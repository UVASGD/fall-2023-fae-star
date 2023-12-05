using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAnimationDamageSignal : MonoBehaviour
{
    public void Damage()
    {
        Move.damage();
    }

    public void EnemyDamage()
    {
        Move.enemyDamage();
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
