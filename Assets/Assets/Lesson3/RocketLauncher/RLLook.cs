using UnityEngine;

public class RLLook : MonoBehaviour
{
    [SerializeField] private Transform rlhead;
    [SerializeField] private Transform rlbase;
    [SerializeField] private Transform target;


    void Update()
    {
        rlhead.LookAt(target);
        rlbase.rotation = Quaternion.Euler(Vector3.Scale(rlhead.rotation.eulerAngles, Vector3.up));
    }
} 
