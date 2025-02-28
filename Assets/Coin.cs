using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1; // Quantidade de moedas que essa moeda adiciona

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance != null) // Verifica se o GameManager existe
            {
                GameManager.instance.AddCoin(coinValue);
            }

            Destroy(gameObject); // Destroi a moeda ao coletar
        }
    }
}
