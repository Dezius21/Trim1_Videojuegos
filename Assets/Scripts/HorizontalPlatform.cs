using UnityEngine;

public class HorizontalPlatform : MonoBehaviour
{
    public float speed = 1.8f;
    private int direction = 1;
    private float limiteX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        limiteX = (Camera.main.orthographicSize * Camera.main.aspect) - 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < Camera.main.transform.position.y - 5.5f)
            Destroy(gameObject);
        
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
