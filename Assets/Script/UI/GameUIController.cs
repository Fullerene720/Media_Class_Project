using System.Collections;
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [Header("Round Transition UI")]
    [SerializeField]
    private CanvasGroup _roundTransitionGroup;

    [SerializeField]
    private TMP_Text _stageText;

    [SerializeField]
    private TMP_Text _roundText;

    [Header("Round Clear UI")]
    [SerializeField]
    private CanvasGroup _roundClearGroup;

    [SerializeField]
    private TMP_Text _roundClearText;

    [Header("Stage Clear UI")]
    [SerializeField]
    private CanvasGroup _stageClearGroup;

    [SerializeField]
    private TMP_Text _stageClearText;

    [Header("Animation")]
    [SerializeField, Min(0.01f)]
    private float _fadeDuration = 0.25f;

    [SerializeField, Min(0f)]
    private float _displayDuration = 0.8f;

    private void Awake()
    {
        _roundTransitionGroup.alpha = 0f;
        _roundClearGroup.alpha = 0f;
        _stageClearGroup.alpha = 0f;
    }


    public IEnumerator PlayRoundStart(  int stageNumber, int roundNumber)
    {
        _stageText.text = $"STAGE {stageNumber}";

        _roundText.text = $"ROUND {roundNumber}";

        yield return StartCoroutine(FadeCanvasGroup( _roundTransitionGroup, 0f, 1f));

        yield return new WaitForSeconds(_displayDuration);

        yield return StartCoroutine( FadeCanvasGroup(_roundTransitionGroup, 1f, 0f ) );
    }


    public IEnumerator PlayRoundClear(int roundNumber)
    {
        _roundClearText.text =  $"ROUND {roundNumber} CLEAR!";

        yield return StartCoroutine( FadeCanvasGroup(_roundClearGroup, 0f,1f) );

        yield return new WaitForSeconds(_displayDuration);

        yield return StartCoroutine(  FadeCanvasGroup(_roundClearGroup, 1f, 0f ));
    }

    public IEnumerator PlayStageClear(int stageNumber)
    {
        _stageClearText.text = $"STAGE {stageNumber} CLEAR!";

        yield return StartCoroutine(FadeCanvasGroup(_stageClearGroup,0f,1f));

        yield return new WaitForSeconds(_displayDuration);

        yield return StartCoroutine( FadeCanvasGroup(_stageClearGroup,1f,0f));
    }

    private IEnumerator FadeCanvasGroup( CanvasGroup canvasGroup,float startAlpha,float endAlpha)
    {
        float elapsedTime = 0f;

        canvasGroup.alpha =startAlpha;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01( elapsedTime / _fadeDuration );

            float smoothT = Mathf.SmoothStep(  0f,  1f,t);

            canvasGroup.alpha = Mathf.Lerp( startAlpha, endAlpha, smoothT );

            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
