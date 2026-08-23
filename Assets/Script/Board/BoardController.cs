using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{

    [SerializeField, Min(0.1f)]
    private float _cellSize = 2f;

    [SerializeField]
    private BoardAnimationController _animationController;

    public BoardAnimationController AnimationController
    => _animationController;

    private BoardData _data;

    private readonly HashSet<Vector2Int> _obstaclePositions = new();

    public float CellSize => _cellSize;

    private PlayerController _player;

    public void Initialize(BoardData data)
    {
        _data = data;

        _obstaclePositions.Clear();

        foreach (Vector2Int position in data.ObstaclePositions)
        {
            _obstaclePositions.Add(position);
        }
    }

    /// <summary>
    /// 指定されたマスへ移動可能か調べる。
    /// </summary>
    public bool CanMove(Vector2Int position)
    {
        if (!IsInside(position))
            return false;

        if (_obstaclePositions.Contains(position))
            return false;

        return true;
    }

    /// <summary>
    /// 指定された座標が盤面内か調べる。
    /// </summary>
    public bool IsInside(Vector2Int position)
    {
        if (_data == null)
            return false;

        return
            position.x >= 0 &&
            position.x < _data.Width &&
            position.y >= 0 &&
            position.y < _data.Height;
    }

    /// <summary>
    /// マス目座標をUnity上のワールド座標へ変換する。
    /// </summary>
    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        Vector3 localPosition = new Vector3(
            gridPosition.x * _cellSize,
            0f,
            gridPosition.y * _cellSize
        );

        return transform.TransformPoint(localPosition);
    }


    public void RegisterPlayer(
    PlayerController player)
    {
        _player = player;
    }

    public bool IsGoal(Vector2Int position)
    {
        if (_data == null)
            return false;

        return position == _data.GoalPosition;
    }

    public bool IsPlayerOnGoal
    {
        get
        {
            if (_player == null)
                return false;

            return IsGoal(
                _player.GridPosition
            );
        }
    }


    //盤面の範囲を取得
    public Bounds GetWorldBounds()
    {
        if (_data == null)
            return new Bounds(transform.position, Vector3.zero);

        Vector3 localCenter = new Vector3(
            (_data.Width - 1) * _cellSize * 0.5f,
            0f,
            (_data.Height - 1) * _cellSize * 0.5f
        );

        Vector3 worldCenter =
            transform.TransformPoint(localCenter);

        Vector3 size = new Vector3(
            _data.Width * _cellSize,
            0f,
            _data.Height * _cellSize
        );

        return new Bounds(worldCenter, size);
    }


}
