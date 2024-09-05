using UnityEditor;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using System;

public class PlayerInputSetter : MonoBehaviour
{
    private Movement movement;
    private PlayerInput PlayerInput;
    void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        var players = FindObjectsOfType<Movement>();
        var index = PlayerInput.playerIndex;
        movement = players.FirstOrDefault(m => m.GetPlayerIndex() == index);
    }

    public void OnMove(CallbackContext context)
    {
        movement.SetInputVector(context.ReadValue<Vector2>());
    }

    public void OnPull(CallbackContext context)
    {
        if (context.performed) 
        {
            movement.SetPull(true);
        }
        else if (context.canceled)
        {
            movement.SetPull(false);
        }  
    }

    public void OnPush()
    {
        movement.Push();
    }
}
