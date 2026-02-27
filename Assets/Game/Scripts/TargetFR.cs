using UnityEngine;

public class TargetFR: MonoBehaviour 
{
    public void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    Application.targetFrameRate = 120;
#endif
    }
}