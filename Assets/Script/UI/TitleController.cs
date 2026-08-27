using UnityEngine;

public class TitleController : MonoBehaviour
{
    [SerializeField]
    private SceneLoader _sceneLoader;

    [SerializeField]
    private string _firstStageSceneName =
        "Stage1";

    public void OnStartButtonClicked()
    {
        _sceneLoader.LoadScene(
            _firstStageSceneName
        );
    }
}