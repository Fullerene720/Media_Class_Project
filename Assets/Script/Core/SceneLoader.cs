using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private bool _isLoading;

    public void LoadScene(string sceneName)
    {
        if (_isLoading)
            return;

        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogWarning("Scene名が設定されていません。");
            return;
        }

        StartCoroutine(
            LoadSceneCoroutine(sceneName)
        );
    }

    private IEnumerator LoadSceneCoroutine(
        string sceneName)
    {
        _isLoading = true;

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}