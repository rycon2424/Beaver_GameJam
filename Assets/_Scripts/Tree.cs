using Sirenix.OdinInspector;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractable
{
    [SerializeField] Rigidbody rb;
    [SerializeField] FixedJoint[]joints;
    [SerializeField] Vector3 force;
    [SerializeField] int health = 1;
    [SerializeField] GameObject leafs;

    [Button]
    public void OnInteract()
    {
        health--;
        if (health <= 0)
        {
            leafs.SetActive(false);

            rb.isKinematic = false;
            rb.AddTorque(force);

            StartCoroutine(BreakJoints(2));
        }
    }

    IEnumerator BreakJoints(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var joint in joints)
        {
            Destroy(joint);
        }
    }
}
