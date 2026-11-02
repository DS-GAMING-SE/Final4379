using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private InputSystem_Actions inputSystemActions;
    public event Action<Vector2> TouchStarted;
    public event Action<Vector2> TouchEnded;
    public Vector2 touchStartPosition { get; private set; }
    public Vector2 touchCurrentPosition { get; private set; }
    public bool touchHeld { get; private set; }

    private void Awake()
    {
        inputSystemActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputSystemActions.Enable();
        inputSystemActions.All.TouchPoint.performed += OnTouchPerformed;
        inputSystemActions.All.TouchPoint.canceled += OnTouchCancelled;
    }
    private void OnTouchPerformed(InputAction.CallbackContext context)
    {
        touchHeld = true;
        touchCurrentPosition = context.ReadValue<Vector2>();
        touchStartPosition = touchCurrentPosition;
        TouchStarted?.Invoke(touchStartPosition);
        Debug.Log("Touch" + touchStartPosition);
    }
    private void OnTouchCancelled(InputAction.CallbackContext context)
    {
        touchHeld = false;
        TouchEnded?.Invoke(touchCurrentPosition);
        Debug.Log("Release" + touchCurrentPosition);
        touchStartPosition = Vector3.zero;
        touchCurrentPosition = Vector3.zero;
    }
    private void Update()
    {
        if (touchHeld)
        {
            touchCurrentPosition = inputSystemActions.All.TouchPoint.ReadValue<Vector2>();
        }
    }
    private void OnDisable()
    {
        inputSystemActions.All.TouchPoint.performed -= OnTouchPerformed;
        inputSystemActions.All.TouchPoint.canceled -= OnTouchCancelled;
        inputSystemActions.Disable();
    }
}
