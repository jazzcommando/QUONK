using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        QuonkController player = other.GetComponent<QuonkController>();

        if (player != null)
        {
            player.Die();
        }
    }
}