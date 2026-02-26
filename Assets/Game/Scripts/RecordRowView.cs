using System;
using TMPro;
using UnityEngine;

public sealed class RecordRowView : MonoBehaviour
{
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text scoreText;

    public void Bind(string dateIso, int score)
    {
        if (DateTime.TryParse(dateIso, out var dt))
        {
            dateText.text = dt.ToString("dd.MM");
        }
        else
        {
            dateText.text = dateIso;
        }

        scoreText.text = score.ToString();
    }
}