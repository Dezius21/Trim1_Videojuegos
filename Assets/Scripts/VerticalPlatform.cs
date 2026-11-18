using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float rangoMovimiento = 1.5f;
    
    private int direction = 1;
    private float limiteSuperior;
    private float limiteInferior;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;



        limiteSuperior = transform.position.y + rangoMovimiento;
        limiteInferior = transform.position.y - rangoMovimiento;
    }

    void Update()
    {

        if (transform.position.y < mainCamera.transform.position.y - 10f)
        {
            Destroy(gameObject);
        }


        transform.Translate(Vector2.up * direction * speed * Time.deltaTime);


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
