using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private RoundManager _roundManager;

    [SerializeField]
    private GameUIController _gameUIController;

    [Header("Stage")]
    [SerializeField]
    private int _stageNumber = 1;


    [SerializeField]
    private SceneLoader _sceneLoader;

    [SerializeField]
    private string _nextSceneName;

    [SerializeField, Min(0f)]
    private float _stageStartDelay = 0.8f;

    private void OnEnable()
    {
        if (_roundManager != null)
        {
            _roundManager.StageCompleted +=
                HandleStageCompleted;
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(
            _stageStartDelay
        );

        _roundManager.StartStage(
            _stageNumber
        );
    }

    private void OnDisable()
    {
        if (_roundManager != null)
        {
            _roundManager.StageCompleted -=
                HandleStageCompleted;
        }
    }

    private void HandleStageCompleted()
    {
        StartCoroutine(
            StageClearSequence()
        );
    }

    private IEnumerator StageClearSequence()
    {
        // Stage Clear UIŠJn
        Coroutine uiCoroutine =
            StartCoroutine(
                _gameUIController.PlayStageClear(
                    _stageNumber
                )
            );

        // “¯‚É”Õ–Ê‚ğÁ‚µ‚Ä‚¢‚­
        yield return StartCoroutine(
            _roundManager
                .ClearCurrentBoardsAnimated()
        );

        // UI‚ª‚Ü‚¾I‚í‚Á‚Ä‚¢‚ê‚Î‘Ò‚Â
        yield return uiCoroutine;

        Debug.Log(
            $"Stage {_stageNumber} Complete!"
        );

        if (!string.IsNullOrWhiteSpace(
        _nextSceneName))
        {
            _sceneLoader.LoadScene(
                _nextSceneName
            );
        }
    }
}
