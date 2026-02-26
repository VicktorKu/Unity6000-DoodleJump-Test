using System.Collections.Generic;
using UnityEngine;

public sealed class ScreenManager : MonoBehaviour
{
    [SerializeField] private ScreenRoot[] screens;

    private readonly Dictionary<ScreenId, ScreenRoot> _map = new();
    private ScreenId _currentPage = ScreenId.MainMenu;

    private void Awake()
    {
        _map.Clear();
        foreach (var s in screens)
            if (s != null) _map[s.Id] = s;

        OpenPage(ScreenId.MainMenu);
        CloseAllOverlays();
    }

    public void OpenPage(ScreenId pageId)
    {
        _currentPage = pageId;

        foreach (var kv in _map)
        {
            if (kv.Value.Layer == ScreenLayer.Page)
                kv.Value.Show(kv.Key == pageId);
        }

        CloseAllOverlays();
    }

    public void OpenOverlay(ScreenId overlayId)
    {
        if (_map.TryGetValue(overlayId, out var s) && s.Layer == ScreenLayer.Overlay)
            s.Show(true);
    }

    public void CloseOverlay(ScreenId overlayId)
    {
        if (_map.TryGetValue(overlayId, out var s) && s.Layer == ScreenLayer.Overlay)
            s.Show(false);
    }

    public void CloseAllOverlays()
    {
        foreach (var kv in _map)
            if (kv.Value.Layer == ScreenLayer.Overlay)
                kv.Value.Show(false);
    }

    public bool IsPageOpen(ScreenId pageId) => _currentPage == pageId;
}