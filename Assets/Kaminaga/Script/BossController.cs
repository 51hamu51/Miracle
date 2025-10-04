using UnityEngine;

public enum BossState
{
    Idle,
    Move,
    Attack,
    Dead
}

public class BossController : MonoBehaviour
{
    private GameObject _player;
    private Vector3 _playerDistance;
    private Vector3 _moveDirection;
    private Material _myMaterial;
    private float _moveSpeed;
    private BossState _currentState;
    public BossState CurrentState { get { return _currentState; } }
    private int _counter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.Find("Player");
        _myMaterial = GetComponent<Renderer>().material;
        _playerDistance = Vector3.zero;
        _moveDirection = Vector3.zero;
        _moveSpeed = 0.01f;
        _currentState = BossState.Idle;
        _counter = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _playerDistance = _player.transform.position - transform.position;
        transform.LookAt(_player.transform);
        Debug.Log(_currentState);
        if (_counter == 50)
        {
            _counter = 0;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _currentState = BossState.Move;
        }
        if (_playerDistance.magnitude < 2.0f)
        {
            _currentState = BossState.Attack;
        }

        switch (_currentState)
        {
            case BossState.Idle:
                _moveDirection = Vector3.zero;
                break;
            case BossState.Move:
                _myMaterial.color = Color.red;
                _moveDirection = _playerDistance.normalized;
                if (_playerDistance.magnitude < 2.0f)
                {
                    _currentState = BossState.Attack;
                }
                break;
            case BossState.Attack:
                _moveDirection = Vector3.zero;

                if (_counter == 0)
                {
                    Debug.Log("UŒ‚!");
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _currentState = BossState.Dead;
                }
                break;
            case BossState.Dead:
                Destroy(gameObject);
                break;
        }
        transform.position += _moveDirection * _moveSpeed;
    }
}
