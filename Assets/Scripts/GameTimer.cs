using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{

    public TextMeshProUGUI timerText;

    private float timer;

    private void Start()
    {
        timer = 0f;
        UpdateTimerText();
    }

    private void Update()
    {
        if (GameManager.Instance.playerHasDied == false)
        {
            timer += Time.deltaTime;
            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
