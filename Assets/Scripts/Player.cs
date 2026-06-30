using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float _jumpVelocity = 5f;
    [SerializeField] float _horizontalVelocity = 5f;
    [SerializeField] float _jumpDuration = 0.5f;

    [SerializeField] Sprite _defaultSprite;
    [SerializeField] Sprite _jumpSprite;
    [SerializeField] Sprite _walkingSprite;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _footOffest = 0.3f;

    Animator _animator;

    AudioSource _aduioSource;
    PlayerInput _playerInput;
    Rigidbody2D _rb;
    SpriteRenderer _spriteRenderer;

    float _horizontal;
    float _jumpEndTime;
    bool IsGrounded;
    int _remainingJumps;
    int _coin;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRenderer.sprite;
        _animator = GetComponent<Animator>();
        _aduioSource = GetComponent<AudioSource>();
        _playerInput = GetComponent<PlayerInput>();
    }

    void OnDrawGizmos()
    {
        if (_spriteRenderer == null) return;
        Gizmos.color = Color.red;
        float bottomY = transform.position.y - _spriteRenderer.bounds.extents.y;
        Vector2 origin = new Vector2(transform.position.x, bottomY);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        origin = new Vector2(transform.position.x - _footOffest, bottomY);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        origin = new Vector2(transform.position.x + _footOffest, bottomY);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
    }

    void Update()
    {
        UpdateGrounded();

        _horizontal = 0f;

        if (_playerInput.actions["Move"].ReadValue<Vector2>().x > 0f)
            _horizontal += 1f;
        if (_playerInput.actions["Move"].ReadValue<Vector2>().x < 0f)
            _horizontal -= 1f;

        var vertical = _rb.linearVelocity.y;

        if (_playerInput.actions["Jump"].WasPerformedThisFrame() && _remainingJumps > 0)
        {
            _jumpEndTime = Time.time + _jumpDuration;
            _remainingJumps--;

            _aduioSource.pitch = (_remainingJumps > 0) ? 1f : 1.2f;

            _aduioSource.Play();
        }

        if (_playerInput.actions["Jump"].IsPressed() && Time.time < _jumpEndTime)
        {
            vertical = _jumpVelocity;
        }

        _horizontal *= _horizontalVelocity;
        _rb.linearVelocity = new Vector2(_horizontal, vertical);

        UpdateSprite();
    }

    private void UpdateGrounded()
    {
        IsGrounded = false;
        float bottomY = transform.position.y - _spriteRenderer.bounds.extents.y;
        
        Vector2 origin = new Vector2(transform.position.x, bottomY);
        var hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider != null)
            IsGrounded = true;
        
        origin = new Vector2(transform.position.x - _footOffest, bottomY);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider != null)
            IsGrounded = true;
        
        origin = new Vector2(transform.position.x + _footOffest, bottomY);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider != null)
            IsGrounded = true;

        if (IsGrounded && _rb.linearVelocity.y <= 0f)
            _remainingJumps = 2;
    }

    private void UpdateSprite()
    {
        _animator.SetBool("IsGrounded", IsGrounded);
        _animator.SetFloat("HorizontalSpeed", Mathf.Abs(_horizontal));

        if (_horizontal > 0f)
            _spriteRenderer.flipX = false;
        else if (_horizontal < 0f)
            _spriteRenderer.flipX = true;
    }

    public void AddCoin()
    {
        _coin++;
    }
}
