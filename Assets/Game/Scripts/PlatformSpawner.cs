using System.Collections.Generic;
using UnityEngine;

public sealed class PlatformSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject longPlatformPrefab;
    [SerializeField] private GameObject shortPlatformPrefab;

    [Header("Parents")]
    [SerializeField] private Transform platformsParent;

    [Header("Spawn area")]
    [SerializeField] private float minX = -2.2f;
    [SerializeField] private float maxX = 2.2f;

    [Header("Spawn params")]
    [SerializeField] private float minStepY = 1.6f;
    [SerializeField] private float maxStepY = 2.2f;
    [SerializeField] private float aheadDistance = 14f;
    [SerializeField] private float destroyBelowDistance = 10f;

    [Header("Chance")]
    [Range(0f, 1f)]
    [SerializeField] private float shortPlatformChance = 0.35f;

    private readonly List<Transform> _spawned = new();
    private float _nextSpawnY;

    public void ResetWorld(float startY = 0f)
    {
        for (int i = platformsParent.childCount - 1; i >= 0; i--)
            Destroy(platformsParent.GetChild(i).gameObject);

        _spawned.Clear();

        _nextSpawnY = startY;

        SpawnPlatform(new Vector2(0f, startY));
        _nextSpawnY += Random.Range(minStepY, maxStepY);
    }

    public void Tick(float cameraY)
    {
        float targetY = cameraY + aheadDistance;

        while (_nextSpawnY < targetY)
        {
            float x = Random.Range(minX, maxX);
            SpawnPlatform(new Vector2(x, _nextSpawnY));
            _nextSpawnY += Random.Range(minStepY, maxStepY);
        }

        float killY = cameraY - destroyBelowDistance;
        for (int i = _spawned.Count - 1; i >= 0; i--)
        {
            if (_spawned[i] == null)
            {
                _spawned.RemoveAt(i);
                continue;
            }

            if (_spawned[i].position.y < killY)
            {
                Destroy(_spawned[i].gameObject);
                _spawned.RemoveAt(i);
            }
        }
    }

    private void SpawnPlatform(Vector2 pos)
    {
        var prefab = (Random.value < shortPlatformChance) ? shortPlatformPrefab : longPlatformPrefab;
        var go = Instantiate(prefab, pos, Quaternion.identity, platformsParent);
        _spawned.Add(go.transform);
    }
    public void ClearAll()
    {
        if (platformsParent == null) return;

        for (int i = platformsParent.childCount - 1; i >= 0; i--)
            Destroy(platformsParent.GetChild(i).gameObject);

        _spawned.Clear();
    }
}