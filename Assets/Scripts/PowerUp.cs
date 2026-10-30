using UnityEngine;

public enum TipoPowerUp { SuperSalto, PuntuacionDoble }

public class PowerUp : MonoBehaviour
{
    // El tipo se asignará solo, ya no necesitas cambiarlo en el Inspector
    public TipoPowerUp tipo;

    // Variables para asignar las imágenes desde Unity
    public Sprite skinSuperSalto;
    public Sprite skinPuntuacionDoble;

    public float multiplicadorSalto = 2f;
    public float duracionDoblePuntuacion = 5f;

    void Start()
    {
        // Elige un número al azar: 0 (SuperSalto) o 1 (PuntuacionDoble)
        int tipoAleatorio = Random.Range(0, 2);
        tipo = (TipoPowerUp)tipoAleatorio;

        // Obtiene el componente que dibuja la imagen y la cambia
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (tipo == TipoPowerUp.SuperSalto)
        {
            sr.sprite = skinSuperSalto;
              // --- NUEVO: FORZAR TAMAÑO FIJO ---
            // Define el tamaño que ocupará en el mundo (ej. 0.8 unidades de Unity)
            float tamanoDeseado = 0.8f; 
            
            // Obtenemos las dimensiones físicas del sprite recién asignado
            float anchoReal = sr.sprite.bounds.size.x;
            float altoReal = sr.sprite.bounds.size.y;

            // Calculamos y aplicamos la escala exacta para llegar al tamaño deseado
            transform.localScale = new Vector3(tamanoDeseado / anchoReal, tamanoDeseado / altoReal, 1f);
        }
        else
        {
            sr.sprite = skinPuntuacionDoble;
            // --- NUEVO: FORZAR TAMAÑO FIJO ---
            // Define el tamaño que ocupará en el mundo (ej. 0.8 unidades de Unity)
            float tamanoDeseado = 0.8f; 
            
            // Obtenemos las dimensiones físicas del sprite recién asignado
            float anchoReal = sr.sprite.bounds.size.x;
            float altoReal = sr.sprite.bounds.size.y;

            // Calculamos y aplicamos la escala exacta para llegar al tamaño deseado
            transform.localScale = new Vector3(tamanoDeseado / anchoReal, tamanoDeseado / altoReal, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Player jugador = collider.GetComponent<Player>();
            if (jugador != null)
            {
                switch (tipo)
                {
                    case TipoPowerUp.SuperSalto:
                        jugador.AplicarSuperSalto(multiplicadorSalto);
                        break;
                    case TipoPowerUp.PuntuacionDoble:
                        jugador.ActivarPuntuacionDoble(duracionDoblePuntuacion);
                        break;
                }
            }
            Destroy(gameObject);
        }
    }
}