using System;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public TMPro.TMP_Text points;

    //Color original
    private Color colorOriginal;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        points = GetComponent<TMPro.TMP_Text>();
        colorOriginal = points.color;
    }

    // Update is called once per frame
    void Update()
    {
        points.text = "Score: " + GameManager.Instance.playerPoints;

        //Manejo del parpadeo
        if (GameManager.Instance.isMultiplierActive)
        {
            float velocidadParpadeo = 10f;

            points.color = Mathf.PingPong(Time.time * velocidadParpadeo, 1f) > 1f ? Color.yellow : colorOriginal;
        }
        else
        {
            points.color = colorOriginal;
        }
    }
}

