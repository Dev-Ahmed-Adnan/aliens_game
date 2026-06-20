using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    void Start()
    {

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

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        var vertical = rb.linearVelocity.y;

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            vertical = 5;
        }

        rb.linearVelocity = new Vector2(horizontal, vertical);
    }
}
