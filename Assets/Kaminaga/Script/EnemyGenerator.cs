using UnityEngine;

public enum EnemyGeneratorState
{
    Easy,
    Normal,
    Hard,
}
public class EnemyGenerator : MonoBehaviour
{
    private GameObject _hopperPrefab;
    private GameObject _cowPrefab;
    private GameObject _elephantPrefab;
    private GameObject _player;
    [SerializeField] private GameObject _spawnPositionLef; // �����ʒu�̍��W�擾�p
    [SerializeField] private GameObject _spawnPositionRig; // �����ʒu�̍��W�擾�p    
    private GameObject _spawnEffect; // �����ʒu��ǂ��G�t�F�N�g�p
    private Vector3 _spawnPositionCenter;
    private Vector3 _spawnArea;
    private Vector3 _spawnDirection;
    private int _spawnTimer;
    private const int kEasyInterval = 300; // �E��E�Փx�E�C�E�[�E�W�E�[�E�̐��E��E��E�Ԋu
    private const int kNormalInterval = 200; // �E��E�Փx�E�m�E�[�E�}�E��E��E�̐��E��E��E�Ԋu
    private const int kHardInterval = 100; // �E��E�Փx�E�n�E�[�E�h�E�̐��E��E��E�Ԋu
    private const int kEffectMoveDuration = 100; // �E�G�E�t�E�F�E�N�E�g�E��E��E��E��E��E��E�ʒu�E�Ɉړ��E��E��E�鎞��E�
    private const int kEffectStopDuration = 25; // �E�G�E�t�E�F�E�N�E�g�E��E��E�~�E�܂鎞�E��E�
    private int _effectStopTimer;
    private bool _isSpawning;
    private bool _isSpawnRight;
    private EnemyGeneratorState _currentState;
    private int _spawnInterval;
    void Start()
    {
        _hopperPrefab = (GameObject)Resources.Load("Enemy_Hopper");
        _cowPrefab = (GameObject)Resources.Load("Enemy_Cow");
        _elephantPrefab = (GameObject)Resources.Load("Enemy_Elephant");
        _player = GameObject.FindWithTag("Player");
        _spawnEffect = GameObject.Find("SpawnEffect");
        _spawnPositionCenter = _spawnPositionLef.transform.position;
        _spawnArea = Vector3.zero;
        _spawnDirection = Vector3.zero;
        _spawnTimer = 0;
        _effectStopTimer = 0;
        _isSpawning = false;
        _isSpawnRight = false;
        _currentState = EnemyGeneratorState.Easy;
        _spawnInterval = kEasyInterval;
    }

    void FixedUpdate()
    {
        // �E��E�Փx�E�ɉ��E��E��E�Đ��E��E��E�Ԋu�E��E�ύX
        switch (_currentState)
        {
            case EnemyGeneratorState.Easy:
                _spawnInterval = kEasyInterval;
                break;
            case EnemyGeneratorState.Normal:
                _spawnInterval = kNormalInterval;
                break;
            case EnemyGeneratorState.Hard:
                _spawnInterval = kHardInterval;
                break;
        }
        if (!_isSpawning)
        {
            _spawnTimer++;
        }

        if (_spawnTimer >= _spawnInterval)
        {
            SetSpawnPoint();
            _isSpawning = true;
            if (_isSpawnRight)
            {
                _spawnArea = _spawnPositionCenter + new Vector3(Random.Range(-5.5f, -2.0f), 0.0f, Random.Range(-4.0f, 7.0f));
            }
            else
            {
                _spawnArea = _spawnPositionCenter + new Vector3(Random.Range(2.0f, 5.5f), 0.0f, Random.Range(-4.0f, 7.0f));
            }
            _spawnTimer = 0;
        }


        // �E��E��E��E��E�̈ʒu�E�܂ł̕��E��E��E��E��E�擾
        _spawnDirection = (_spawnArea - _spawnPositionCenter) / kEffectMoveDuration;
        if (_isSpawning)
        {
            _spawnEffect.transform.position += _spawnDirection;
        }
        else
        {
            // �E�G�E�t�E�F�E�N�E�g�E��ES�E�ɖ߂�
            _spawnEffect.transform.position = _spawnPositionCenter;
        }

        // �E��E��E��E��E�̈ʒu�E�܂ŃG�E�t�E�F�E�N�E�g�E��E��E�ړ��E��E��E��E��E��E�ɐ��E��E�
        if (_isSpawning && (_spawnArea - _spawnEffect.transform.position).magnitude < 0.1f)
        {
            _spawnEffect.transform.position = _spawnArea; // �E��E��E��E��E�Ȃ��E��E��E��E��E�߂ɒ��E�ڑ��E�
            _effectStopTimer++;
            if (_effectStopTimer > kEffectStopDuration) // �E�G�E�t�E�F�E�N�E�g�E��E��E�~�E�܂��E�Ă��E�班�E��E��E�҂�
            {
                _effectStopTimer = 0;
                if(GameManager.Instance.clearStageNum == 0)
                {
                    Instantiate(_hopperPrefab, _spawnArea, Quaternion.identity);
                }
                else if (GameManager.Instance.clearStageNum == 1)
                {
                    Instantiate(_cowPrefab, _spawnArea, Quaternion.identity);
                }
                else
                {
                    Instantiate(_elephantPrefab, _spawnArea, Quaternion.identity);
                }
                _isSpawning = false;
            }
        }



    }

    void SetSpawnPoint()
    {
        // �E�v�E��E��E�C�E��E��E�[�E��E��E��E��E�ԋ߂��E��E��E��E��E�ʒu�E��ES�E�ɐݒ�
        if ((_player.transform.position - _spawnPositionLef.transform.position).magnitude > (_player.transform.position - _spawnPositionRig.transform.position).magnitude)
        {
            _spawnPositionCenter = _spawnPositionRig.transform.position;
            _isSpawnRight = true;
        }
        else
        {
            _spawnPositionCenter = _spawnPositionLef.transform.position;
            _isSpawnRight = false;
        }
        _spawnEffect.transform.position = _spawnPositionCenter;
    }
}
