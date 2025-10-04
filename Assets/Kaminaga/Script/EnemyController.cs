using UnityEngine;


public class EnemyController : MonoBehaviour
{
    private GameObject _player;
    private Vector3 _playerDistance;
    private Vector3 _lookPlayer;
    private Vector3 _moveDirection;
    private float _moveSpeed;
    private const float _scareDistance = 3.0f;
    private const float _scareCancelDistance = 5.0f;
    private bool _isScared;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.Find("Player");
        _playerDistance = Vector3.zero;
        _lookPlayer = Vector3.zero;
        _moveDirection = Vector3.zero;
        _moveSpeed = 0.01f;
        _isScared = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        Debug.Log(_moveDirection);
        _playerDistance = _player.transform.position - transform.position;

        if (_playerDistance.magnitude < _scareDistance) // プレイヤーの距離が一定以下なら逃げる
        {
            _isScared = true;
        }
        else if (_playerDistance.magnitude > _scareCancelDistance) // プレイヤーの距離が一定以上なら逃げるのをやめる
        {
            _isScared = false;
        }

        if (_isScared)
        {
            _moveDirection = -_playerDistance.normalized;
            _moveDirection.y = 0.0f;
        }
        else
        {
            _moveDirection = Vector3.zero;
        }
        _lookPlayer = _playerDistance.normalized;
        _lookPlayer.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(_lookPlayer);
        transform.position += _moveDirection * _moveSpeed;
    }

    public void EnemyDead()
    {
        Destroy(gameObject);
    }

}
