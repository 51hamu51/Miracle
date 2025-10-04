using UnityEngine;

public enum BossGeneratorState
{
    Easy,
    Normal,
    Hard,
}

public class BossGenerator : MonoBehaviour
{
    private GameObject _bossPrefab;
    [SerializeField] private GameObject _spawnPosition; // �����ʒu�̍��W�擾�p
    private GameObject _spawnEffect; // �����ʒu��ǂ��G�t�F�N�g�p
    private Vector3 _spawnArea;
    private Vector3 _spawnDirection;
    private int _spawnTimer;
    private int _spawnInterval;
    private int _effectMoveDuration; // �G�t�F�N�g�������ʒu�Ɉړ����鎞��
    private int _effectStopDuration; // �G�t�F�N�g���~�܂鎞��
    private int _effectStopTimer;
    private bool _isSpawning;
    private BossGeneratorState _currentState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _bossPrefab = (GameObject)Resources.Load("BossEnemy");
        _spawnEffect = GameObject.Find("SpawnEffectBoss");
        _spawnArea = Vector3.zero;
        _spawnDirection = Vector3.zero;
        _spawnTimer = 0;
        _spawnInterval = 600;
        _effectMoveDuration = 100; // �G�t�F�N�g�������ʒu�Ɉړ����鎞��
        _effectStopDuration = 25;
        _effectStopTimer = 0;
        _isSpawning = false;
        _currentState = BossGeneratorState.Easy;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // ��Փx�ɉ����Đ����Ԋu��ύX
        switch (_currentState)
        {
            case BossGeneratorState.Easy:
                _spawnInterval = 600;
                break;
            case BossGeneratorState.Normal:
                _spawnInterval = 400;
                break;
            case BossGeneratorState.Hard:
                _spawnInterval = 200;
                break;
        }
        if (!_isSpawning)
        {
            _spawnTimer++;
        }
        if (_spawnTimer >= _spawnInterval)
        {
            _isSpawning = true;
            _spawnArea = _spawnPosition.transform.position + new Vector3(0.0f,0.0f,Random.Range(-2.0f,-5.0f));
            _spawnTimer = 0;
        }
        // �����̈ʒu�܂ł̕������擾
        _spawnDirection = (_spawnArea - _spawnPosition.transform.position) / _effectMoveDuration;
        if (_isSpawning)
        {
            _spawnEffect.transform.position += _spawnDirection;
        }
        else
        {
            _spawnEffect.transform.position = _spawnPosition.transform.position;
        }

        if (_isSpawning && (_spawnEffect.transform.position - _spawnArea).magnitude < 0.1f)
        {
            _spawnEffect.transform.position = _spawnArea;
            _effectStopTimer++;
            if (_effectStopTimer >= _effectStopDuration)
            {
                Instantiate(_bossPrefab, _spawnArea, Quaternion.identity);
                _isSpawning = false;
                _effectStopTimer = 0;
            }
        }
    }
}
