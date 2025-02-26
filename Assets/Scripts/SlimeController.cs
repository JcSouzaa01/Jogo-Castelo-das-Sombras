using UnityEngine;

public class SlimeController : MonoBehaviour
{

    public float _moveSpeedSlime = 3.5f;
    private Vector2 _slimeDirection;
    private Rigidbody2D _slimeRB2D;
    public DetectionController _detectionaArea;
    private SpriteRenderer _spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _slimeRB2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        _slimeDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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
}
