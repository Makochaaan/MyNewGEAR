using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPFoundation : MonoBehaviour
{
    public int maxHP, currentHP;
    
    public virtual void Damage(int damage)
    {
        currentHP -= damage;
        if(currentHP <= 0)
        {
            OnHPZero();
        }
    }
    public virtual void OnHPZero()
    {
        Debug.Log("hp zero from base class");
    }
}
