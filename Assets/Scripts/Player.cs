using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 8f;
    private float limitX;
    private bool isGrounded = true;
    private float lastScoredY = float.NegativeInfinity;

    public Sprite jumpSprite;
    private Sprite defaultSprite;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Collider2D col;

    private bool dead;


    // --- VARIABLE PARA POWER-UPS ---
    public bool puntuacionDobleActiva = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        limitX = Camera.main.orthographicSize * Camera.main.aspect;

        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;


        if (GameManager.Instance != null)
            GameManager.Instance.ResetScore();

    }

    

    void Update()
    {
        if (dead) return;

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
            Die();
        }
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Platform")) return;
        if (rb.linearVelocity.y > 0.01f) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        float platY = collision.transform.position.y;
        if (platY > lastScoredY + 0.1f)
        {
            lastScoredY = platY;
            GameManager.Instance.IncreasePlayerPointsHeigth();
        }

        isGrounded = true;
        spriteRenderer.sprite = defaultSprite;
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
        if (collider.CompareTag("Enemy"))
        {
            Die();
        }
    }
    public void AplicarSuperSalto(float multiplicador)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * (jumpForce * multiplicador), ForceMode2D.Impulse);
    }

    public void ActivarPuntuacionDoble(float duracion)
    {
        StartCoroutine(RutinaPuntuacionDoble(duracion));
    }

    private System.Collections.IEnumerator RutinaPuntuacionDoble(float duracion)
    {
        puntuacionDobleActiva = true;
        yield return new WaitForSeconds(duracion);
        puntuacionDobleActiva = false;
    }
    void Die()
    {
        if (dead) return;
        dead = true;
        SceneManager.LoadScene("GameOver");
    }
}
