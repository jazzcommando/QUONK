using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{

    public int destroyTime = 3;
    
    void Start()
    {
        Destroy(gameObject, destroyTime); // auto détruit après 3 secondes
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DeleteProjectiles")) // évite d'amasser des objets inutiles en dehors de la map
        {
            Destroy(gameObject);
        }
    }

}
