using UnityEngine;
using UnityEngine.InputSystem;

public class CameraDrag : MonoBehaviour
{
    private InputAction _dragAction;
    private Vector3 _origin;
    private Camera _mainCamera;

    void Awake()
    {
        _mainCamera = Camera.main;
        _dragAction = InputSystem.actions.FindAction("Drag");
    }

    void OnEnable()
    {
        _dragAction.Enable();
        _dragAction.started += OnDragStarted;

    }

    void LateUpdate()
    {
        if (_dragAction.IsPressed())
        {
            transform.position += _origin - GetMousePosition;
        }
    }

    void OnDisable()
    {
        _dragAction.Disable();
        _dragAction.started -= OnDragStarted;
    }

    private void OnDragStarted(InputAction.CallbackContext context)
    {
        _origin = GetMousePosition;
    }

    private Vector3 GetMousePosition => _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
}
