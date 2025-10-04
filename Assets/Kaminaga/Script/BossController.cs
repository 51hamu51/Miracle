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
    public bool _isAttack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.Find("Player");
        _myMaterial = GetComponent<Renderer>().material;
        _playerDistance = Vector3.zero;
        _moveDirection = Vector3.zero;
        _moveSpeed = 0.01f;
        _currentState = BossState.Move;
        _counter = 0;
        _isAttack = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _playerDistance = _player.transform.position - transform.position;
        transform.LookAt(_player.transform);
        Debug.Log("�������Ă���G�̏��:" + _currentState.ToString());


        if (Input.GetKeyDown(KeyCode.K))
        {
            _currentState = BossState.Move;
        }



        switch (_currentState)
        {
            case BossState.Idle:
                _moveDirection = Vector3.zero;
                break;
            case BossState.Move:
                _counter++;
                _myMaterial.color = Color.green;
                _moveDirection = _playerDistance.normalized;

                if (_counter == 100)
                {
                    _isAttack = true;
                    _counter = 0;
                    Debug.Log("�U��!");
                    _currentState = BossState.Attack;
                }
                break;
            case BossState.Attack:
                _myMaterial.color = Color.red;
                _counter++;
                if (_counter == 50)
                {
                    _isAttack = false;
                    _counter = 0;
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
