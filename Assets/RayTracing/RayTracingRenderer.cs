using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTracingRenderer : MonoBehaviour {

    public Texture2D Tex;
    public Camera Cam;

    public const int kWidth = 1024;
    public const int kHeight = 728;

    public int MaxBounceCount = 5;

	// Use this for initialization
	void Start () {
        Tex = new Texture2D(kWidth, kHeight);
        Render();
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.D))
        {
            Render();
        }
    }

    public void Render()
    {
        var camera = Cam;
        var camOrigin = camera.transform.position;
        var corner00 = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.farClipPlane));
        var corner10 = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.farClipPlane));
        var corner01 = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.farClipPlane));
        var camRay = new Ray();
        camRay.origin = camOrigin;

        var pixels = Tex.GetPixels();

        for (int hi = 0; hi < kHeight; ++hi)
        {
            for (int wi = 0; wi < kWidth; ++wi)
            {
                var rayEndPoint = corner00 + (corner10 - corner00) * ((float)wi / kWidth) + (corner01 - corner00) * ((float)hi / kHeight);
                camRay.direction = Vector3.Normalize(rayEndPoint - camOrigin);

                var col = ComputeColor(camRay);
                pixels[kWidth * hi + wi] = col;
            }
        }

        Tex.SetPixels(pixels);
        Tex.Apply();
    }

    private Vector3 RandomVector()
    {
        Vector3 vec;
        do
        {
            vec = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        }
        while (vec.magnitude >= 1.0f);
        return vec;
    }

    private Color ComputeColor(Ray ray, int traverseDepth = 0)
    {
        Color col;
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            var normal = (hitInfo.normal + Vector3.one);
            var material = hitInfo.collider.GetComponent<RayTracingMaterialBase>();
            var diffuseRay = new Ray(hitInfo.point, hitInfo.point + hitInfo.normal + 0.5f * RandomVector());
            var addColor = traverseDepth > MaxBounceCount ? Color.black : ComputeColor(diffuseRay, ++traverseDepth); 
            col = 0.5f * (material.Albedo + addColor);
        }
        else
        {
            var unitDirection = ray.direction.normalized;
            float t = 0.5f * (unitDirection.y + 1.0f);
            return Color.Lerp(Color.white, new Color(0.5f, 0.7f, 1.0f), t);
        }

        return col;
    }
}
