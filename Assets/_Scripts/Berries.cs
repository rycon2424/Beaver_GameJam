using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Berries : ResourceAble, IInteractable
{
    [FormerlySerializedAs("barries"),SerializeField]
    private GameObject[] berries;
    private bool noBerries;

    private void Start()
    {
        StartCoroutine(RespawnBerries());
    }

    private IEnumerator RespawnBerries()
    {
        yield return new WaitForSeconds(Random.Range(3, 15));
        while (true)
        {
            yield return new WaitForSeconds(GameManager.Singleton.berriesTimer);
            for (int i = 0; i < berries.Length; i++)
            {
                if (berries[i].activeSelf == false)
                {
                    berries[i].SetActive(true);
                    health++;
                    yielded = false;
                    noBerries = false;
                    break;
                }
            }
        }
    }

    public override void OnInteract(Transform playerTransform)
    {
        base.OnInteract(playerTransform);
        
        FindFirstObjectByType<PlayerAnimation>().PlayAnimation(PlayerAnimationStates.Smack);

        for (int i = 0; i < berries.Length; i++)
        {
            berries[i].SetActive(i < health);
        }

        if (noBerries == false)
            UIManager.Singleton.UpdateItem(resourceType, 1);

        if (yielded)
        {
            noBerries = true;
        }
    }
}