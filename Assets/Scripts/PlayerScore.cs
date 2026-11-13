using System;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public TMPro.TMP_Text points;
    public TMPro.TMP_Text timeText;

    //Color original
    private Color colorOriginal;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Si no lo asignas en el inspector, lo busca automáticamente
        if (points == null) points = GetComponent<TMPro.TMP_Text>();
        colorOriginal = points.color;
    }

    // Update is called once per frame
    void Update()
    {
        // 1. GESTIÓN DE LOS PUNTOS Y EL PARPADEO
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

        // 2. GESTIÓN DEL CRONÓMETRO EN PANTALLA
        if (timeText != null)
        {
            // Convertimos los segundos brutos a formato de minutos y segundos
            int minutos = Mathf.FloorToInt(GameManager.Instance.currentTime / 60F);
            int segundos = Mathf.FloorToInt(GameManager.Instance.currentTime - minutos * 60);
            
            timeText.text = string.Format("Time: {0:00}:{1:00}", minutos, segundos);
        }
    }
}

