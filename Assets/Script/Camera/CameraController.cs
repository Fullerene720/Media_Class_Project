using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Camera _camera;

    [Header("Movement")]
    [SerializeField, Min(0.01f)]
    private float _moveDuration = 0.6f;

    [SerializeField, Min(0.1f)]
    private float _padding = 1.2f;

    [Header("Orthographic")]
    [SerializeField, Min(0.1f)]
    private float _orthographicDistance = 10f;

    [SerializeField, Min(0.1f)]
    private float _minOrthographicSize = 3f;

    [Header("Perspective")]
    [SerializeField, Min(0.1f)]
    private float _minPerspectiveDistance = 5f;

    private void Reset()
    {
        _camera = GetComponent<Camera>();
    }

    public IEnumerator FocusBoards( IReadOnlyList<BoardController> boards)
    {
        if (boards == null || boards.Count == 0)
            yield break;

        Bounds combinedBounds = boards[0].GetWorldBounds();

        for (int i = 1; i < boards.Count; i++)
        {
            Bounds boardBounds =  boards[i].GetWorldBounds();

            combinedBounds.Encapsulate(  boardBounds.min );

            combinedBounds.Encapsulate( boardBounds.max );
        }

        Vector3 targetCenter =  combinedBounds.center;

        float radius =
            Mathf.Sqrt( combinedBounds.extents.x *  combinedBounds.extents.x + combinedBounds.extents.z * combinedBounds.extents.z );

        radius *= _padding;

        Vector3 startPosition =  transform.position;

        float startOrthographicSize = _camera.orthographicSize;

        Vector3 targetPosition;
        float targetOrthographicSize = startOrthographicSize;

        if (_camera.orthographic)
        {
            targetPosition =   targetCenter - transform.forward *   _orthographicDistance;

            targetOrthographicSize =   Mathf.Max( _minOrthographicSize,   radius );
        }
        else
        {
            float verticalHalfFov =  _camera.fieldOfView *   0.5f * Mathf.Deg2Rad;

            float horizontalHalfFov =   Mathf.Atan( Mathf.Tan(verticalHalfFov) * _camera.aspect  );

            float limitingHalfFov = Mathf.Min(verticalHalfFov,  horizontalHalfFov  );

            float distance = radius / Mathf.Sin(limitingHalfFov);

            distance =  Mathf.Max(   distance, _minPerspectiveDistance );

            targetPosition = targetCenter - transform.forward * distance;
        }

        float elapsedTime = 0f;

        while (elapsedTime < _moveDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / _moveDuration );

            float smoothT = Mathf.SmoothStep( 0f, 1f, t  );

            transform.position = Vector3.Lerp( startPosition,  targetPosition, smoothT);

            if (_camera.orthographic)
            {
                _camera.orthographicSize =  Mathf.Lerp(  startOrthographicSize,targetOrthographicSize,  smoothT );
            }

            yield return null;
        }

        transform.position = targetPosition;

        if (_camera.orthographic)
        {
            _camera.orthographicSize =  targetOrthographicSize;
        }
    }
}
