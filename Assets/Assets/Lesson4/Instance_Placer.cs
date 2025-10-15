using System.Collections.Generic;
using UnityEngine;

public class Instance_Placer : MonoBehaviour
{
    [SerializeField] Mesh mesh;
    [SerializeField] List<Material> mats;
    private List<Matrix4x4[]> matrices;
    [SerializeField] int numberOfFans = 1000;
    [SerializeField] float range = 20f;
    void Awake()
    {
        matrices = new List<Matrix4x4[]>();
        for (int i = 0; i < mats.Count; i++)
        {
            matrices.Add(new Matrix4x4[numberOfFans / mats.Count]);

            Vector4[] colors = new Vector4[1];

            for (int j = 0; j < numberOfFans / mats.Count; j++)
            {
                // Random position and rotation
                Vector3 position = new Vector3(Random.Range(-range, range), 5f, Random.Range(-range, range));
                RaycastHit hit;
                Physics.Raycast(position, Vector3.down, out hit, 5);
                position.y = hit.point.y - 0.45f;
                matrices[i][j] = Matrix4x4.TRS(
                    position,
                    Quaternion.Euler(90, 0, 0),
                    Vector3.one * 2
                );
            }

        }
    }

    void Update()
    {
        for (int i = 0; i < mats.Count; i++)
        {
            Graphics.DrawMeshInstanced(
                mesh,
                0,
                mats[i],
                matrices[i]                
            );
        }
    }
}
