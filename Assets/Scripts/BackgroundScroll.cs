using UnityEngine;

public class FondoScroll : MonoBehaviour
{
    [Range(0f, 1f)]
    public float factor = 0.3f;   

    private Renderer rend;
    private Transform cam;
    private float alturaVista;

    void Start()
    {
        rend = GetComponent<Renderer>();
        cam = Camera.main.transform;
        alturaVista = Camera.main.orthographicSize * 2f;
    }

    void LateUpdate()
    {
        float offsetY = cam.position.y / alturaVista * factor;
        rend.material.mainTextureOffset = new Vector2(0f, offsetY);
    }
}
