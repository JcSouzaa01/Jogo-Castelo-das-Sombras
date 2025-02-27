using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public HealthController playerHealth;

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
}
