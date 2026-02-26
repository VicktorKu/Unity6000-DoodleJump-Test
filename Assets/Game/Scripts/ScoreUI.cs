using TMPro;
using UnityEngine;

public sealed class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    public void SetScore(int value)
    {
        if (scoreText != null)
            scoreText.text = $"SCORE: {value}";
    }
}