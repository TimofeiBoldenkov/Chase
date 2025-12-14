using UnityEngine;
using UnityEngine.InputSystem;

public class Zoom : MonoBehaviour
{
    private Camera _mainCamera;
    private float _zoom;
    public float ZoomMultiplier = 2;
    public float MinZoom = 5;
    public float MaxZoom = 50;
    public float SmoothTime = 0.14f;
    private float _velocity = 0;

    void Awake()
    {
        _mainCamera = Camera.main;
        _zoom = _mainCamera.orthographicSize;
    }

    void Start()
    {

    }

    void Update()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;
        _zoom -= scroll * ZoomMultiplier;
        _zoom = Mathf.Clamp(_zoom, MinZoom, MaxZoom);
        _mainCamera.orthographicSize = Mathf.SmoothDamp(_mainCamera.orthographicSize, _zoom, ref _velocity, SmoothTime);
    }
}
