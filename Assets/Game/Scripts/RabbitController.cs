using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class RabbitController : MonoBehaviour
{
    [SerializeField] private float jumpVelocity = 10f;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float deathBelowY = -10f;

    [Header("Mobile input")]
    [Range(0.1f, 1f)]
    [SerializeField] private float activeScreenBottom01 = 0.4f;

    private Rigidbody2D _rb;
    public bool IsDead { get; private set; }

    private Rigidbody2D RB => _rb != null ? _rb : (_rb = GetComponent<Rigidbody2D>());

    public void ResetPlayer()
    {
        IsDead = false;
        RB.linearVelocity = Vector2.zero;
    }

    private void Update()
    {
        if (IsDead) return;

        float x = ReadMoveX_New();

        var v = RB.linearVelocity;
        v.x = x * moveSpeed;
        RB.linearVelocity = v;

        if (transform.position.y < deathBelowY)
            IsDead = true;
    }

    private float ReadMoveX_New()
    {
        float touch = ReadTouchMoveX_New();
        if (touch != 0f) return touch;

        float mouse = ReadMouseMoveX_New();
        if (mouse != 0f) return mouse;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) return -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) return 1f;
        }

        return 0f;
    }

    private float ReadTouchMoveX_New()
    {
        if (Touchscreen.current == null) return 0f;

        var touch = Touchscreen.current.primaryTouch;
        if (!touch.press.isPressed) return 0f;

        Vector2 pos = touch.position.ReadValue();
        return ScreenSplitMoveX(pos);
    }

    private float ReadMouseMoveX_New()
    {
        if (Mouse.current == null) return 0f;
        if (!Mouse.current.leftButton.isPressed) return 0f;

        Vector2 pos = Mouse.current.position.ReadValue();
        return ScreenSplitMoveX(pos);
    }

    private float ScreenSplitMoveX(Vector2 screenPos)
    {
        float y01 = screenPos.y / Screen.height;
        if (y01 > activeScreenBottom01) return 0f;

        float x01 = screenPos.x / Screen.width;
        return (x01 < 0.5f) ? -1f : 1f;
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (IsDead) return;

        if (c.collider.CompareTag("Platform") && RB.linearVelocity.y <= 0f)
        {
            var v = RB.linearVelocity;
            v.y = jumpVelocity;
            RB.linearVelocity = v;
        }
    }
}