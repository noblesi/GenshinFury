using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // ���� ��� (�÷��̾�)
    [SerializeField] private Vector3 offset = new Vector3(0, 10, -10);
    [SerializeField] private float smoothSpeed = 0.125f; // �ε巯�� �̵� �ӵ�

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // ���ϴ� ī�޶� ��ġ ���
        Vector3 desiredPosition = target.position + offset;
        // �ε巯�� ī�޶� �̵�
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // �׻� �÷��̾ �ٶ󺸵��� ȸ��
        transform.LookAt(target);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
