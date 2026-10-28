using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public TMPro.TMP_Text points;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        points = GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        points.text = "Score: " + GameManager.Instance.playerPoints;
    }
}

