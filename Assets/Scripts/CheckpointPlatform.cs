using UnityEngine;

public class CheckpointPlatform : MonoBehaviour
{
    public Color colorActivado = Color.green;
    private bool activado = false;

    public void Activate()
    {
        if (activado || GameManager.Instance == null) return;
        activado = true;

        GameManager.Instance.SetCheckpoint(transform.position, GameManager.Instance.playerPoints);

        // Feedback visual para que el jugador sepa que se ha guardado
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.color = colorActivado;
    }
}