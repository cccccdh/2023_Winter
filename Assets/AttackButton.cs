using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    public void Attack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        AmmoShoot shoot = player.GetComponent<AmmoShoot>();
        shoot.Attack();
    }
}
