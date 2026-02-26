using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class RabbitController : MonoBehaviour
{
    [SerializeField] private float jumpVelocity = 10f;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float deathBelowY = -10f;

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

        float x = Input.GetAxisRaw("Horizontal");

        var v = RB.linearVelocity;
        v.x = x * moveSpeed;
        RB.linearVelocity = v;

        if (transform.position.y < deathBelowY)
            IsDead = true;
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