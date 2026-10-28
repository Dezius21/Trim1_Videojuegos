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
        }
        else
        {
            sr.sprite = skinPuntuacionDoble;
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