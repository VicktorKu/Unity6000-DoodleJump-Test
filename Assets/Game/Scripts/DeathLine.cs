using UnityEngine;

public sealed class DeathLine : MonoBehaviour
{
    [SerializeField] private GameController game;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        game.TriggerGameOver();
    }
}