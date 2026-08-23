using System;
using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField, Min(0.01f)]
    private float _moveDuration = 0.15f;

    private BoardController _board;

    private Vector2Int _gridPosition;

    private bool _isMoving;

    public Vector2Int GridPosition => _gridPosition;

    public bool IsMoving => _isMoving;

    public void Initialize(
        BoardController board,
        Vector2Int startPosition)
    {
        _board = board;
        _gridPosition = startPosition;

        transform.position =
            _board.GridToWorld(_gridPosition);
    }

    /// <summary>
    /// 指定方向への移動を試みる。
    /// 移動できた場合はtrue。
    /// </summary>
    public bool TryMove(Vector2Int direction)
    {
        if (_isMoving)
            return false;

        Vector2Int nextPosition =
            _gridPosition + direction;

        if (!_board.CanMove(nextPosition))
            return false;

        _gridPosition = nextPosition;

        Vector3 targetPosition =
            _board.GridToWorld(_gridPosition);

        _isMoving = true;

        StartCoroutine(
            MoveCoroutine(targetPosition));

        return true;
    }

    private IEnumerator MoveCoroutine(
        Vector3 targetPosition)
    {
        Vector3 startPosition =
            transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < _moveDuration)
        {
            elapsedTime += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    elapsedTime / _moveDuration);

            // 少し滑らかな加減速を付ける
            float smoothT =
                Mathf.SmoothStep(0f, 1f, t);

            transform.position =
                Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    smoothT);

            yield return null;
        }

        transform.position = targetPosition;

        _isMoving = false;

    }
}
