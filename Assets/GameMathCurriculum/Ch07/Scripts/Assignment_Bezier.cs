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
        public TrailRenderer trail;
        public Vector3 p0;
        public Vector3 p1;
        public Vector3 p2;
        public Vector3 p3;

        public float moveSpeed;
        public float moveT;
    }

    private Vector3 startPos = new Vector3(-10f, 0f, 0f);
    private Vector3 endPos = new Vector3(10f, 0f, 0f);

    private List<Sphere> spheres;

    private float minSpeed = 0.5f;
    private float maxSpeed = 2f;
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

        for (int i = 0; i < spheres.Count; i++)
        {
            spheres[i].moveT += Time.deltaTime * spheres[i].moveSpeed;
            spheres[i].sphereObject.transform.position = CubicBezier(spheres[i].p0, spheres[i].p1, spheres[i].p2, spheres[i].p3, spheres[i].moveT) ;

            if (spheres[i].moveT >= 1f)
            { 
                Destroy(spheres[i].trail.material);
                Destroy(spheres[i].sphereObject);
                spheres.RemoveAt(i);
                i--;
            }
        }
    }
    private Sphere SetSphere()
    {
        Sphere sphere = new Sphere();
        sphere.sphereObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.sphereObject.transform.localScale = Vector3.one * 0.3f;
        
        sphere.trail = sphere.sphereObject.AddComponent<TrailRenderer>();
        sphere.trail.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
        sphere.trail.time = 0.5f;
        sphere.trail.startWidth = 0.2f;
        sphere.trail.endWidth = 0f;

        Color randomColor = Random.ColorHSV();
        sphere.trail.startColor = randomColor;
        sphere.trail.endColor = new Color(randomColor.r, randomColor.g, randomColor.b, 0f);

        sphere.p0 = startPos;
        sphere.p3 = endPos;
        var (randomP1, randomP2) = SetRandomP1P2();
        sphere.p1 = randomP1;
        sphere.p2 = randomP2;

        sphere.moveSpeed = Random.Range(minSpeed, maxSpeed);
        sphere.moveT = 0f;

        return sphere;
    }

    private (Vector3, Vector3) SetRandomP1P2()
    {
        var randomP1 = new Vector3(Random.Range(-10f, 10f), Random.Range(0f, 10f), 0f);
        var randomP2 = new Vector3(Random.Range(-10f, 10f), Random.Range(0f, 10f), 0f);

        return (randomP1, randomP2);
    }

    private Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(d, e, t);
    }

}