using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public int destroyTime = 3;

    private Color initialColor;

    void Start()
    {
        initialColor = spriteRenderer.color;
        StartCoroutine(FadeOut());
        Destroy(gameObject, destroyTime); // auto détruit après 3 secondes
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DeleteProjectiles")) // évite d'amasser des objets inutiles en dehors de la map
        {
            Destroy(gameObject);
        }
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0;
        Color targetColor = initialColor;
        targetColor.a = 0;

        while (elapsedTime < destroyTime)
        {
            Debug.Log("Shell Fade Out triggered");
            spriteRenderer.color = Color.Lerp(initialColor, targetColor, elapsedTime / destroyTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = targetColor;
    }

}
