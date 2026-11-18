using System;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public TMPro.TMP_Text points;
    public TMPro.TMP_Text timeText;


    private Color colorOriginal;




    void Start()
    {

        if (points == null) points = GetComponent<TMPro.TMP_Text>();
        colorOriginal = points.color;
    }


    void Update()
    {

        if (points != null)
        {
            points.text = "Score: " + GameManager.Instance.playerPoints;

            if (GameManager.Instance.isMultiplierActive)
            {
                float velocidadParpadeo = 10f;
                points.color = Mathf.PingPong(Time.time * velocidadParpadeo, 1f) > 0.5f ? Color.yellow : colorOriginal;
            }
            else
            {
                points.color = colorOriginal;
            }
        }


        if (timeText != null)
        {

            int minutos = Mathf.FloorToInt(GameManager.Instance.currentTime / 60F);
            int segundos = Mathf.FloorToInt(GameManager.Instance.currentTime - minutos * 60);
            
            timeText.text = string.Format("Time: {0:00}:{1:00}", minutos, segundos);
        }
    }
}

