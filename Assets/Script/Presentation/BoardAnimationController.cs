using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAnimationController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField, Min(0.01f)]
    private float _scaleDuration = 0.12f;

    [SerializeField, Min(0f)]
    private float _spawnInterval = 0.035f;

    private readonly List<ElementData> _elements = new();

    private class ElementData
    {
        public Transform Transform;
        public Vector3 OriginalScale;

        public ElementData(
            Transform transform,
            Vector3 originalScale)
        {
            Transform = transform;
            OriginalScale = originalScale;
        }
    }

    /// <summary>
    /// 演出対象となるオブジェクトを登録する。
    /// 登録した瞬間は見えないようScaleを0にする。
    /// </summary>
    public void RegisterElement(Transform element)
    {
        Vector3 originalScale =
            element.localScale;

        _elements.Add(
            new ElementData(
                element,
                originalScale
            )
        );

        element.localScale =
            Vector3.zero;
    }

    /// <summary>
    /// 登録順にオブジェクトを出現させる。
    /// </summary>
    public IEnumerator PlayAppear()
    {
        foreach (ElementData element in _elements)
        {
            if (element.Transform == null)
                continue;

            StartCoroutine(
                ScaleElement(
                    element.Transform,
                    Vector3.zero,
                    element.OriginalScale
                )
            );

            yield return new WaitForSeconds(
                _spawnInterval
            );
        }

        // 最後のオブジェクトの拡大完了を待つ
        yield return new WaitForSeconds(
            _scaleDuration
        );
    }

    /// <summary>
    /// 登録とは逆順に縮小させる。
    /// </summary>
    public IEnumerator PlayDisappear()
    {
        for (
            int i = _elements.Count - 1;
            i >= 0;
            i--)
        {
            ElementData element =
                _elements[i];

            if (element.Transform == null)
                continue;

            StartCoroutine(
                ScaleElement(
                    element.Transform,
                    element.OriginalScale,
                    Vector3.zero
                )
            );

            yield return new WaitForSeconds(
                _spawnInterval
            );
        }

        yield return new WaitForSeconds(
            _scaleDuration
        );
    }

    private IEnumerator ScaleElement(
        Transform target,
        Vector3 startScale,
        Vector3 endScale)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _scaleDuration)
        {
            elapsedTime += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    elapsedTime / _scaleDuration
                );

            float smoothT =
                Mathf.SmoothStep(
                    0f,
                    1f,
                    t
                );

            target.localScale =
                Vector3.Lerp(
                    startScale,
                    endScale,
                    smoothT
                );

            yield return null;
        }

        target.localScale = endScale;
    }
}