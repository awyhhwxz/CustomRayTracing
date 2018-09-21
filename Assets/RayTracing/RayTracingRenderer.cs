using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTracingRenderer : MonoBehaviour {

    public Texture2D Tex;
    public Camera Cam;

    public const int kWidth = 1024;
    public const int kHeight = 728;

	// Use this for initialization
	void Start () {
        Tex = new Texture2D(kWidth, kHeight);
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

                var color = (camRay.direction + Vector3.one) * 0.5f;

                Color col = new Color(color.x, color.y, color.z);
                RaycastHit hitInfo;
                if (Physics.Raycast(camRay, out hitInfo))
                {
                    var normal = (hitInfo.normal + Vector3.one) * 0.5f;
                    var material = hitInfo.collider.GetComponent<RayTracingMaterialBase>();
                    col = material.Albedo;
                }
                pixels[kWidth * hi + wi] = col;
            }
        }

        Tex.SetPixels(pixels);
        Tex.Apply();
    }
}
