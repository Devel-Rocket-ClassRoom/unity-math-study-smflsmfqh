using System.Linq;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Camera cam;
    private GameObject selectedObject;
    private bool isDragging;
    private bool isClicked;
    private const float yOffset = 7f;
    private Vector3 mousePos;
    private Vector3 targetPos;
    private Vector3 initialPos;

    private void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("[RaycastDemo] MainCamera를 찾을 수 없습니다. " +
                "카메라에 'MainCamera' 태그를 추가하세요.");
            enabled = false;
        }

        isDragging = false;
    }

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !isDragging)
        {
            int maskT = LayerMask.GetMask("Target");
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, maskT))
            {
                selectedObject = hit.collider.gameObject;
                initialPos = selectedObject.transform.position;
                Debug.Log($"[Raycast] 선택됨: {selectedObject.name} at {hit.point}");
                isDragging = true;
            }   
        }
        if (isDragging && selectedObject != null)
        {
            int maskG = LayerMask.GetMask("Ground");
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, maskG))
            {
                targetPos = hit.point;
                targetPos.y += yOffset;
                selectedObject.transform.position = targetPos;
            }
        }
    
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            int maskD = LayerMask.GetMask("DropPoint");
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, maskD))
            {
                targetPos = hit.point;
                targetPos.y += yOffset;
                selectedObject.transform.position = targetPos;
            }
            else
            {
                selectedObject.transform.position = initialPos;
            }
            isDragging = false;
        }
    }

}
