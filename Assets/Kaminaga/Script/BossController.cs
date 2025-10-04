using UnityEngine;

public enum BossState
{
    Idle,
    Move,
    Attack,
    Rush,
    Dead
}

public class BossController : MonoBehaviour
{
    private GameObject _player;
    private Vector3 _playerDistance;
    private Vector3 _moveDirection;
    private Vector3 _lookPlayer;
    private Material _myMaterial;
    private float _moveSpeed;
    private BossState _currentState;
    public BossState CurrentState { get { return _currentState; } }
    private int _loopTimer;
    private int _rushTimer;
    public bool _isAttack;
    private int _attackCounter;
    private const int _maxAttackCount = 3;
    private const int _attackDuration = 50;
    private const int _rushDuration = 120;
    private const int _waitDuration = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.Find("Player");
        _myMaterial = GetComponent<Renderer>().material;
        _playerDistance = Vector3.zero;
        _moveDirection = Vector3.zero;
        _lookPlayer = Vector3.zero;
        _moveSpeed = 0.01f;
        _currentState = BossState.Move;
        _loopTimer = 0;
        _rushTimer = 0;
        _isAttack = false;
        _attackCounter = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _playerDistance = _player.transform.position - transform.position;
        _lookPlayer = _playerDistance.normalized;
        _lookPlayer.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(_lookPlayer);
        Debug.Log("�������Ă���G�̏��:" + _currentState.ToString());


        if (Input.GetKeyDown(KeyCode.K))
        {
            _currentState = BossState.Rush;
        }



        switch (_currentState)
        {
            case BossState.Idle:
                _moveDirection = Vector3.zero;
                break;
            case BossState.Move:
                _loopTimer++;
                _myMaterial.color = Color.green;
                _moveDirection = _playerDistance.normalized;

                if (_loopTimer == _waitDuration)
                {
                    _isAttack = true;
                    _loopTimer = 0;
                    Debug.Log("�U��!");
                    if(_attackCounter == _maxAttackCount)
                    {
                        _currentState = BossState.Rush;
                        _attackCounter = 0;
                    }
                    else
                    {
                        _currentState = BossState.Attack;
                    }
                }
                break;
            case BossState.Attack:
                _myMaterial.color = Color.red;
                _loopTimer++;
                if (_loopTimer == _attackDuration)
                {
                    _isAttack = false;
                    _attackCounter++;
                    _loopTimer = 0;
                    _currentState = BossState.Move;
                }
                break;
            case BossState.Rush:
                _rushTimer++;
                _myMaterial.color = Color.blue;

                if (_rushTimer < _waitDuration)
                {
                    _moveDirection = -_playerDistance.normalized;
                }
                else
                {
                    _moveDirection = _playerDistance.normalized;
                    _moveSpeed = 0.30f;
                }
                if(_rushTimer >= _rushDuration)
                {
                    _rushTimer = 0;
                    _moveSpeed = 0.01f;
                    _currentState = BossState.Move;
                }
                    break;
            case BossState.Dead:
                Destroy(gameObject);
                break;
        }
        _moveDirection.y = 0.0f;
        transform.position += _moveDirection * _moveSpeed;
        GetComponent<Renderer>().material = _myMaterial;
    }

    public void BossDead()
    {
        Destroy(gameObject);
    }
}
