using UnityEngine;

public enum ScreenLayer
{
    Page,
    Overlay
}

public sealed class ScreenRoot : MonoBehaviour
{
    public ScreenId Id;
    public ScreenLayer Layer;

    public void Show(bool show) => gameObject.SetActive(show);
}