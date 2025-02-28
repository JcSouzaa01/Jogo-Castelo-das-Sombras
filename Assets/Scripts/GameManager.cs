using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public HealthController playerHealth;
    public int coinCount = 0;
    public TMP_Text coinText;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayerTakeDamage(int damage, Vector2 knockbackDirection)
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage, knockbackDirection);
        }
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateCoinUI();
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "Moedas: " + coinCount.ToString();
        }
    }
}
