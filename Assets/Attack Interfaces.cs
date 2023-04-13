using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public bool takeDamage(float damage, GameObject attacker);
}

public interface IAttacker
{

}