using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _playerRigidbody2D;
    private Animator _playerAnimator;
    public float _playerSpeed;

    private float _playerInitialSpeed;
    private Vector2 _playerDirection;

    private bool _isAttack = false;

    // Dano do jogador
    public int playerDamage = 10;

    // Raio do ataque
    public float attackLength = 2f; // Comprimento do ataque (em direção ao movimento)
    public float attackWidth = 0.5f; // Largura do ataque (largura do retângulo)

    private Vector2 attackDirection = Vector2.right; // Direção inicial do ataque (vai para a direita por padrão)

    // Start é chamado uma vez antes da primeira execução do Update
    void Start()
    {
        _playerRigidbody2D = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();

        _playerInitialSpeed = _playerSpeed;
    }

    void Update()
    {
        OnAttack();
    }

    void FixedUpdate()
    {
        _playerDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Controla a animação de idle e walk
        if (_playerDirection.sqrMagnitude > 0.1) 
        {
            MovePlayer();

            _playerAnimator.SetFloat("AxisX", _playerDirection.x);
            _playerAnimator.SetFloat("AxisY", _playerDirection.y);

            _playerAnimator.SetInteger("Movimento", 1);
        }
        else
        {
            _playerAnimator.SetInteger("Movimento", 0);
        }

        if (_isAttack) // Durante o ataque
        {
            _playerAnimator.SetInteger("Movimento", 2);
        }

        // Verifica a direção em que o jogador está olhando
        if (_playerDirection.x > 0)
        {
            attackDirection = Vector2.right;  // Olhando para a direita
        }
        else if (_playerDirection.x < 0)
        {
            attackDirection = Vector2.left;  // Olhando para a esquerda
        }
        else if (_playerDirection.y > 0)
        {
            attackDirection = Vector2.up;    // Olhando para cima
        }
        else if (_playerDirection.y < 0)
        {
            attackDirection = Vector2.down;  // Olhando para baixo
        }
    }

    void MovePlayer()
    {
        _playerRigidbody2D.MovePosition(_playerRigidbody2D.position + _playerDirection.normalized * _playerSpeed * Time.fixedDeltaTime);
    }

    void OnAttack()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
        {
            _isAttack = true;
            _playerSpeed = 0f;

            // Aplica dano se colidir com o inimigo durante o ataque
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(transform.position + (Vector3)attackDirection * attackLength / 2f, new Vector2(attackLength, attackWidth), 0f);
            foreach (var enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    SlimeController slimeController = enemy.GetComponent<SlimeController>();
                    if (slimeController != null)
                    {
                        slimeController.TakeDamage(playerDamage); // Aplica o dano no inimigo
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetMouseButtonUp(0))
        {
            _isAttack = false;
            _playerSpeed = _playerInitialSpeed;
        }
    }

    // Método para desenhar o retângulo de detecção de ataque no editor
    void OnDrawGizmos()
    {
        if (_isAttack) // Só desenha o retângulo quando o ataque está ativo
        {
            Gizmos.color = Color.red; // Cor do retângulo de ataque

            Vector2 attackOrigin = transform.position; // Origem do retângulo de ataque

            // Ajustando a origem dependendo da direção de ataque
            if (attackDirection == Vector2.right)
                attackOrigin = new Vector2(transform.position.x + attackLength , transform.position.y / 2f);
            else if (attackDirection == Vector2.left)
                attackOrigin = new Vector2(transform.position.x - attackLength , transform.position.y / 2f);
            else if (attackDirection == Vector2.up)
                attackOrigin = new Vector2(transform.position.x, transform.position.y + attackLength);
            else if (attackDirection == Vector2.down)
                attackOrigin = new Vector2(transform.position.x, transform.position.y - attackLength / 2f);

            // Desenha um retângulo com a largura e comprimento de ataque, com a origem ajustada
            Gizmos.DrawWireCube(attackOrigin, new Vector2(attackLength, attackWidth));
        }
    }
}
