using Sirenix.OdinInspector;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Wood : ResourceAble, IInteractable
{
    public ConfigurableJoint joint;
    public Rigidbody rb;
    WoodCollector collector;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collector != null && collision.gameObject.tag == "Obstacle")
        {
            collector.RemoveWood(this);
            SoundPool.Singleton.PlayRandomSound("Log 1", "Log 2");
        }
    }

    [Button]
    public override void OnInteract(Transform playerTransform)
    {
        if (joint != null || enabled == false) return;

        base.OnInteract(playerTransform);

        Rigidbody connectTo;
        collector = playerTransform.GetComponent<WoodCollector>();

        if (collector.GetLastWood() == null)
        {
            connectTo = collector.playerRb;
            joint = gameObject.AddComponent<ConfigurableJoint>();

            StartCoroutine(AttachJointAfterOneFrame(connectTo, new Vector3(0, -0.55f, 1)));
        }
        else
        {
            connectTo = collector.GetLastWood().rb;

            Vector3 offset = connectTo.transform.forward * -2.6f;
            transform.position = connectTo.transform.position + offset;
            transform.rotation = connectTo.transform.rotation;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            StartCoroutine(AttachJointAfterOneFrame(connectTo, new Vector3(0, 0, -1.7f)));
        }

        SoundPool.Singleton.PlayRandomSound("Log 1", "Log 2");
        collector.AddWood(this);
    }

    IEnumerator AttachJointAfterOneFrame(Rigidbody connectTo, Vector3 connectedAnchor)
    {
        rb.isKinematic = true;
        yield return new WaitForFixedUpdate();
        rb.isKinematic = false;

        joint = gameObject.AddComponent<ConfigurableJoint>();

        joint.connectedBody = connectTo;
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = new Vector3(0, 0, 1.1f);
        joint.connectedAnchor = connectedAnchor;

        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.enableCollision = true;

        joint.projectionMode = JointProjectionMode.PositionAndRotation;
        joint.projectionDistance = 0.1f;
        joint.projectionAngle = 1f;
    }
}