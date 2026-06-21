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

    private float _horizontal;



    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRenderer.sprite;
    }

    void Update()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
        var hit = Physics2D.Raycast(origin, Vector2.down, 0.1f);
        if (hit.collider != null)
            IsGrounded = true;
        else 
            IsGrounded = false;

        _horizontal = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                _horizontal += 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                _horizontal -= 1f;
        }

        var vertical = _rb.linearVelocity.y;

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) {
            _jumpEndTime = Time.time + _jumpDuration;
        }

        if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed && Time.time < _jumpEndTime && IsGrounded)
        {
            vertical = _jumpVelocity;
        }

        _horizontal *= _horizontalVelocity;
        _rb.linearVelocity = new Vector2(_horizontal, vertical);

        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (IsGrounded)
            _spriteRenderer.sprite = _defaultSprite;
        else 
            _spriteRenderer.sprite = _jumpSprite;

        if (_horizontal > 0f)
            _spriteRenderer.flipX = false;
        else if (_horizontal < 0f)
            _spriteRenderer.flipX = true;
    }
}
