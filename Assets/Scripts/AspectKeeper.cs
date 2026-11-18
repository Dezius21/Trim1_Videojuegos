using UnityEngine;

public class AspectKeeper : MonoBehaviour
{
    void Start()
    {

        float targetaspect = 9.0f / 16.0f;


        float windowaspect = (float)Screen.width / (float)Screen.height;


        float scalewidth = windowaspect / targetaspect;

        Camera camera = GetComponent<Camera>();


        if (scalewidth < 1.0f)
        {
            Rect rect = camera.rect;
            rect.width = 1.0f;
            rect.height = scalewidth;
            rect.x = 0;
            rect.y = (1.0f - scalewidth) / 2.0f;
            camera.rect = rect;
        }
        else
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
