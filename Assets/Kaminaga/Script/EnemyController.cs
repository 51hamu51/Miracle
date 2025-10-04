using UnityEngine;


public class EnemyController : MonoBehaviour
{
    private GameObject _player;
    private Vector3 _playerDistance;
    private Vector3 _lookPlayer;
    private Vector3 _moveDirection;
    private float _moveSpeed;
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

        if (_playerDistance.magnitude < 2.0f)
        {
            _isScared = true;
        }
        else if (_playerDistance.magnitude > 5.0f)
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
