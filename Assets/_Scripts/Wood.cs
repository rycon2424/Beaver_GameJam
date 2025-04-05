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
        if (collision.gameObject.tag == "Obstacle")
        {
            collector.RemoveWood(this);
        }
    }

    public override void OnInteract(Transform playerTransform)
    {
        base.OnInteract(playerTransform);

        if (joint == null)
            joint = gameObject.AddComponent<ConfigurableJoint>();

        collector = playerTransform.GetComponent<WoodCollector>();


        Rigidbody connectTo;
        if (collector.GetLastWood() == null)
        {
            connectTo = collector.playerRb;
            joint.connectedAnchor = new Vector3(0, -0.7f, 1);
        }
        else
        {
            connectTo = collector.GetLastWood().rb;
            joint.connectedAnchor = new Vector3(0, -0.6f, -0.5f);
            //joint.anchor = new Vector3(0, 0, 1.7f);
        }
        collector.AddWood(this);

        joint.anchor = new Vector3(0, 0, 1.1f);
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedBody = connectTo;
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.enableCollision = true;
    }
}
