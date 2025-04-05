using Sirenix.OdinInspector;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractable
{
    [SerializeField] Rigidbody rb;
    [SerializeField] FixedJoint[] joints;
    [SerializeField] Transform target;
    [SerializeField, ReadOnly] Vector3 dir;
    [SerializeField] float torqueForce = 800;
    [SerializeField] float force = 200;
    [SerializeField] int health = 1;
    [SerializeField] GameObject leafs;

    [Button]
    public void OnInteract()
    {
        health--;
        if (health == 0)
        {
            dir = transform.position - target.position;

            rb.isKinematic = false;
            rb.AddTorque(dir.normalized * torqueForce);

            StartCoroutine(BreakJoints(1.5f));
        }
    }

    IEnumerator BreakJoints(float delay)
    {
        yield return new WaitForSeconds(delay);

        leafs.SetActive(false);

        for (int i = joints.Length - 1; i >= 0; i--)
        {
            joints[i].GetComponent<Rigidbody>().AddForce(dir.normalized * force);
            Destroy(joints[i]);
            joints[i] = null;
        }
    }
}
