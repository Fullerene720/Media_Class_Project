using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerGroupController : MonoBehaviour
{
    [SerializeField]
    private InputController _inputController;

    private readonly List<PlayerController> _players = new();

    private bool _isProcessingMove;

    public event Action MoveCompleted;

    private void OnEnable()
    {
        if (_inputController != null)
        {
            _inputController.MoveRequested +=
                HandleMoveRequested;
        }
    }

    private void OnDisable()
    {
        if (_inputController != null)
        {
            _inputController.MoveRequested -=
                HandleMoveRequested;
        }
    }

    public void RegisterPlayer(
        PlayerController player)
    {
        if (player == null)
            return;

        if (_players.Contains(player))
            return;

        _players.Add(player);
    }

    public void UnregisterPlayer(
        PlayerController player)
    {
        _players.Remove(player);
    }

    public void ClearPlayers()
    {
        _players.Clear();
    }

    private void HandleMoveRequested(
        Vector2Int direction)
    {
        if (_isProcessingMove)
            return;

        _isProcessingMove = true;

        bool anyPlayerMoved = false;

        foreach (PlayerController player in _players)
        {
            if (player.TryMove(direction))
            {
                anyPlayerMoved = true;
            }
        }

        // ëSàıï«Ç»Ç«Ç…Ç‘Ç¬Ç©Ç¡ÇƒÇ¢ÇΩèÍçá
        if (!anyPlayerMoved)
        {
            _isProcessingMove = false;
            return;
        }

        StartCoroutine(
            WaitForPlayersToFinish());
    }

    private IEnumerator WaitForPlayersToFinish()
    {
        while (IsAnyPlayerMoving())
        {
            yield return null;
        }

        _isProcessingMove = false;

        MoveCompleted?.Invoke();
    }

    private bool IsAnyPlayerMoving()
    {
        foreach (PlayerController player in _players)
        {
            if (player.IsMoving)
                return true;
        }

        return false;
    }
}