using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 8f;
    private float limitX;


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


            if (GameManager.Instance.hasCheckpoint)
            {
                Vector2 cp = GameManager.Instance.checkpointPos;
                float spawnY = cp.y + 1f;

                transform.position = new Vector3(cp.x, spawnY, 0f);
                maxAlturaY = spawnY;

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


        if (jumpSprite != null)
        {
            spriteRenderer.sprite = rb.linearVelocity.y > 0.01f ? jumpSprite : defaultSprite;
        }


        Vector3 pos = transform.position;
        if (pos.x > limitX) pos.x = -limitX;
        else if (pos.x < -limitX) pos.x = limitX;
        transform.position = pos;


        if (transform.position.y > maxAlturaY)
        {
            float diferenciaAltura = transform.position.y - maxAlturaY;


            int puntosASumar = Mathf.FloorToInt(diferenciaAltura * 10f);

            if (puntosASumar > 0)
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.SumarPuntosPorDistancia(puntosASumar);
                }




                maxAlturaY += (puntosASumar / 10f);
            }
        }


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


        CheckpointPlatform cp = collision.gameObject.GetComponentInParent<CheckpointPlatform>();
        if (cp != null) cp.Activate();


        spriteRenderer.sprite = defaultSprite;
    }









    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Die();
        }
        else if (collider.CompareTag("Goal"))
            Win(collider.transform.root.position);
    }

    public void AplicarSuperSalto(float multiplicador)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * (jumpForce * multiplicador), ForceMode2D.Impulse);
    }


    public void ActivarPuntuacionDoble(float duracion)
    {
        if (GameManager.Instance != null)
        {

            GameManager.Instance.ActivarMultiplicador(duracion);
        }
    }

    void Die()
    {
        if (dead) return;
        dead = true;
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isTimerRunning = false;
            GameManager.Instance.GuardarDatosPartida();
        }

        SceneManager.LoadScene("GameOver");
    }

    void Win(Vector2 metaPos)
    {
        if (dead) return;
        dead = true;
        if (GameManager.Instance != null)
            GameManager.Instance.Victoria(metaPos);

        SceneManager.LoadScene("Victory");
    }
}