using UnityEngine;
using UnityEngine.UI;

public class OffscreenDetector: MonoBehaviour
{
    public Transform[] targets;
    public Image[] indicators;

    private Camera cam;
    private Vector3 targetPos;
    private Vector3 screenPoint;

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
            screenPoint = cam.WorldToScreenPoint(targetPos);

            screenPoint.x = Mathf.Clamp(screenPoint.x, margin, Screen.width - margin);
            screenPoint.y = Mathf.Clamp(screenPoint.y, margin, Screen.height - margin);

            if (!IsVisibleTarget(targetPos))
            {
                if (screenPoint.z < 0)
                {
                    screenPoint *= -1f;
                }
                indicators[i].transform.position = screenPoint;

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
        var targetViewport = cam.WorldToViewportPoint(targetPosition);
        if (targetViewport.x < 0 || targetViewport.x > 1 || targetViewport.y < 0 || targetViewport.y > 1 || targetViewport.z < 0)
        {
            return false;
        }
        return true;
    }
}
