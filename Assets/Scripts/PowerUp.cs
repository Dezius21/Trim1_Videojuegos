using UnityEngine;

public enum TipoPowerUp { SuperSalto, PuntuacionDoble }

public class PowerUp : MonoBehaviour
{

    public TipoPowerUp tipo;


    public Sprite skinSuperSalto;
    public Sprite skinPuntuacionDoble;

    public float multiplicadorSalto = 2f;
    public float duracionDoblePuntuacion = 5f;

    void Start()
    {

        int tipoAleatorio = Random.Range(0, 2);
        tipo = (TipoPowerUp)tipoAleatorio;


        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (tipo == TipoPowerUp.SuperSalto)
        {
            sr.sprite = skinSuperSalto;


            float tamanoDeseado = 0.8f; 
            

            float anchoReal = sr.sprite.bounds.size.x;
            float altoReal = sr.sprite.bounds.size.y;


            transform.localScale = new Vector3(tamanoDeseado / anchoReal, tamanoDeseado / altoReal, 1f);
        }
        else
        {
            sr.sprite = skinPuntuacionDoble;


            float tamanoDeseado = 0.8f; 
            

            float anchoReal = sr.sprite.bounds.size.x;
            float altoReal = sr.sprite.bounds.size.y;


            transform.localScale = new Vector3(tamanoDeseado / anchoReal, tamanoDeseado / altoReal, 1f);
        }
    }

    void Update(){
        if (transform.position.y < Camera.main.transform.position.y - 5.5f)
            Destroy(gameObject);
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