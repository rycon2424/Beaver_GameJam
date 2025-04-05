using Sirenix.OdinInspector;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractable
{
    [Title("Reference")]
    [SerializeField] Rigidbody rb;
    [SerializeField] FixedJoint[] joints;
    [SerializeField] GameObject leafs;
    [Title("Setting")]
    [SerializeField] int health = 1;
    [SerializeField] float torqueForce = 800;
    [SerializeField] float force = 200;
    [Title("Data")]
    [SerializeField, ReadOnly] Vector3 dir;
    Collider trigger;

    void Awake()
    {
        trigger = GetComponent<Collider>();
    }

    [Button]
    public void OnInteract(Transform playerTransform)
    {
        health--;
        if (health == 0)
        {
            dir = transform.position - playerTransform.position;

            rb.isKinematic = false;
            Vector3 pushPos = transform.position;
            pushPos.y += 10;
            rb.AddForceAtPosition(dir.normalized * torqueForce, pushPos);
            trigger.enabled = false;
            StartCoroutine(BreakJoints(1.75f));
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
