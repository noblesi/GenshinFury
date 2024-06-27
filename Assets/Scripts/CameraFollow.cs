using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // 따라갈 대상 (플레이어)
    [SerializeField] private Vector3 offset = new Vector3(0, 10, -10);
    [SerializeField] private float smoothSpeed = 0.125f; // 부드러운 이동 속도

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // 원하는 카메라 위치 계산
        Vector3 desiredPosition = target.position + offset;
        // 부드러운 카메라 이동
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // 항상 플레이어를 바라보도록 회전
        transform.LookAt(target);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
