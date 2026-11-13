using UnityEngine;

public class AspectKeeper : MonoBehaviour
{
    void Start()
    {
        // El ratio objetivo que quieres (9:16 = 0.5625)
        float targetaspect = 9.0f / 16.0f;

        // Obtener el ratio actual de la pantalla del jugador
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // Calcular la diferencia de escala
        float scalewidth = windowaspect / targetaspect;

        Camera camera = GetComponent<Camera>();

        // Si la pantalla es más ancha que 9:16 (caso habitual de monitores PC)
        if (scalewidth < 1.0f)
        {
            Rect rect = camera.rect;
            rect.width = 1.0f;
            rect.height = scalewidth;
            rect.x = 0;
            rect.y = (1.0f - scalewidth) / 2.0f;
            camera.rect = rect;
        }
        else // Si la pantalla es más alta
        {
            float scaleheight = 1.0f / scalewidth;
            Rect rect = camera.rect;
            rect.width = scaleheight;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleheight) / 2.0f;
            rect.y = 0;
            camera.rect = rect;
        }
    }
}
