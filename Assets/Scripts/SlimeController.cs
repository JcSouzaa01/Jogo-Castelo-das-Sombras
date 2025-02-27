using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public float _moveSpeedSlime = 3.5f;
    private Vector2 _slimeDirection;
    private Rigidbody2D _slimeRB2D;
    public DetectionController _detectionaArea;
    private SpriteRenderer _spriteRenderer;
    public int damage = 10; // Dano do inimigo

    void Start()
    {
        _slimeRB2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (_detectionaArea.detectedObjs.Count > 0)
        {
            _slimeDirection = (_detectionaArea.detectedObjs[0].transform.position - transform.position).normalized;
            _slimeRB2D.MovePosition(_slimeRB2D.position + _slimeDirection * _moveSpeedSlime * Time.fixedDeltaTime);

            if(_slimeDirection.x > 0)
            {
                _spriteRenderer.flipX = false;
            }
            else if (_slimeDirection.x < 0)
            {
                _spriteRenderer.flipX = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            GameManager.instance.PlayerTakeDamage(10, knockbackDirection); //passa a direção corretamente
        }
    }


}
