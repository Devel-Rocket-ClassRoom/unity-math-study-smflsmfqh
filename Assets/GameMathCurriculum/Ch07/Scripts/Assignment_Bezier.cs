using NUnit.Framework;
using System.Security.Cryptography;
using UnityEngine;
using static BezierCurveDemo;
using static System.Security.Cryptography.ECCurve;
using System.Collections.Generic;
using Unity.VisualScripting;

// TrailRenderer 사용해서 렌더링

public class NewMonoBehaviourScript : MonoBehaviour
{
    private class Sphere
    {
        public GameObject sphereObject;
        public Vector3 p0;
        public Vector3 p1;
        public Vector3 p2;
        public Vector3 p3;

        public float moveSpeed;
        public float moveT;
    }
    public enum BezierType { Linear, Quadratic, Cubic }

    [Header("=== 곡선 설정 ===")]
    [Tooltip("베지어 곡선 종류 (선형/이차/삼차)")]
    [SerializeField] private BezierType curveType = BezierType.Cubic;

    [Tooltip("제어점 0 (곡선 시작)")]
    [SerializeField] private Vector3 p0 = new Vector3(-3f, 0f, 0f);

    [Tooltip("제어점 1")]
    [SerializeField] private Vector3 p1 = new Vector3(-1f, 3f, 0f);

    [Tooltip("제어점 2")]
    [SerializeField] private Vector3 p2 = new Vector3(1f, 3f, 0f);

    [Tooltip("제어점 3 (곡선 끝)")]
    [SerializeField] private Vector3 p3 = new Vector3(3f, 0f, 0f);

    [Tooltip("곡선을 따라 이동하는 속도")]
    [SerializeField] private float speed = 0.5f;

    [Header("=== 시각화 설정 ===")]
    [SerializeField] private Color colorCurve = Color.cyan;
    [SerializeField] private Color colorControlPoints = Color.yellow;
    [SerializeField] private Color colorMovingPoint = Color.green;
    [SerializeField] private Color colorControlLines = Color.gray;

    private List<Sphere> spheres;
    private Vector3 startPos;
    private Vector3 endPos;
    private float currentT;
    private const int count = 20;

    private void Awake()
    {
        spheres = new List<Sphere>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < count; i++)
            {
                spheres.Add(SetSphere());
            }
        }

        foreach (Sphere sphere in spheres)
        {
            sphere.moveT += Time.deltaTime * sphere.moveSpeed;
            sphere.sphereObject.transform.position = CubicBezier(sphere.p0, sphere.p1, sphere.p2, sphere.p3, sphere.moveT) ;

            if (sphere.moveT >= 1f)
            { 
                Destroy(sphere.sphereObject);
                spheres.Remove(sphere);

            }
        }
    }

    private Sphere SetSphere()
    {
        Sphere sphere = new Sphere();
        sphere.sphereObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.p0 = startPos;
        sphere.p3 = endPos;
        SetRandomP1P2(p1, p2);
        sphere.p2 = p2;
        sphere.p1 = p1;

        sphere.moveSpeed = Random.value;
        sphere.moveT = 0f;

        return sphere;
    }

    private void SetRandomP1P2(Vector3 p1, Vector3 p2)
    {
        Vector3 randomP1 = new Vector3(Random.Range(p0.x, p2.x), Random.Range(p0.y, p2.y), p1.z);
        Vector3 randomP2 = new Vector3(Random.Range(randomP1.x, p3.x), Random.Range(randomP1.y, p3.y), p2.z);

        p1 = randomP1;
        p2 = randomP2;
    }

    private Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 a = Vector3.Lerp(p0, p2, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(d, e, t);
    }

    private void OnDrawGizmos()
    {
        if (!enabled) return;

        float cpSize = 0.15f;
        Vector3 labelOffset = Vector3.up * 0.3f;

        Gizmos.color = colorControlPoints;
        Gizmos.DrawSphere(p0, cpSize);

        switch (curveType)
        {
            case BezierType.Linear:
                Gizmos.DrawSphere(p3, cpSize);
                break;

            case BezierType.Quadratic:
                Gizmos.DrawSphere(p1, cpSize);
                Gizmos.DrawSphere(p2, cpSize);
                Gizmos.color = colorControlLines;
                Gizmos.DrawLine(p0, p1);
                Gizmos.DrawLine(p1, p2);
                break;

            case BezierType.Cubic:
                Gizmos.DrawSphere(p1, cpSize);
                Gizmos.DrawSphere(p2, cpSize);
                Gizmos.DrawSphere(p3, cpSize);
                Gizmos.color = colorControlLines;
                Gizmos.DrawLine(p0, p1);
                Gizmos.DrawLine(p1, p2);
                Gizmos.DrawLine(p2, p3);
                break;
        }
    }
}