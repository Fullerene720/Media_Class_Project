using UnityEngine;

public class BoardSpawner : MonoBehaviour
{
    /*[Header("Data")]
    [SerializeField]
    private BoardData _boardData;*/


    [Header("Prefabs")]
    [SerializeField]
    private BoardController _boardPrefab;

    [SerializeField]
    private GameObject _fieldTilePrefab;

    [SerializeField]
    private GameObject _goalTilePrefab;

    [SerializeField]
    private GameObject _obstaclePrefab;

    [SerializeField]
    private PlayerController _playerPrefab;

    [Header("Goal")]
    [SerializeField]
    private Vector2Int _goalPosition;

    public Vector2Int GoalPosition => _goalPosition;

    [Header("References")]
    [SerializeField]
    private PlayerGroupController _playerGroupController;

    /*[Header("Debug")]
    [SerializeField]
    private bool _spawnOnStart = true;*/

    private void Start()
    {
        
    }

    public BoardController SpawnBoard(
        BoardData data,
        Vector3 worldPosition)
    {
        if (data == null)
        {
            Debug.LogError(  "BoardDataÇ™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒÅB");

            return null;
        }

        // Boardñ{ëÃ
        BoardController board = Instantiate(  _boardPrefab,worldPosition, Quaternion.identity );

        board.name =  $"{data.name}_Board";

        board.Initialize(data);

        // Field
        SpawnField(data, board);

        // Obstacle
        SpawnObstacles(data, board);

        SpawnGoal(data, board);

        // Player
        SpawnPlayer(data, board);

        return board;
    }

    private void SpawnField(
        BoardData data,
        BoardController board)
    {
        for (int y = 0; y < data.Height; y++)
        {
            for (int x = 0; x < data.Width; x++)
            {
                Vector2Int gridPosition =
                    new Vector2Int(x, y);

                // GoalÇÃà íuÇ…ÇÕí èÌÇÃFieldÇê∂ê¨ÇµÇ»Ç¢
                if (gridPosition == data.GoalPosition)
                    continue;

                Vector3 worldPosition =   board.GridToWorld(  gridPosition);

                GameObject tile = Instantiate( _fieldTilePrefab,  worldPosition,   board.transform.rotation,  board.transform );

                tile.name =  $"Tile_{x}_{y}";

                board.AnimationController.RegisterElement(tile.transform);

            }
        }
    }

    private void SpawnObstacles( BoardData data,  BoardController board)
    {
        foreach ( Vector2Int gridPosition in data.ObstaclePositions)
        {
            GameObject obstacle =
                Instantiate(
                    _obstaclePrefab,
                    board.GridToWorld(gridPosition),
                    board.transform.rotation,
                    board.transform
                );

            obstacle.name = $"Obstacle_{gridPosition.x}_{gridPosition.y}";

            board.AnimationController.RegisterElement(obstacle.transform);
        }
    }

    private void SpawnPlayer(  BoardData data,  BoardController board)
    {
        PlayerController player =
            Instantiate(
                _playerPrefab,
                board.GridToWorld(
                    data.PlayerStartPosition),
                board.transform.rotation,
                board.transform
            );

        player.name = "Player";

        player.Initialize(
            board,
            data.PlayerStartPosition
        );

        board.RegisterPlayer(player);

        _playerGroupController.RegisterPlayer(
            player);

        board.AnimationController.RegisterElement(player.transform);
    }

    private void SpawnGoal(
    BoardData data,
    BoardController board)
    {
        GameObject goal =
            Instantiate(
                _goalTilePrefab,
                board.GridToWorld(data.GoalPosition),
                board.transform.rotation,
                board.transform
            );

        goal.name =
            $"Goal_{data.GoalPosition.x}_{data.GoalPosition.y}";
        board.AnimationController.RegisterElement(goal.transform);
    }
}