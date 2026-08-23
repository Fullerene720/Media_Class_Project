using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _moveAction;

    public event Action<Vector2Int> MoveRequested;

    private bool _waitingForNeutral;

    private void OnEnable()
    {
        if (_moveAction == null)
        {
            Debug.LogError(
                "Move Actionが設定されていません。");

            return;
        }

        _moveAction.action.performed += OnMovePerformed;
        _moveAction.action.canceled += OnMoveCanceled;

        _moveAction.action.Enable();
    }

    private void OnDisable()
    {
        if (_moveAction == null)
            return;

        _moveAction.action.performed -= OnMovePerformed;
        _moveAction.action.canceled -= OnMoveCanceled;

        _moveAction.action.Disable();
    }

    private void OnMovePerformed(
        InputAction.CallbackContext context)
    {
        // キーを押しっぱなしにして
        // 何度も移動するのを防ぐ
        if (_waitingForNeutral)
            return;

        Vector2 input =
            context.ReadValue<Vector2>();

        if (input.sqrMagnitude < 0.25f)
            return;

        Vector2Int direction;

        if (Mathf.Abs(input.x) >
            Mathf.Abs(input.y))
        {
            direction = new Vector2Int(
                input.x > 0f ? 1 : -1,
                0
            );
        }
        else
        {
            direction = new Vector2Int(
                0,
                input.y > 0f ? 1 : -1
            );
        }

        _waitingForNeutral = true;

        MoveRequested?.Invoke(direction);
    }

    private void OnMoveCanceled(
        InputAction.CallbackContext context)
    {
        _waitingForNeutral = false;
    }

    /// <summary>
    /// Round開始演出などで入力を停止するために使用する。
    /// </summary>
    public void SetInputEnabled(bool enabled)
    {
        if (_moveAction == null)
            return;

        _waitingForNeutral = false;

        if (enabled)
        {
            _moveAction.action.Enable();
        }
        else
        {
            _moveAction.action.Disable();
        }
    }
}
