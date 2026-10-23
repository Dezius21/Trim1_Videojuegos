using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 6f;
    public float fuerzaSalto = 8f;

    private float limiteX;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        limiteX = Camera.main.orthographicSize * Camera.main.aspect;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(h * speed, rb.linearVelocity.y);

        // Salir por un lado y aparecer por el otro
        Vector3 pos = transform.position;
        if (pos.x > limiteX) pos.x = -limiteX;
        else if (pos.x < -limiteX) pos.x = limiteX;
        transform.position = pos;

        if (transform.position.y < Camera.main.transform.position.y - 6f)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }
    }
}