using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    private int direction = 1;
    private float limiteX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        limiteX = Camera.main.orthographicSize * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
        
        if ( transform.position.x > limiteX)
        {
            direction = -1 ;
        }
        else if (transform.position.x  < -limiteX)
        {
            direction = 1;
        }
    }
}
