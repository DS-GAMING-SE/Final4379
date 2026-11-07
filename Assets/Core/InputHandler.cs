using DSGameUtils;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputHandler : MonoBehaviour
{
    private InputSystem_Actions inputSystemActions;
    public static InputHandler instance;
    public event Action<Vector2> TouchStarted;
    public event Action<Vector2> TouchEnded;
    public event Action<Vector2> Swipe;
    public Vector2 touchStartPosition { get; private set; }
    public Vector2 touchCurrentPosition { get; private set; }
    public bool touchHeld { get; private set; }
    public Vector2 touchLastIdlePosition { get; private set; }
    public Vector2 touchVelocity { get; private set; }
    public bool touchIdle { get { return touchVelocity.sqrMagnitude < IDLE_SPEED_SQR; } }
    public Vector2 touchLastHighVelocity { get; private set; }
    public float touchIdleTimer { get; private set; }
    private const float IDLE_SPEED = 1f;
    private const float IDLE_SPEED_SQR = IDLE_SPEED * IDLE_SPEED;
    private const float SWIPE_MIN_SPEED = 2.5f;
    private const float SWIPE_MIN_SPEED_SQR = SWIPE_MIN_SPEED * SWIPE_MIN_SPEED;
    private const float SWIPE_MIN_LENGTH = 6f;
    private const float SWIPE_MIN_LENGTH_SQR = SWIPE_MIN_LENGTH * SWIPE_MIN_LENGTH;
    private const float IDLE_BUFFER_DURATION = 0.35f;
    // ADJUST TO USE SCREEN.DPI INSTEAD OF PIXELS which it's currently using
    private void Awake()
    {
        inputSystemActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputSystemActions.Enable();
        inputSystemActions.All.TouchPoint.performed += OnTouchPerformed;
        inputSystemActions.All.TouchPoint.canceled += OnTouchCancelled;
        Util.SetSingleton(this, ref instance);
    }
    private void OnTouchPerformed(InputAction.CallbackContext context)
    {
        touchHeld = true;
        touchCurrentPosition = context.ReadValue<Vector2>();
        touchStartPosition = touchCurrentPosition;
        touchLastIdlePosition = touchCurrentPosition;
        touchIdleTimer = 0f;
        TouchStarted?.Invoke(touchStartPosition);
        Debug.Log("Touch" + touchStartPosition);
    }
    private void OnTouchCancelled(InputAction.CallbackContext context)
    {
        touchHeld = false;
        TouchEnded?.Invoke(touchCurrentPosition);
        Debug.Log("Release " + touchCurrentPosition);
        if (touchLastHighVelocity.sqrMagnitude >= SWIPE_MIN_SPEED_SQR && touchIdleTimer < IDLE_BUFFER_DURATION && 
            Vector3.SqrMagnitude(touchLastIdlePosition - touchCurrentPosition) >= SWIPE_MIN_LENGTH_SQR)
        {
            Swipe?.Invoke(touchCurrentPosition - touchLastIdlePosition);
            Debug.Log("Swipe Successful");
        }
        Debug.Log("Last High Velocity " + touchLastHighVelocity.sqrMagnitude);
        touchStartPosition = Vector3.zero;
        touchLastIdlePosition = Vector3.zero;
        touchCurrentPosition = Vector3.zero;
    }
    private void Update()
    {
        if (touchHeld)
        {
            touchVelocity = touchCurrentPosition;
            touchCurrentPosition = inputSystemActions.All.TouchPoint.ReadValue<Vector2>();
            touchVelocity -= touchCurrentPosition;
            touchVelocity *= (1 - Time.deltaTime);
            if (touchIdle)
            {
                touchLastIdlePosition = touchCurrentPosition;
                touchIdleTimer += Time.deltaTime;
            }
            else
            {
                touchLastHighVelocity = touchVelocity;
                touchIdleTimer = 0f;
            }
        }
    }
    private void OnDisable()
    {
        inputSystemActions.All.TouchPoint.performed -= OnTouchPerformed;
        inputSystemActions.All.TouchPoint.canceled -= OnTouchCancelled;
        inputSystemActions.Disable();
        Util.UnsetSingleton(this, ref instance);
    }
}
