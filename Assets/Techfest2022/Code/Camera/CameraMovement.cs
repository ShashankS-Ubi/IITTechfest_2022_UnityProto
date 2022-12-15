using System;
using UnityEngine;

/// <summary>
/// Camera controller class.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float _panningSpeed = 3f;
    [SerializeField]
    private float _panningMousePositionThreshold = 1f;
    [SerializeField]
    private Vector3 _cameraPanMin = Vector3.zero;
    [SerializeField]
    private Vector3 _cameraPanMax = Vector3.zero;
    [SerializeField]
    private float _zoomSpeedMultiplier = 100f;

    private Vector3 _currentMousePosition = Vector3.zero;
    private Vector3 _currentCamerPosition = Vector3.zero;
    private Vector2 _currentScreenSize = Vector2.zero;
    private Transform _mainCameraTransform = null;

    private bool _allowCameraMovemet = false;
    private bool _isGameInFocus = false;


    public bool AllowPanning => _allowCameraMovemet;
    private void Awake()
    {
        _currentScreenSize.x = Screen.width;
        _currentScreenSize.y = Screen.height;

        _mainCameraTransform = Camera.main.transform;
        _currentCamerPosition = _mainCameraTransform.position;

        ToggleMovement(true);
    }

    private void LateUpdate()
    {
        PanCamera();
        ZoomCamera();
    }


    private void OnApplicationFocus(bool focus)
    {
        ///If you click outside the game's window, it shouldn't continue to
        ///pan into oblivion.
        _isGameInFocus = focus;
    }
    
    /// <summary>
    /// Zooms the camera in and out using mouse scroll.
    /// </summary>
    private void ZoomCamera()
    {
        if (!_allowCameraMovemet || !_isGameInFocus)
            return;

        if (Input.mouseScrollDelta.y == 0f)
            return;

        _currentCamerPosition.y -= Mathf.Sign(Input.mouseScrollDelta.y) * _panningSpeed * Time.deltaTime * _zoomSpeedMultiplier;
        ClampCameraZoom();
        _mainCameraTransform.position = _currentCamerPosition;
    }

    /// <summary>
    /// Pans the camera up-down and sideways based on where the cursor goes outside the screen area.
    /// </summary>
    private void PanCamera()
    {
        if (!_allowCameraMovemet || !_isGameInFocus)
            return;

        _currentMousePosition = Input.mousePosition;
        if (_currentMousePosition.x < _panningMousePositionThreshold || _currentMousePosition.x > _currentScreenSize.x - _panningMousePositionThreshold)
        {
            _currentCamerPosition.x += Mathf.Sign(_currentMousePosition.x) * _panningSpeed * Time.deltaTime;
            _currentCamerPosition.z -= Mathf.Sign(_currentMousePosition.x) * _panningSpeed * Time.deltaTime;
            ClampCameraPanning();

            _mainCameraTransform.position = _currentCamerPosition;
        }

        if (_currentMousePosition.y < _panningMousePositionThreshold || _currentMousePosition.y > _currentScreenSize.y - _panningMousePositionThreshold)
        {
            _currentCamerPosition.x += Mathf.Sign(_currentMousePosition.y) * _panningSpeed * Time.deltaTime;
            _currentCamerPosition.z += Mathf.Sign(_currentMousePosition.y) * _panningSpeed * Time.deltaTime;

            ClampCameraPanning();

            _mainCameraTransform.position = _currentCamerPosition;
        }
    }

    /// <summary>
    /// This will set a boundary for the panning so you don't accidentally 
    /// pan into another infinite space one day.
    /// </summary>
    private void ClampCameraPanning()
    {
        if (_currentCamerPosition.x < _cameraPanMin.x || _currentCamerPosition.x > _cameraPanMax.x
                        || _currentCamerPosition.z < _cameraPanMin.z || _currentCamerPosition.z > _cameraPanMax.z)
        {
            _currentCamerPosition.x = Mathf.Clamp(_currentCamerPosition.x, _cameraPanMin.x, _cameraPanMax.x);
            _currentCamerPosition.z = Mathf.Clamp(_currentCamerPosition.z, _cameraPanMin.z, _cameraPanMax.z);
        }
    }

    /// <summary>
    /// This will set a roof and floor boundary to your zoom level so you don't
    /// see what the ants are doing, but also don't stop caring about your world altogether!
    /// </summary>
    private void ClampCameraZoom()
    {
        if (_currentCamerPosition.y < _cameraPanMin.y || _currentCamerPosition.y > _cameraPanMax.y)
        {
            _currentCamerPosition.y = Mathf.Clamp(_currentCamerPosition.y, _cameraPanMin.y, _cameraPanMax.y);
        }
    }

    /// <summary>
    /// This can be used in a number of cases to stop you from moving camera.
    /// </summary>
    /// <param name="allow"></param>
    public void ToggleMovement(bool allow)
    {
        _allowCameraMovemet = allow;
    }
}
