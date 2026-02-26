using UnityEngine;

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

    private Rigidbody2D RB
    {
        get
        {
            if (_rb == null)
                _rb = GetComponent<Rigidbody2D>();
            return _rb;
        }
    }

    public void ResetPlayer()
    {
        IsDead = false;
        RB.linearVelocity = Vector2.zero;
    }

    private void Update()
    {
        if (IsDead) return;

        float x = ReadMoveX();

        var v = RB.linearVelocity;
        v.x = x * moveSpeed;
        RB.linearVelocity = v;

        if (transform.position.y < deathBelowY)
            IsDead = true;
    }

    private float ReadMoveX()
    {
        float touch = ReadTouchMoveX_Multi();
        if (touch != 0f) return touch;

#if UNITY_EDITOR || UNITY_STANDALONE
        float mouse = ReadMouseMoveX();
        if (mouse != 0f) return mouse;
#endif
        return Input.GetAxisRaw("Horizontal");
    }

    private float ReadTouchMoveX_Multi()
    {
        if (Input.touchCount <= 0) return 0f;

        for (int i = 0; i < Input.touchCount; i++)
        {
            var t = Input.GetTouch(i);

            if (t.phase != TouchPhase.Began &&
                t.phase != TouchPhase.Moved &&
                t.phase != TouchPhase.Stationary)
                continue;

            float y01 = t.position.y / Screen.height;
            if (y01 > activeScreenBottom01)
                continue;

            float x01 = t.position.x / Screen.width;
            return (x01 < 0.5f) ? -1f : 1f;
        }

        return 0f;
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    private float ReadMouseMoveX()
    {
        if (!Input.GetMouseButton(0)) return 0f;

        float y01 = Input.mousePosition.y / Screen.height;
        if (y01 > activeScreenBottom01) return 0f;

        float x01 = Input.mousePosition.x / Screen.width;
        return (x01 < 0.5f) ? -1f : 1f;
    }
#endif

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