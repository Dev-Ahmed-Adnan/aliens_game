using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float _jumpEndTime;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    private bool IsGrounded;

    [SerializeField] private float _jumpVelocity = 5f;
    [SerializeField] private float _horizontalVelocity = 5f;
    [SerializeField] private float _jumpDuration = 0.5f;

    [SerializeField] Sprite _defaultSprite;
    [SerializeField] Sprite _jumpSprite;
    [SerializeField] Sprite _walkingSprite;
    [SerializeField] LayerMask _layerMask;

    private float _horizontal;

    private Animator _animator;

    [SerializeField] private float _footOffest = 0.3f;
    private int _remainingJumps;
    private AudioSource _aduioSource;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRenderer.sprite;
        _animator = GetComponent<Animator>();
        _aduioSource = GetComponent<AudioSource>();
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

        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                _horizontal += 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                _horizontal -= 1f;
        }

        var vertical = _rb.linearVelocity.y;

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && _remainingJumps > 0)
        {
            _jumpEndTime = Time.time + _jumpDuration;
            _remainingJumps--;
            _aduioSource.Play();
        }

        if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed && Time.time < _jumpEndTime)
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
}
