using System.Collections.Generic;
using UnityEngine;

public class SimplePlacer : MonoBehaviour
{
    [SerializeField] GameObject fan;
    [SerializeField] List<Material> materials;

    [SerializeField] int numberOfFans = 1000;
    [SerializeField] float range = 20f;


    void Awake()
    {
        GenerateFans();
    }

    void GenerateFans()
    {
        for(int i = 0; i < numberOfFans; i++)
        {
            Vector3 position = new Vector3(Random.Range(-range, range), 5f, Random.Range(-range, range));
            RaycastHit hit;
            Physics.Raycast(position, Vector3.down, out hit, 5);
            position.y = hit.point.y;
            GameObject fanInstance = Instantiate(fan, position, transform.rotation);
            fanInstance.transform.parent = transform;
            fanInstance.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, materials.Count)];
        }
    }

}
