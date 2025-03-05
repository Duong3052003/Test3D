using UnityEngine;

public class CompositeCollider3D : MonoBehaviour
{
    private void Start()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.gameObject.layer = gameObject.layer;
        }
    }
}