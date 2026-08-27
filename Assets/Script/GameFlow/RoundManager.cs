using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoundManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private BoardSpawner _boardSpawner;

    [SerializeField]
    private PlayerGroupController _playerGroupController;

    [SerializeField]
    private InputController _inputController;

    [Header("Board Data")]
    [SerializeField]
    private BoardData _round1BoardData;

    [SerializeField]
    private BoardData _round2BoardData;

    [SerializeField]
    private GameUIController _gameUIController;

    [SerializeField]
    private GameCameraController _cameraController;

    [Header("Board Position")]
    [SerializeField]
    private Vector3 _leftBoardPosition =
        Vector3.zero;

    [SerializeField]
    private Vector3 _rightBoardPosition =
        new Vector3(7f, 0f, 0f);

    private readonly List<BoardController>
        _activeBoards = new();

    private int _currentRound;

    public event Action StageCompleted;


    private int _stageNumber;

    private void OnEnable()
    {
        if (_playerGroupController != null)
        {
            _playerGroupController.MoveCompleted +=
                CheckRoundClear;
        }
    }

    private void OnDisable()
    {
        if (_playerGroupController != null)
        {
            _playerGroupController.MoveCompleted -=
                CheckRoundClear;
        }
    }

    public void StartStage(int stageNumber)
    {
        _stageNumber = stageNumber;

        StartCoroutine(StartRound1());
    }

    private IEnumerator StartRound1()
    {
        _currentRound = 1;

        _inputController.SetInputEnabled(false);



        BoardController board =
            _boardSpawner.SpawnBoard(
                _round1BoardData,
                _leftBoardPosition
            );

        _activeBoards.Add(board);

        Debug.Log("Stage1 Round1");

        // UIとBoard生成演出を同時開始
        Coroutine uiCoroutine =
            StartCoroutine(
               _gameUIController.PlayRoundStart(_stageNumber, 1)
        );

        //カメラ演出
        Coroutine cameraCoroutine =StartCoroutine(
            _cameraController.FocusBoards(_activeBoards)
        );



        // 盤面生成演出
        yield return StartCoroutine(
            board.AnimationController.PlayAppear()
        );

        yield return uiCoroutine;
        yield return cameraCoroutine;

        _inputController.SetInputEnabled(true);
    }

    private IEnumerator StartRound2()
    {
        _currentRound = 2;

        _inputController.SetInputEnabled(false);

        // Round Clear UIを開始
        Coroutine clearUICoroutine =
            StartCoroutine(
                _gameUIController.PlayRoundClear(1)
            );

        yield return StartCoroutine(
            ClearCurrentBoardsAnimated()
        );

        yield return clearUICoroutine;

        // Round1と同じBoardを初期状態から再生成
        BoardController leftBoard =
            _boardSpawner.SpawnBoard(
                _round1BoardData,
                _leftBoardPosition
            );

        // 新しいBoard
        BoardController rightBoard =
            _boardSpawner.SpawnBoard(
                _round2BoardData,
                _rightBoardPosition
            );

        _activeBoards.Add(leftBoard);
        _activeBoards.Add(rightBoard);

        Debug.Log("Stage1 Round2");

        // Round2開始UI
        Coroutine roundUICoroutine =
            StartCoroutine( _gameUIController.PlayRoundStart(_stageNumber, 2 ) );

        Coroutine cameraCoroutine =
            StartCoroutine(_cameraController.FocusBoards(_activeBoards ));

        yield return StartCoroutine(
            leftBoard.AnimationController.PlayAppear()
        );

        yield return StartCoroutine(
            rightBoard.AnimationController.PlayAppear()
        );

        // 終わるまで待つ
        yield return roundUICoroutine;
        yield return cameraCoroutine;

        _inputController.SetInputEnabled(true);
    }

    private void CheckRoundClear()
    {
        if (_activeBoards.Count == 0)
            return;

        foreach (BoardController board in _activeBoards)
        {
            if (!board.IsPlayerOnGoal)
                return;
        }

        // ここまで来た = 全BoardのPlayerがGoal上
        CompleteRound();
    }

    private void CompleteRound()
    {
        if (_currentRound == 1)
        {
            Debug.Log("Round1 Clear!");


            StartCoroutine(StartRound2());
        }
        else if (_currentRound == 2)
        {
            _inputController.SetInputEnabled(false);

            StageCompleted?.Invoke();
        }
    }

    public IEnumerator ClearCurrentBoardsAnimated()
    {
        _playerGroupController.ClearPlayers();

        for (int i = _activeBoards.Count - 1; i >= 0; i--)
        {
            BoardController board =
                _activeBoards[i];

            if (board == null)
                continue;

            yield return StartCoroutine(
                board.AnimationController.PlayDisappear()
            );

            Destroy(board.gameObject);
        }

        _activeBoards.Clear();
    }
}