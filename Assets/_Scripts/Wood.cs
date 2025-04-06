using Sirenix.OdinInspector;
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
            SoundPool.Singleton.PlayRandomSound("Hit Stone");
        }
    }

    [Button]
    public override void OnInteract(Transform playerTransform)
    {
        if (joint != null || enabled == false) return;

        base.OnInteract(playerTransform);

        joint = gameObject.AddComponent<ConfigurableJoint>();
        collector = playerTransform.GetComponent<WoodCollector>();

        Rigidbody connectTo;
        if (collector.GetLastWood() == null)
        {
            connectTo = collector.playerRb;
            joint.connectedAnchor = new Vector3(0, -0.55f, 1);
        }
        else
        {
            connectTo = collector.GetLastWood().rb;

            joint.connectedAnchor = new Vector3(0, 0, -1.7f);

            transform.position = connectTo.transform.position;
            transform.rotation = connectTo.transform.rotation;

            connectTo.linearVelocity = Vector3.zero;
            connectTo.angularVelocity = Vector3.zero;
        }

        joint.anchor = new Vector3(0, 0, 1.1f);
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedBody = connectTo;
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.enableCollision = true;

        collector.AddWood(this);
    }
}
