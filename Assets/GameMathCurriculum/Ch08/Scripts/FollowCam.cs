using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [Header("=== 추적 대상 ===")]
    [Tooltip("카메라가 따라갈 플레이어(타겟)")]
    [SerializeField] private Transform target;

    [Tooltip("타겟으로부터 카메라의 오프셋(상대 위치)")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -8f);

    [Header("=== SmoothDamp 설정 ===")]
    [Tooltip("위치 보간 부드러움 정도 (초, 작을수록 빠름)")]
    [Range(0.01f, 1f)]
    [SerializeField] private float positionSmoothTime = 0.3f;

    [Tooltip("줌 거리 보간 부드러움 정도")]
    [Range(0.01f, 1f)]
    [SerializeField] private float zoomSmoothTime = 0.2f;

    [Tooltip("회전 보간 속도 (높을수록 빠르게 회전)")]
    [Range(1f, 20f)]
    [SerializeField] private float rotationSmoothSpeed = 5f;

    [Header("=== 줌 설정 ===")]
    [Tooltip("최소 줌 거리")]
    [Range(2f, 10f)]
    [SerializeField] private float minZoomDistance = 3f;

    [Tooltip("최대 줌 거리")]
    [Range(10f, 30f)]
    [SerializeField] private float maxZoomDistance = 15f;

    [Tooltip("마우스 휠 줌 속도")]
    [Range(1f, 10f)]
    [SerializeField] private float zoomSpeed = 3f;

    private Vector3 positionVelocity = Vector3.zero;
    private float currentZoomDistance = 10f;
    private float targetZoomDistance;
    private float zoomVelocity = 0f;
    private float maxSpeed = 20f;

    private void Start()
    { 
        targetZoomDistance = currentZoomDistance;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        float t = Time.deltaTime * rotationSmoothSpeed;

        targetZoomDistance -= wheelInput * zoomSpeed;
        targetZoomDistance = Mathf.Clamp(targetZoomDistance, minZoomDistance, maxZoomDistance);

        currentZoomDistance = Mathf.SmoothDamp(
            currentZoomDistance,
            targetZoomDistance,
            ref zoomVelocity,
            zoomSmoothTime);

        Vector3 newPos = target.position + offset.normalized * currentZoomDistance + Vector3.up * offset.y;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            newPos,
            ref positionVelocity,
            positionSmoothTime,
            maxSpeed
            );

        Vector3 look = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
    }
}
