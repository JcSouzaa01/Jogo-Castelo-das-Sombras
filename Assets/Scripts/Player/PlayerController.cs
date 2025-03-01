using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _playerRigidbody2D;
    private Animator _playerAnimator;
    public float _playerSpeed;

    private float _playerInitialSpeed;
    private Vector2 _playerDirection;
    
    public int playerDamage = 10;
    private BoxCollider2D attackCollider;

    private Vector2 attackDirection = Vector2.right;

    [Header("Attack Settings")]
    public float attackDuration = 0.3f;
    public float attackCooldown = 0.5f;
    private bool _canAttack = true;
    private bool _isAttacking = false;

    [Header("Dash Settings")]
    public float dashForce = 20f;          // Força do impulso
    public float dashDuration = 0.15f;     // Tempo de duração do dash
    public float dashCooldown = 0.4f;      // Tempo de espera entre os dashes
    private bool _isDashing = false;
    private bool _canDash = true;
    private Vector2 _dashDirection;        // Direção específica para o dash

    void Start()
    {
        _playerRigidbody2D = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _playerInitialSpeed = _playerSpeed;

        attackCollider = gameObject.AddComponent<BoxCollider2D>();
        attackCollider.isTrigger = true;
        attackCollider.enabled = false;
        attackCollider.size = new Vector2(0.84f, 1.16f);

        _playerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void Update()
    {
        
        HandleAttackInput();
        HandleDashInput(); // Movido para Update para melhor resposta
    }

    void FixedUpdate()
    {
        if (!_isAttacking && !_isDashing)
        {
            HandleMovement();
            UpdateAttackDirection();
        }
    }

    void UpdateAttackDirection()
    {
        if (_playerDirection.x > 0)
            attackDirection = Vector2.right;
        else if (_playerDirection.x < 0)
            attackDirection = Vector2.left;
        else if (_playerDirection.y > 0)
            attackDirection = Vector2.up;
        else if (_playerDirection.y < 0)
            attackDirection = Vector2.down;
    }

    void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _canDash && !_isAttacking)
        {
            // Captura a direção atual do input
            Vector2 inputDirection = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );

            // Usa a última direção de movimento se não houver input
            _dashDirection = inputDirection != Vector2.zero ? 
                            inputDirection.normalized : 
                            _playerDirection.normalized;

            StartCoroutine(PerformDash());
        }
    }

     IEnumerator PerformDash()
    {
        _canDash = false;
        _isDashing = true;
        
        // Salva valores originais
        float originalSpeed = _playerSpeed;
        Color originalColor = GetComponent<SpriteRenderer>().color;
        
        // Aplica dash
        _playerRigidbody2D.linearVelocity = _dashDirection * dashForce;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.6f);
        
        // Tempo de duração do dash
        yield return new WaitForSeconds(dashDuration);
        
        // Restaura valores
        _playerRigidbody2D.linearVelocity = Vector2.zero;
        GetComponent<SpriteRenderer>().color = originalColor;
        _isDashing = false;
        
        // Cooldown
        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    void HandleMovement()
    {
        _playerDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

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
    }

    void MovePlayer()
    {
        _playerRigidbody2D.MovePosition(_playerRigidbody2D.position + _playerDirection.normalized * _playerSpeed * Time.fixedDeltaTime);
    }

    void HandleAttackInput()
    {
        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0)) && _canAttack)
        {
            StartCoroutine(AttackSequence());
        }
    }

   IEnumerator AttackSequence()
    {
        // Inicia o ataque
        _canAttack = false;
        _isAttacking = true;
        
        // Armazena a velocidade original
        float originalSpeed = _playerSpeed;
        
        // Configura animação e collider
        _playerAnimator.SetInteger("Movimento", 2);
        EnableAttackCollider();
        _playerSpeed = 0f;

        // Tempo de duração do ataque
        yield return new WaitForSeconds(attackDuration);
        
        // Finaliza a fase ativa do ataque
        DisableAttackCollider();
        _playerAnimator.SetInteger("Movimento", 0);
        _playerSpeed = originalSpeed; // Restaura a velocidade aqui
        _isAttacking = false;
        
        // Tempo de cooldown
        yield return new WaitForSeconds(attackCooldown);
        
        // Permite novo ataque
        _canAttack = true;
    }
    void EnableAttackCollider()
    {
        if (attackDirection == Vector2.right)
        {
            attackCollider.offset = new Vector2(1.0f, 0f);
            attackCollider.size = new Vector2(0.84f, 1.16f);
        }
        else if (attackDirection == Vector2.left)
        {
            attackCollider.offset = new Vector2(-1.0f, 0f);
            attackCollider.size = new Vector2(0.84f, 1.16f);
        }
        else if (attackDirection == Vector2.up)
        {
            attackCollider.offset = new Vector2(0f, 1.0f);
            attackCollider.size = new Vector2(0.84f, 1.16f);
        }
        else if (attackDirection == Vector2.down)
        {
            attackCollider.offset = new Vector2(0f, -1.0f);
            attackCollider.size = new Vector2(0.84f, 1.16f);
        }
        attackCollider.enabled = true;
    }

    void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            SlimeController slimeController = other.GetComponent<SlimeController>();
            if (slimeController != null)
            {
                slimeController.TakeDamage(playerDamage);
            }
        }
        else if (other.CompareTag("Destruido"))
        {
            Destructible destructible = other.GetComponent<Destructible>();
            if (destructible != null)
            {
                destructible.DestroyObject();
            }
        }
    }
}