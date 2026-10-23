using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 8f;

    public Sprite jumpSprite;
    private Sprite defaultSprite;
    private bool isGrounded = true;
    private SpriteRenderer spriteRenderer;

    private float limitX;
    private Rigidbody2D rb;
    private Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        limitX = Camera.main.orthographicSize * Camera.main.aspect;

        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(h * speed, rb.linearVelocity.y);

        if (h > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (h < 0)
        {
            spriteRenderer.flipX = true;
        }

        // CAMBIAR-------------------------------------------------------------------
        if (!isGrounded && jumpSprite != null)
        {
            spriteRenderer.sprite = jumpSprite;
        }
        //-----------------------------------------------------------------------

        // Wrap around the screen sides
        Vector3 pos = transform.position;
        if (pos.x > limitX) pos.x = -limitX;
        else if (pos.x < -limitX) pos.x = limitX;
        transform.position = pos;

        // Die if falling below the camera view
        if (transform.position.y < Camera.main.transform.position.y - 6f)
        {
            SceneManager.LoadScene("Start");
        }
    }

     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Platform")) return;

        if (rb.linearVelocity.y > 0.01f) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        //CAMBIAR ------------------------------------------------------------------------------------------

        // --- RESTAURAR SPRITE AL TOCAR PLATAFORMA ---
        isGrounded = true;
        spriteRenderer.sprite = defaultSprite; // Vuelve a ponerse el sprite normal
    }

    // --- ¡NUEVO MÉTODO AÑADIDO! ---
    // En cuanto el jugador se separe de la plataforma (comience el salto), 
    // activamos la alerta de que ya no está tocando el suelo.
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }
    //---------------------------------------------------------------------------------------------------------


    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Comprobamos si el objeto que hemos tocado tiene la etiqueta "Enemy"
        if (collider.CompareTag("Enemy"))
        {
            // Reinicia la escena al igual que cuando el jugador cae al vac�o
            UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
        }
    }
}