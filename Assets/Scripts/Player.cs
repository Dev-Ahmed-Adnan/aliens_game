using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float _jumpEndTime;

    private Rigidbody2D _rb;

    private bool IsGrounded;

    [SerializeField] private float _jumpVelocity = 5f;
    [SerializeField] private float _horizontalVelocity = 5f;
    [SerializeField] private float _jumpDuration = 0.5f;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);
        var hit = Physics2D.Raycast(origin, Vector2.down, 0.1f);
        if (hit.collider != null)
            IsGrounded = true;
        else IsGrounded = false;

        float horizontal = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                horizontal += 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                horizontal -= 1f;
        }

        var vertical = _rb.linearVelocity.y;

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) {
            _jumpEndTime = Time.time + _jumpDuration;
        }

        if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed && Time.time < _jumpEndTime && IsGrounded)
        {
            vertical = _jumpVelocity;
        }

        horizontal *= _horizontalVelocity;
        _rb.linearVelocity = new Vector2(horizontal, vertical);
    }
}
