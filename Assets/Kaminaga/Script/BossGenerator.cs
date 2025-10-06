using UnityEngine;

public enum BossGeneratorState
{
    Easy,
    Normal,
    Hard,
}

public class BossGenerator : MonoBehaviour
{
    private GameObject _hopperPrefab;
    private GameObject _cowPrefab;
    private GameObject _elephantPrefab;
    [SerializeField] private GameObject _spawnPosition; // �����ʒu�̍��W�擾�p
    private GameObject _spawnEffect; // �����ʒu��ǂ��G�t�F�N�g�p
    private Vector3 _spawnArea;
    private Vector3 _spawnDirection;
    private int _spawnTimer;
    private int _spawnInterval;
    private int _kEffectMoveDuration; // �G�t�F�N�g�������ʒu�Ɉړ����鎞��
    private int _effectStopDuration; // �G�t�F�N�g���~�܂鎞��
    private int _effectStopTimer;
    private bool _isSpawning;
    private bool _isFirstSpawn;
    public int _bossCount;
    private BossGeneratorState _currentState;
    private const int kEasyInterval = 600;
    private const int kNormalInterval = 400;
    private const int kHardInterval = 200;
    private const int kEffectMoveDuration = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _hopperPrefab = (GameObject)Resources.Load("Boss_Hopper");
        _cowPrefab = (GameObject)Resources.Load("Boss_Cow");
        _elephantPrefab = (GameObject)Resources.Load("Boss_Elephant");
        _spawnEffect = GameObject.Find("SpawnEffectBoss");
        _spawnArea = Vector3.zero;
        _spawnDirection = Vector3.zero;
        _spawnTimer = 0;
        _spawnInterval = kEasyInterval;
        _kEffectMoveDuration = kEffectMoveDuration; // �G�t�F�N�g�������ʒu�Ɉړ����鎞��
        _effectStopDuration = 25;
        _effectStopTimer = 0;
        _isSpawning = false;
        _isFirstSpawn = false;
        _bossCount = 0;
        _currentState = BossGeneratorState.Easy;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (GameManager.Instance.clearStageNum)
        {
            case 0:
                _currentState = BossGeneratorState.Easy;
                break;
            case 1:
                _currentState = BossGeneratorState.Normal;
                break;
            case 2:
                _currentState = BossGeneratorState.Hard;
                break;
            case 3:
                _currentState = BossGeneratorState.Hard;
                break;
            default:
                _currentState = BossGeneratorState.Hard;
                break;
        }
        // ��Փx�ɉ����Đ����Ԋu��ύX
        switch (_currentState)
        {
            case BossGeneratorState.Easy:
                _spawnInterval = kEasyInterval;
                break;
            case BossGeneratorState.Normal:
                _spawnInterval = kNormalInterval;
                break;
            case BossGeneratorState.Hard:
                _spawnInterval = kHardInterval;
                if(!_isFirstSpawn)
                {
                    _bossCount++;
                    Instantiate(_elephantPrefab, new Vector3(0.0f, 1.0f, 3.0f), Quaternion.identity);
                    _spawnTimer = _spawnInterval / 2; // �ŏ��̐����𑁂߂�
                    _isFirstSpawn = true;
                }
                break;
        }
        if (!_isSpawning)
        {
            _spawnTimer++;
        }
        if (_spawnTimer >= _spawnInterval)
        {
            _isSpawning = true;
            _spawnArea = _spawnPosition.transform.position + new Vector3(0.0f, 0.0f, Random.Range(-2.0f, -5.0f));
            _spawnTimer = 0;
        }
        // �����̈ʒu�܂ł̕������擾
        _spawnDirection = (_spawnArea - _spawnPosition.transform.position) / _kEffectMoveDuration;
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
                if(_bossCount >= 15)
                {
                    // 15�̐��������炻��ȏ㐶�����Ȃ�
                    _isSpawning = false;
                    _effectStopTimer = 0;
                    return;
                }
                // ��Փx�ɉ����Đ�������G��ύX
                if (GameManager.Instance.clearStageNum == 0)
                {
                    _bossCount++;
                    Instantiate(_hopperPrefab, _spawnArea, Quaternion.identity);
                }
                else if (GameManager.Instance.clearStageNum == 1)
                {
                    _bossCount++;
                    Instantiate(_cowPrefab, _spawnArea, Quaternion.identity);
                }
                else
                {
                    _bossCount++;
                    Instantiate(_elephantPrefab, _spawnArea, Quaternion.identity);
                }
                _isSpawning = false;
                _effectStopTimer = 0;
            }
        }
    }
}
