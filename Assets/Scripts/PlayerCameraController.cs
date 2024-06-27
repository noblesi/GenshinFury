using UnityEngine;
using Cinemachine;

public class PlayerCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // 시네머신 가상 카메라 참조

    void Start()
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = transform;
            // Transposer 설정: 쿼터뷰를 위해 카메라 위치 설정
            var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset = new Vector3(0, 10, -10); // 적절한 오프셋으로 설정
            transposer.m_XDamping = 1f; // X축 감쇠 값
            transposer.m_YDamping = 1f; // Y축 감쇠 값
            transposer.m_ZDamping = 1f; // Z축 감쇠 값

            // Composer 설정: 카메라 각도 설정
            var composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
            composer.m_TrackedObjectOffset = new Vector3(0, 1, 0); // 적절한 오프셋으로 설정
            composer.m_DeadZoneWidth = 0.1f; // 데드 존 너비
            composer.m_DeadZoneHeight = 0.1f; // 데드 존 높이
            composer.m_SoftZoneWidth = 0.8f; // 소프트 존 너비
            composer.m_SoftZoneHeight = 0.8f; // 소프트 존 높이
            composer.m_BiasX = 0f; // X축 바이어스
            composer.m_BiasY = 0f; // Y축 바이어스
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera not assigned.");
        }
    }

}
