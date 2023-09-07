using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log("Collided with death trigger");

        QuonkController player = GetComponent<QuonkController>();
        if (player != null)
        {
            player.Die();
        }

    }
}