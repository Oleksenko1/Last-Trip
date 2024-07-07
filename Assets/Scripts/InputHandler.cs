using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    [Tooltip ("Returns Vector2 that reprsents player's WASD input")]
    public UnityEvent<Vector2> OnMovePlayer = new UnityEvent<Vector2>();
    [Tooltip("Invokes on pressing ESC")]
    public UnityEvent OnESCPressed = new UnityEvent();

    private void Update()
    {
        GetMovePlayer();
        is_ESC_pressed();
    }
    public void GetMovePlayer()
    {
        Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        OnMovePlayer?.Invoke(moveVector.normalized);
    }
    public void is_ESC_pressed()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnESCPressed?.Invoke();
        }
    }
}
