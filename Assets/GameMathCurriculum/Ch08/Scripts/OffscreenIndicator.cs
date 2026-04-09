using UnityEngine;
using UnityEngine.UI;

public class OffscreenIndicator: MonoBehaviour
{
    public Transform[] targets;
    public Image[] indicators;

    private Camera cam;
    private Vector3 targetPos;
    private Vector3 screenPoint;
    [Header("=== UI 이동 속도 ===")]
    [SerializeField] private float smoothSpeed = 3f;
  
    private const float margin = 50f;

    private void Start()
    {
        cam = Camera.main;
        if (cam == null )
        {
            Debug.LogError("[ScreenToWorldDemo] MainCamera를 찾을 수 없습니다. " +
               "카메라에 'MainCamera' 태그를 추가하세요.");
            enabled = false;
            return;
        }
    }
    private void OnEnable()
    {
        foreach (var image in indicators)
        {
            image.enabled = false;
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue;
            
            targetPos = targets[i].transform.position;

            if (!IsVisibleTarget(targetPos))
            {
                if (screenPoint.z < 0)
                {
                    screenPoint *= -1f;
                }
                screenPoint.x = Mathf.Clamp(screenPoint.x, margin, Screen.width - margin);
                screenPoint.y = Mathf.Clamp(screenPoint.y, margin, Screen.height - margin);

                var currentPos = indicators[i].transform.position; 
                indicators[i].transform.position = Vector3.Lerp(
                    currentPos,
                    screenPoint,
                    Time.deltaTime * smoothSpeed
                );
                indicators[i].enabled = true;
            }
            else
            {
                indicators[i].enabled = false;
            }
        }
    }
    private bool IsVisibleTarget(Vector3 targetPosition)
    {
        screenPoint = cam.WorldToScreenPoint(targetPosition);
        if (screenPoint.x < 0 || screenPoint.x > Screen.width || screenPoint.y < 0 || screenPoint.y > Screen.height || screenPoint.z < 0)
        {
            return false;
        }
        return true;
    }
}
