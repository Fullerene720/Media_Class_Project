using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardData",menuName = "Puzzle/Board Data")]
public class BoardData : ScriptableObject
{
    [Header("”Õ–Ê‚Ì‘å‚«‚³")]
    [SerializeField, Min(1)]
    private int _width = 5;

    [SerializeField, Min(1)]
    private int _height = 5;

    [Header("ƒvƒŒƒCƒ„[")]
    [SerializeField]
    private Vector2Int _playerStartPosition;

    [Header("áŠQ•¨")]
    [SerializeField]
    private List<Vector2Int> _obstaclePositions = new();

    [Header("Goal")]
    [SerializeField]
    private Vector2Int _goalPosition;

    public int Width => _width;
    public int Height => _height;

    public Vector2Int PlayerStartPosition =>
        _playerStartPosition;

    public IReadOnlyList<Vector2Int> ObstaclePositions =>
        _obstaclePositions;

    public Vector2Int GoalPosition => _goalPosition;
}