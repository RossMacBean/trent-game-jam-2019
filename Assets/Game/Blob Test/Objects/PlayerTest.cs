using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public Vector3[] VisiblePoints;

    public Material BasicMaterial;
    public Material HitMaterial;

    private Renderer _renderer;

    public void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < VisiblePoints.Length; i++)
        {
            Gizmos.DrawWireSphere(transform.position + VisiblePoints[i], 0.2f);
        }
        
    }    

    public void VisibleByBlobStart(Blob blob)
    {
        Debug.Log("VisibleByBlob - Start");
        _renderer.material = HitMaterial;
    }

    public void VisibleByBlobStop(Blob blob)
    {
        Debug.Log("VisibleByBlob - Stop");
        _renderer.material = BasicMaterial;
    }

}
