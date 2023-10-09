using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public float destroyTime = 1.5f;

    private Color initialColor;

    void Start(){
        initialColor = spriteRenderer.color;
        StartCoroutine(FadeOut());
        Destroy(gameObject, destroyTime);
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.layer == LayerMask.NameToLayer("DeleteProjectiles")){
            Destroy(gameObject);
        }
    }

    IEnumerator FadeOut(){
        float elapsedTime = 0;
        Color targetColor = initialColor;
        targetColor.a = 0;

        while (elapsedTime < destroyTime){
            spriteRenderer.color = Color.Lerp(initialColor, targetColor, elapsedTime / destroyTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = targetColor;
    }

}
