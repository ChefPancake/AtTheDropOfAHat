using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {
    private const string HORIZONTAL_AXIS_NAME = "Horizontal";
    private const string VERTICAL_AXIS_NAME = "Vertical";

    [SerializeField]
    private float _moveSpeed;
    
    private Vector2 _moveInput;
    private Rigidbody2D _rigidBody;
    
    private void OnMove(InputValue input) =>
        _moveInput = input.Get<Vector2>();

    private void OnJump() =>
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 10f);

    private void Start() {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    
    private void Update() {
        var newVel = new Vector3(
            _moveInput.x * _moveSpeed, 
            _rigidBody.velocity.y, 
            0);
        _rigidBody.velocity = newVel;
    }
}
