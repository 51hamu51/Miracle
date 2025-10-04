using System;
using UnityEngine;

public class WASD : MonoBehaviour
{
    //移動速度
    [SerializeField] private float _speed = 3.0f;

    private Rigidbody _rigidbody;
    private PlayerManager _playerManager;

    private Vector3 _velocity;
    
    private void Start() {
        _playerManager = GetComponent<PlayerManager>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!_playerManager.CanMove) {
            return;
        }
        //x軸方向、z軸方向の入力を取得
        //Horizontal、水平、横方向のイメージ
        float _input_x = Input.GetAxisRaw("Horizontal");
        //Vertical、垂直、縦方向のイメージ
        float _input_z = Input.GetAxisRaw("Vertical");
        //移動の向きなど座標関連はVector3で扱う
        _velocity = new Vector3(_input_x, 0f, _input_z);

        //目線のためGetAxisが良い
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
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