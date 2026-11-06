using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 8f;
    private float limitX;
    private bool isGrounded = true;
    private float lastScoredY = float.NegativeInfinity;

    private float maxAlturaY = 0f;

    public Sprite jumpSprite;
    private Sprite defaultSprite;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Collider2D col;

    private bool dead;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        limitX = Camera.main.orthographicSize * Camera.main.aspect;

        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetScore();

            // Si hay checkpoint, reaparecemos justo encima de esa plataforma
            if (GameManager.Instance.hasCheckpoint)
            {
                Vector2 cp = GameManager.Instance.checkpointPos;
                float spawnY = cp.y + 1f;

                transform.position = new Vector3(cp.x, spawnY, 0f);
                maxAlturaY = spawnY; // para no sumar puntos de altura desde 0

                Vector3 camPos = Camera.main.transform.position;
                Camera.main.transform.position = new Vector3(camPos.x, spawnY, camPos.z);
            }
        }
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

        if (!isGrounded && jumpSprite != null)
        {
            spriteRenderer.sprite = jumpSprite;
        }

        // Wrap around the screen sides
        Vector3 pos = transform.position;
        if (pos.x > limitX) pos.x = -limitX;
        else if (pos.x < -limitX) pos.x = limitX;
        transform.position = pos;

        // --- SISTEMA DE PUNTUACIÓN POR ALTURA MAXIMA ---
        if (transform.position.y > maxAlturaY)
        {
            float diferenciaAltura = transform.position.y - maxAlturaY;

            // Usamos FloorToInt para asegurar que solo cobramos puntos enteros completos
            int puntosASumar = Mathf.FloorToInt(diferenciaAltura * 10f);

            if (puntosASumar > 0)
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.SumarPuntosPorDistancia(puntosASumar);
                }

                // ¡LA CLAVE! Solo avanzamos maxAlturaY la cantidad exacta de altura 
                // por la que ya hemos recibido puntos (ej. 2 puntos = 0.2f de altura).
                // Los decimales sobrantes se quedan acumulados para el siguiente frame.
                maxAlturaY += (puntosASumar / 10f);
            }
        }

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

        // Si hemos aterrizado en una plataforma checkpoint, la activamos
        CheckpointPlatform cp = collision.gameObject.GetComponentInParent<CheckpointPlatform>();
        if (cp != null) cp.Activate();

        isGrounded = true;
        spriteRenderer.sprite = defaultSprite;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }

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

    // --- MODIFICADO: Hace de puente hacia el GameManager ---
    public void ActivarPuntuacionDoble(float duracion)
    {
        if (GameManager.Instance != null)
        {
            // Le avisamos al GameManager para que encienda el efecto x2 y el color amarillo
            GameManager.Instance.ActivarMultiplicador(duracion);
        }
    }

    void Die()
    {
        if (dead) return;
        dead = true;
        SceneManager.LoadScene("GameOver");
    }
}