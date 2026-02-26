using System.Linq;
using UnityEngine;

public sealed class RecordsScreenUI : MonoBehaviour
{
    [SerializeField] private Transform listRoot;
    [SerializeField] private RecordRowView rowPrefab;
    [SerializeField] private int showTop = 6;

    private readonly RecordsService _service = new();

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (listRoot == null || rowPrefab == null) return;

        for (int i = listRoot.childCount - 1; i >= 0; i--)
            Destroy(listRoot.GetChild(i).gameObject);

        var records = _service.Load()
            .OrderByDescending(r => r.score)
            .Take(showTop)
            .ToList();

        foreach (var r in records)
        {
            var row = Instantiate(rowPrefab, listRoot);
            row.Bind(r.dateIso, r.score);
        }
    }
}