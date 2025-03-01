using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraControllerPanel cameraControllerPanel;
    [SerializeField] private float _sensitivity = 2.0f;
    [SerializeField] private float _sensitivityPanelRotate = 1;
    [SerializeField] private float _maxYAngle = 80.0f;
    [SerializeField] private float _rotationX = 0.0f;

    public float rotationSpeed = 0.1f; // Чувствительность вращения
    private Vector2 touchStartPos; // Позиция первого касания

    private bool _isMobile;

    private void Start()
    {
        _isMobile = Application.isMobilePlatform;
    }

    private void Update()
    {
        float mouseX = 0;
        float mouseY = 0;

        if (_isMobile)
        {
            if (cameraControllerPanel.Pressed)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.fingerId == cameraControllerPanel.FingerId)
                    {
                        if (touch.phase == TouchPhase.Moved)
                        {
                            mouseY = touch.deltaPosition.y * _sensitivityPanelRotate;
                            mouseX = touch.deltaPosition.x * _sensitivityPanelRotate;
                        }

                        if (touch.phase == TouchPhase.Stationary)
                        {
                            mouseY = 0;
                            mouseX = 0;
                        }
                    }
                }
            }
        }
        else
        {
            if (cameraControllerPanel.Pressed)
            {
                mouseX = Input.GetAxis("Mouse X") * _sensitivity;
                mouseY = Input.GetAxis("Mouse Y") * _sensitivity;
            }
        }

        transform.parent.Rotate(Vector3.up * mouseX * _sensitivity);

        _rotationX -= mouseY * _sensitivity;
        _rotationX = Mathf.Clamp(_rotationX, -_maxYAngle, _maxYAngle);
        transform.localRotation = Quaternion.Euler(_rotationX, 0.0f, 0.0f);
    }
}
