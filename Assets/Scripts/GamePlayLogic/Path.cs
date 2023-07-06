using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Vector3> points = new List<Vector3>();
    public List<Transform> transforms = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        if (transforms != null)
        {
            points = new List<Vector3>();
            for (int i = 0; i < transforms.Count; i++)
            {
                points.Add(transforms[i].position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public Color lineColor = Color.green;
    public float pointSize = 0.1f;

    private void OnDrawGizmos()
    {
        if (transforms == null || transforms.Count == 0)
            return;

        Gizmos.color = lineColor;
        for (int i = 0; i < transforms.Count - 1; i++)
        {
            Gizmos.DrawLine(transforms[i].position, transforms[i + 1].position);
        }

    }

}
