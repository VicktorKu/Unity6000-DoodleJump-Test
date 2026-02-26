using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class RecordsService
{
    private const string Key = "records_v1";
    private const int Max = 20;

    [Serializable]
    private class RecordsWrapper { public List<RecordEntry> items = new(); }

    public List<RecordEntry> Load()
    {
        var json = PlayerPrefs.GetString(Key, "");
        if (string.IsNullOrWhiteSpace(json))
            return new List<RecordEntry>();

        try
        {
            var w = JsonUtility.FromJson<RecordsWrapper>(json);
            return w?.items ?? new List<RecordEntry>();
        }
        catch
        {
            return new List<RecordEntry>();
        }
    }

    public void AddIfValid(int score)
    {
        if (score <= 0) return;

        var list = Load();
        list.Add(new RecordEntry
        {
            dateIso = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            score = score
        });

        list = list
            .OrderByDescending(r => r.score)
            .Take(Max)
            .ToList();

        Save(list);
    }

    private void Save(List<RecordEntry> list)
    {
        var w = new RecordsWrapper { items = list };
        PlayerPrefs.SetString(Key, JsonUtility.ToJson(w));
        PlayerPrefs.Save();
    }
}