using UnityEngine;

public sealed class GameController : MonoBehaviour
{
    [Header("Scene refs")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject playerRoot;
    [SerializeField] private PlatformSpawner spawner;
    [SerializeField] private RabbitController rabbit;
    [SerializeField] private Camera mainCamera;

    [Header("UI")]
    [SerializeField] private ScoreUI scoreUI;

    private bool _running;
    private bool _paused;

    private float _startY;

    private void Start()
    {
        SetGameplayActive(false);
    }

    public void StartRun()
    {
        SetGameplayActive(true);

        float startY = spawnPoint != null ? spawnPoint.position.y : 0f;
        float startX = spawnPoint != null ? spawnPoint.position.x : 0f;

        spawner.ResetWorld(startY);

        rabbit.ResetPlayer();
        var p = rabbit.transform.position;
        rabbit.transform.position = new Vector3(startX, startY + 0.8f, p.z);

        ResetCameraTo(rabbit.transform.position.y);

        _startY = rabbit.transform.position.y;
        scoreUI.SetScore(0);

        _running = true;
        _paused = false;
        Time.timeScale = 1f;
    }

    public void StopRun()
    {
        _running = false;
        _paused = false;
        Time.timeScale = 1f;

        SetGameplayActive(false);
    }

    public void SetPaused(bool paused)
    {
        _paused = paused;
        Time.timeScale = paused ? 0f : 1f;
    }

    private void Update()
    {
        if (!_running || _paused) return;

        spawner.Tick(mainCamera.transform.position.y);

    }

    private void SetGameplayActive(bool active)
    {
        if (playerRoot != null) playerRoot.SetActive(active);

        if (!active)
            spawner.ClearAll();
    }

    private void ResetCameraTo(float targetY)
    {
        if (mainCamera == null) return;
        var camT = mainCamera.transform;
        camT.position = new Vector3(camT.position.x, targetY + 2.5f, camT.position.z);
    }
}