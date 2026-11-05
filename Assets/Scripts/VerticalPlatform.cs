using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float rangoMovimiento = 1.5f; // El espacio que subirá y bajará desde su punto inicial
    
    private int direction = 1;
    private float limiteSuperior;
    private float limiteInferior;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        // Calculamos los límites fijando el rango de movimiento vertical
        // a partir de la posición Y donde se instanció la plataforma
        limiteSuperior = transform.position.y + rangoMovimiento;
        limiteInferior = transform.position.y - rangoMovimiento;
    }

    void Update()
    {
        // Destrucción si la cámara la deja muy atrás
        if (transform.position.y < mainCamera.transform.position.y - 5.5f)
        {
            Destroy(gameObject);
            return;
        }

        // Movimiento vertical (Vector2.up en lugar de Vector2.right)
        transform.Translate(Vector2.up * direction * speed * Time.deltaTime);

        // Control de rebote utilizando los límites verticales establecidos
        if (transform.position.y > limiteSuperior)
        {
            direction = -1;
            transform.position = new Vector3(transform.position.x, limiteSuperior, transform.position.z);
        }
        else if (transform.position.y < limiteInferior)
        {
            direction = 1;
            transform.position = new Vector3(transform.position.x, limiteInferior, transform.position.z);
        }
    }
}
