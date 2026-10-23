using UnityEngine;

public class Plataform : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y < Camera.main.transform.position.y - 5.5f)
            Destroy(gameObject);
    }
}