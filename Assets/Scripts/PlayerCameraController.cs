using UnityEngine;
using Cinemachine;

public class PlayerCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // �ó׸ӽ� ���� ī�޶� ����

    void Start()
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = transform;
            // Transposer ����: ���ͺ並 ���� ī�޶� ��ġ ����
            var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset = new Vector3(0, 10, -10); // ������ ���������� ����
            transposer.m_XDamping = 1f; // X�� ���� ��
            transposer.m_YDamping = 1f; // Y�� ���� ��
            transposer.m_ZDamping = 1f; // Z�� ���� ��

            // Composer ����: ī�޶� ���� ����
            var composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
            composer.m_TrackedObjectOffset = new Vector3(0, 1, 0); // ������ ���������� ����
            composer.m_DeadZoneWidth = 0.1f; // ���� �� �ʺ�
            composer.m_DeadZoneHeight = 0.1f; // ���� �� ����
            composer.m_SoftZoneWidth = 0.8f; // ����Ʈ �� �ʺ�
            composer.m_SoftZoneHeight = 0.8f; // ����Ʈ �� ����
            composer.m_BiasX = 0f; // X�� ���̾
            composer.m_BiasY = 0f; // Y�� ���̾
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera not assigned.");
        }
    }

}
