using System;
using UnityEngine;

public class WASD : MonoBehaviour
{
    //移動速度
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] public VariableJoystick _joystick;

    private Rigidbody _rigidbody;
    private PlayerManager _playerManager;

    private Vector3 _velocity;
    private bool _useJoystick;
    
    private void Start() {
        _playerManager = GetComponent<PlayerManager>();
        _rigidbody = GetComponent<Rigidbody>();
        _useJoystick = Application.isMobilePlatform;
        _joystick.gameObject.SetActive(_useJoystick);
    }

    void Update()
    {
        if (!_playerManager.CanMove) {
            return;
        }

        //x軸方向、z軸方向の入力を取得
        float _input_x = 0f;
        float _input_z = 0f;

        if (!_useJoystick) {
            //Horizontal、水平、横方向のイメージ
            _input_x = Input.GetAxisRaw("Horizontal");
            //Vertical、垂直、縦方向のイメージ
            _input_z = Input.GetAxisRaw("Vertical");
        }
        else {
            _input_x = _joystick.Horizontal;
            _input_z = _joystick.Vertical;
        }
        
        //移動の向きなど座標関連はVector3で扱う
        _velocity = new Vector3(_input_x, 0f, _input_z);

        //目線のためGetAxisが良い
        float x = !_useJoystick ? Input.GetAxis("Horizontal") : _joystick.Horizontal;
        float z = !_useJoystick ? Input.GetAxis("Vertical") : _joystick.Vertical;
        Vector3 destination = transform.position +  new Vector3(x, 0f, z);

        //移動先に向けて回転
        transform.LookAt(destination);
    }
    
    // FixedUpdate is called at a fixed interval and is used for physics calculations.
    void FixedUpdate() {
        var movement = _velocity.normalized * _speed;
        _rigidbody.linearVelocity = new Vector3(movement.x, _rigidbody.linearVelocity.y, movement.z);
    }
}