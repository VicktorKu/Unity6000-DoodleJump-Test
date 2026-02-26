using TMPro;
using UnityEngine;

public sealed class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    public void SetScore(int score)
    {
        if (scoreText != null)
            scoreText.text = $"SCORE: {score}";
    }
}