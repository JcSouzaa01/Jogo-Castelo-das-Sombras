using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Rigidbody2D _rb;
    private bool isKnockedBack = false; // Para evitar múltiplos knockbacks ao mesmo tempo

    public float knockbackForce = 5f; // Força do knockback
    public float knockbackDuration = 0.2f; // Duração do knockback

    void Start()
    {
        currentHealth = maxHealth;
        UIManager.instance.UpdateHealthText();  // Atualiza a vida na UI assim que o jogo começa
        _rb = GetComponent<Rigidbody2D>(); // Obtém o Rigidbody2D do player
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        currentHealth -= damage;
        UIManager.instance.UpdateHealthText(); // Atualiza o texto da vida
        Debug.Log(gameObject.name + " tomou " + damage + " de dano. Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(ApplyKnockback(knockbackDirection));
        }
    }

    private IEnumerator ApplyKnockback(Vector2 knockbackDirection)
    {
        if (isKnockedBack) yield break; // Evita múltiplos knockbacks ao mesmo tempo

        isKnockedBack = true;
        _rb.linearVelocity = Vector2.zero; // Zera a velocidade antes do knockback
        _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); // Aplica o impulso

        yield return new WaitForSeconds(knockbackDuration); // Espera um tempo

        _rb.linearVelocity = Vector2.zero; // Para o movimento do knockback
        isKnockedBack = false;
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " morreu!");
        Destroy(gameObject); // Remove o objeto da cena
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
