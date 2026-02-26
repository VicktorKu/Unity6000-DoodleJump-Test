using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class EntryPoint : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private float minLoadTime = 0.8f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(minLoadTime);

        SceneManager.LoadScene(gameSceneName);
    }
}