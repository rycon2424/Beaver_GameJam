using NUnit.Framework.Constraints;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractable
{
    [Title("Reference")]
    [SerializeField] Rigidbody rb;
    [SerializeField] FixedJoint[] joints;
    [SerializeField] Wood[] woods;
    [SerializeField] GameObject leafs;
    [Title("Setting")]
    [SerializeField] int health = 1;
    [SerializeField] float torqueForce = 800;
    [SerializeField] float force = 200;
    [Title("Data")]
    [SerializeField, ReadOnly] Vector3 dir;
    Collider trigger;
    List<Vector3> directions = new List<Vector3>();

    void Awake()
    {
        trigger = GetComponent<Collider>();
        directions.Clear();

        foreach (var wood in woods)
        {
            wood.enabled = false;
        }
    }

    [Button]
    public void OnInteract(Transform playerTransform)
    {
        FindFirstObjectByType<PlayerAnimation>().PlayAnimation(PlayerAnimationStates.Chomp);
        
        health--;
        directions.Add((playerTransform.position - transform.position).normalized);

        if (health == 1)
        {
            StartCoroutine(DelayedDrop(Random.Range(1.5f, 3f)));
        }
        else if (health == 0)
        {
            DropTree();
        }
    }

    IEnumerator DelayedDrop(float delay)
    {
        yield return new WaitForSeconds(delay);

        DropTree();
    }

    void DropTree()
    {
        foreach (var item in directions)
        {
            dir += item;
        }
        dir = dir.normalized;

        rb.isKinematic = false;
        Vector3 pushPos = transform.position;
        pushPos.y += 10;
        rb.AddForceAtPosition(dir * torqueForce, pushPos);
        trigger.enabled = false;

        StopAllCoroutines();
        StartCoroutine(BreakJoints(1.75f));
    }

    IEnumerator BreakJoints(float delay)
    {
        yield return new WaitForSeconds(delay);

        leafs.SetActive(false);

        for (int i = joints.Length - 1; i >= 0; i--)
        {
            joints[i].GetComponent<Rigidbody>().AddForce(dir * force);
            Destroy(joints[i]);
            joints[i] = null;
        }
        foreach (var wood in woods)
        {
            wood.enabled = true;
        }

        yield return new WaitForSeconds(GameManager.Singleton.treeTimer);
        GameManager.Singleton.RespawnTree(transform);
    }
}