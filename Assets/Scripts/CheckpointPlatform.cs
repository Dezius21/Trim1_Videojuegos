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


        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.color = colorActivado;
    }
}