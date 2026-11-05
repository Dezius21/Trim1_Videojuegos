using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    private int direction = 1;
    private float limiteX;

    public Sprite enemySprite;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemySprite = spriteRenderer.sprite;

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
            spriteRenderer.flipX = true;
            transform.position = new Vector3(limiteX, transform.position.y, transform.position.z);
        }
        else if (transform.position.x  < -limiteX)
        {
            direction = 1;
            spriteRenderer.flipX = false;
            transform.position = new Vector3(-limiteX, transform.position.y, transform.position.z);
        }
    }
}
