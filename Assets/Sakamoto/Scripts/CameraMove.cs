using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    void Start()
    {

    }
    void Update()
    {
        // プレイヤーの後方に一定距離で追従
        Vector3 targetPos = playerTransform.position;
        targetPos.y += 4;
        targetPos.z -= 6;
        transform.position = targetPos;
    }
}
