using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenPlaneGenerator {

    private Mesh _mesh;
	
    public Mesh GetPlane()
    {
        if (_mesh == null)
        {
            _mesh = new Mesh();
            _mesh.SetVertices(new List<Vector3>()
            {
                new Vector3(-1, -1, 0),
                new Vector3(-1, 1, 0),
                new Vector3(1, 1, 0),
                new Vector3(1, -1, 0),
            });

            _mesh.SetUVs(0, new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
            });

            _mesh.SetIndices(new int[] { 0, 1, 2, 3 }, MeshTopology.Quads, 0);
        }

        return _mesh;
    }
}
