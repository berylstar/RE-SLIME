using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct GameState
{
    public Action<Vector2> OnMove;
    public Action OnESC;
    public Action OnSpace;
    public Action OnI;
    public Action OnC;
    public Action OnV;
    public Action OnR;
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public GameState StoryState = new GameState();
    public GameState IdleState = new GameState();
    public GameState MenuState = new GameState();
    public GameState DialogueState = new GameState();
    public GameState ShopState = new GameState();
    public GameState BoxState = new GameState();
    public GameState InBossState = new GameState();

    private readonly Queue<GameState> StateQueue = new Queue<GameState>();

    private void Awake()
    {
        // ΩÃ±€≈Ê
        if (Instance == null) { Instance = this; }
        else if (Instance != null) { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);
    }

    public void StateEnqueue(GameState state)
    {
        StateQueue.Enqueue(state);
    }

    public void StateDequeue()
    {
        StateQueue.Dequeue();
    }

    public GameState GetCurrState()
    {
        if (StateQueue.Count < 1)
            return IdleState;
        else
            return StateQueue.Peek();
    }

    #region InputAction
    private void OnMove(InputValue value)
    {
        Debug.Log("OnMove");
        GetCurrState().OnMove?.Invoke(value.Get<Vector2>());
    }

    private void OnSpace(InputValue value)
    {
        Debug.Log("OnSpace");
        GetCurrState().OnSpace?.Invoke();
    }

    private void OnESC(InputValue value)
    {
        Debug.Log("OnESC");
        GetCurrState().OnESC?.Invoke();
    }

    private void OnI(InputValue value)
    {
        Debug.Log("OnI");
        GetCurrState().OnI?.Invoke();
    }

    private void OnC(InputValue value)
    {
        Debug.Log("OnC");
        GetCurrState().OnC?.Invoke();
    }

    private void OnV(InputValue value)
    {
        Debug.Log("OnV");
        GetCurrState().OnV?.Invoke();
    }

    private void OnR(InputValue value)
    {
        Debug.Log("OnR");
        GetCurrState().OnR?.Invoke();
    }
    #endregion
}
