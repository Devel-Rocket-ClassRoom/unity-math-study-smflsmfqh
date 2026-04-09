using System.Linq;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private readonly string selectableTag = "Selectable";
    private readonly string groundTag = "Ground";

    private Camera cam;
    private GameObject selectedObject;
    private bool isDragging;
    private float yOffset;
    private Vector3 mousePos;
    private Vector3 pos;

    private void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("[RaycastDemo] MainCamera를 찾을 수 없습니다. " +
                "카메라에 'MainCamera' 태그를 추가하세요.");
            enabled = false;
        }

        yOffset = Terrain.activeTerrain.SampleHeight(transform.position);
        pos = transform.position;
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
                Debug.Log($"[Raycast] 선택됨: {selectedObject.name} at {hit.point}");
                isDragging = true;
            }
                
        }
        if (isDragging)
        {
            int maskG = LayerMask.GetMask("Ground");
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, maskG))
            {
                Debug.Log("땅 충돌됨");
                mousePos = Input.mousePosition;
                mousePos.z = 10f;
                
                Vector3 newPos = cam.ScreenToWorldPoint(mousePos);
                //newPos.y += yOffset;
                selectedObject.transform.position = newPos;
                
            }
        }
    }


}
