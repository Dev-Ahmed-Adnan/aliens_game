using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float _jumpEndTime;

    private Rigidbody2D _rb;

    [SerializeField] private float _jumpVelocity = 5f;
    [SerializeField] private float _jumpDuration = 0.5f;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
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

        if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed && Time.time < _jumpEndTime)
        {
            vertical = _jumpVelocity;
        }

        _rb.linearVelocity = new Vector2(horizontal, vertical);
    }
}
