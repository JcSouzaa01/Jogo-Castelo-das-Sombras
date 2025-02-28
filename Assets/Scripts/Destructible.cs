using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject destructionEffect; // Partícula de destruição

    // Método para ser chamado quando o objeto estiver dentro do alcance do ataque
    public void DestroyObject()
    {
        if (destructionEffect != null)
        {
            Instantiate(destructionEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
