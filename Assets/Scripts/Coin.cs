using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool picked = false;

    void Update()
    {
        if (transform.position.y < Camera.main.transform.position.y - 10f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(picked)return;

        if (collider.CompareTag("Player"))
        {
            picked = true;
            GameManager.Instance.IncreasePlayerPointsCoins();
            Destroy(gameObject);
        }
    }
}
