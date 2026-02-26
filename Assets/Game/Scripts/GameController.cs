using UnityEngine;

public sealed class GameController : MonoBehaviour
{
    [Header("Scene refs")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject playerRoot;
    [SerializeField] private PlatformSpawner spawner;
    [SerializeField] private RabbitController rabbit;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CameraFollow cameraFollow;

    [Header("UI")]
    [SerializeField] private ScoreUI scoreUI;
    [SerializeField] private GameOverUI gameOverUI;

    [SerializeField] private ScreenManager screens;
    [SerializeField] private float scoreScale = 1f;

    
    private readonly RecordsService _records = new();
    private float _startY;
    private float _maxY;
    private int _score;
    [SerializeField] private float pointsPerUnit = 10f;

    private bool _running;
    private bool _paused;

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

        cameraFollow.ResetFollowToTargetY(rabbit.transform.position.y);

        _startY = rabbit.transform.position.y;
        _maxY = _startY;

        _score = 0;
        scoreUI.SetScore(_score);

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

        float y = rabbit.transform.position.y;

        if (y > _maxY)
        {
            _maxY = y;

            float height = _maxY - _startY;
            int newScore = Mathf.Max(0, Mathf.FloorToInt(height * pointsPerUnit));

            if (newScore != _score)
            {
                _score = newScore;
                scoreUI.SetScore(_score);
            }
        }
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

    public void TriggerGameOver()
    {
        if (!_running) return;
        OnGameOver();
    }

    private void OnGameOver()
    {
        _running = false;
        _paused = false;
        Time.timeScale = 1f;

        _records.AddIfValid(_score);

        if (screens != null)
            screens.OpenOverlay(ScreenId.GameOver);

        if (gameOverUI != null)
            gameOverUI.SetScore(_score);
    }
}