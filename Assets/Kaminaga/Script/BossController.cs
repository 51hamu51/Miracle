using UnityEngine;

public enum BossState
{
    Idle,
    Move,
    Attack,
    Rush,
    Evolve,
    Dead
}

public class BossController : MonoBehaviour
{
    private GameObject _player;
    private Vector3 _playerDistance;
    private Vector3 _moveDirection;
    private Vector3 _lookPlayer;
    //private Material _myMaterial;
    private float _moveSpeed;
    private BossState _currentState;
    public BossState CurrentState { get { return _currentState; } }
    private int _loopTimer;
    private int _rushTimer;
    public bool _isAttack;
    private bool _isEvolve;
    private int _attackCounter;
    private int _maxAttackCount;
    private int _attackDuration;
    private int _rushDuration;
    private int _rushStopDuration;
    private int _waitDuration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.Find("Player");
        //_myMaterial = GetComponent<Renderer>().material;
        _playerDistance = Vector3.zero;
        _moveDirection = Vector3.zero;
        _lookPlayer = Vector3.zero;
        _moveSpeed = 0.01f;
        _currentState = BossState.Move;
        _loopTimer = 0;
        _rushTimer = 0;
        _isAttack = false;
        _isEvolve = false;
        _attackCounter = 0;
        _maxAttackCount = 3;
        _attackDuration = 40;
        _rushDuration = 120;
        _rushStopDuration = 150;
        _waitDuration = 100;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _playerDistance = _player.transform.position - transform.position;
        _lookPlayer = _playerDistance.normalized;
        _lookPlayer.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(_lookPlayer) * Quaternion.AngleAxis(270.0f, new Vector3(0.0f, 1.0f, 0.0f));


        if (Input.GetKeyDown(KeyCode.K)) // ボスが強い状態になる
        {
            _currentState = BossState.Evolve;
        }

        switch (_currentState)
        {
            case BossState.Idle:
                _moveDirection = Vector3.zero;
                break;
            case BossState.Move:
                _loopTimer++;
                //_myMaterial.color = Color.green;
                _moveDirection = _playerDistance.normalized;

                if (_loopTimer == _waitDuration)
                {
                    _isAttack = true;
                    _loopTimer = 0;
                    if (_attackCounter == _maxAttackCount)
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
                //_myMaterial.color = Color.red;
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
                //_myMaterial.color = Color.blue;

                if (_rushTimer < _waitDuration)
                {
                    _moveDirection = -_playerDistance.normalized;
                }
                else
                {
                    _moveDirection = _playerDistance.normalized;
                    _moveSpeed = 0.30f;
                }
                if (_rushTimer >= _rushDuration)
                {
                    _moveSpeed = 0.01f;
                }
                if(_rushTimer >= _rushStopDuration)
                {
                    _isAttack = false;
                    _rushTimer = 0;
                    _currentState = BossState.Move;
                }
                break;
            case BossState.Evolve:
                if (!_isEvolve)
                {
                    BossEvolve();
                }
                break;
            case BossState.Dead:
                Destroy(gameObject);
                break;
        }
        _moveDirection.y = 0.0f;
        transform.position += _moveDirection * _moveSpeed;
        //GetComponent<Renderer>().material = _myMaterial;
    }
    public void BossDead()
    {
        Destroy(gameObject);
    }

    private void BossEvolve()
    {
        _moveSpeed = 0.03f;
        //_myMaterial.color = Color.magenta;
        _maxAttackCount = 2;
        _attackDuration = 60;
        _rushDuration = 100;
        _waitDuration = 70;
        _currentState = BossState.Move;
        _isEvolve = true;
    }
}
