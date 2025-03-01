using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _moveSpeed;
    
    void Update()
    {
        Vector3 moveDirection = transform.forward * _joystick.Vertical + transform.right * _joystick.Horizontal;
        moveDirection.y = -2.0f;
        _controller.Move(moveDirection * _moveSpeed * Time.deltaTime);
    }
}