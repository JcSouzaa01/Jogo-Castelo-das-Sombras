using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI healthText;
    public HealthController playerHealth;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateHealthText(); // Atualiza a vida ao iniciar o jogo
    }

    public void UpdateHealthText()
    {
        if (playerHealth != null && healthText != null)
        {
            healthText.text = "Vida: " + playerHealth.GetCurrentHealth();
        }
    }
}
