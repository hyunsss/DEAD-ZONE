using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    ZombieStateMachineConroller controller;
    public Collider collider; 

    private void Awake() {
        collider = GetComponent<Collider>();
        controller = GetComponentInParent<ZombieStateMachineConroller>();
    }

   

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerHit")) {
            Debug.Log("Player HIt!!!!!");
            DamageByPartComponent damageByPart = other.collider.GetComponentInParent<DamageByPartComponent>();

            damageByPart.TakeDamage(damageByPart.part_cols[other.collider], controller.damage, PlayerManager.death.Death, () => PlayerManager.move.animator.SetTrigger("Hit"));
        }
    }

}
