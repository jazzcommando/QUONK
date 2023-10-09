using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPresenceIndicator : MonoBehaviour
{
    public Transform pivotPoint;
    public SpriteRenderer indicatorRenderer;

    public float rotationRadius = 5f;
    
    private void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        bool anyEnemyVisible = false;
        foreach (GameObject enemy in enemies)
        {
            if (IsObjectVisible(enemy))
            {
                anyEnemyVisible = true;
            }
        }

        if (anyEnemyVisible)
        {
            SetArrowVisibility(false); // Hide the arrow
        }
        else
        {
            GameObject nearestEnemy = FindNearestEnemy();

            if (nearestEnemy != null){
                RotateTowardsNearestEnemy(nearestEnemy);
                SetArrowVisibility(true); // Show the arrow
                indicatorRenderer.enabled = true;
            }
            else{
                SetArrowVisibility(false); // Hide the arrow
                indicatorRenderer.enabled = false;
            }
        }
    }

    private bool IsObjectVisible(GameObject obj)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(obj.transform.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1 && screenPoint.z > 0;
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(pivotPoint.position, enemy.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private void RotateTowardsNearestEnemy(GameObject nearestEnemy)
    {
        Vector3 direction = nearestEnemy.transform.position - pivotPoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = pivotPoint.position + Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right * rotationRadius;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void SetArrowVisibility(bool isVisible)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isVisible);
        }
    }
}
