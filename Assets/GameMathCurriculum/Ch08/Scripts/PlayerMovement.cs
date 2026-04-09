using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("=== 이동 속도 설정 ===")]
    [Range(1f, 20f)]
    [SerializeField] private float moveSpeed = 5f;

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(h, 0f, v);
        moveDirection.Normalize();

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
