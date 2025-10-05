using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    private GameObject _enemyPrefab;
    private GameObject _player;
    [SerializeField] private GameObject _spawnPositionLef; // �����ʒu�̍��W�擾�p
    [SerializeField] private GameObject _spawnPositionRig; // �����ʒu�̍��W�擾�p    
    private GameObject _spawnEffect; // �����ʒu��ǂ��G�t�F�N�g�p
    private Vector3 _spawnPositionCenter;
    private Vector3 _spawnArea;
    private Vector3 _spawnDirection;
    private int _spawnTimer;
    private const int _spawnInterval = 300;
    private const int _effectMoveDuration = 100; // �G�t�F�N�g�������ʒu�Ɉړ����鎞��
    private const int _effectStopDuration = 25; // �G�t�F�N�g���~�܂鎞��
    private int _effectStopTimer;
    private bool _isSpawning;
    private bool _isSpawnRight;
    void Start()
    {
        _enemyPrefab = (GameObject)Resources.Load("ScareEnemy");
        _player = GameObject.FindWithTag("Player");
        _spawnEffect = GameObject.Find("SpawnEffect");
        _spawnPositionCenter = _spawnPositionLef.transform.position;
        _spawnArea = Vector3.zero;
        _spawnDirection = Vector3.zero;
        _spawnTimer = 0;
        _effectStopTimer = 0;
        _isSpawning = false;
        _isSpawnRight = false;
    }

    void FixedUpdate()
    {
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


        // �����̈ʒu�܂ł̕������擾
        _spawnDirection = (_spawnArea - _spawnPositionCenter) / _effectMoveDuration;
        if (_isSpawning)
        {
            Debug.DrawRay(_spawnPositionCenter, (_spawnArea - _spawnPositionCenter), Color.red);
            _spawnEffect.transform.position += _spawnDirection;
        }
        else
        {
            // �G�t�F�N�g�𒆐S�ɖ߂�
            _spawnEffect.transform.position = _spawnPositionCenter;
        }

        // �����̈ʒu�܂ŃG�t�F�N�g���ړ�������ɐ���
        if (_isSpawning && (_spawnArea - _spawnEffect.transform.position).magnitude < 0.1f)
        {
            _spawnEffect.transform.position = _spawnArea; // �덷���Ȃ������߂ɒ��ڑ��
            _effectStopTimer++;
            if (_effectStopTimer > _effectStopDuration) // �G�t�F�N�g���~�܂��Ă��班���҂�
            {
                _effectStopTimer = 0;
                Instantiate(_enemyPrefab, _spawnArea, Quaternion.identity);
                _isSpawning = false;
            }
        }



    }

    void SetSpawnPoint()
    {
        // �v���C���[�����ԋ߂������ʒu�𒆐S�ɐݒ�
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
