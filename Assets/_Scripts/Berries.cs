using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Berries : ResourceAble, IInteractable
{
    [FormerlySerializedAs("barries"),SerializeField]
    private GameObject[] berries;

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
                    break;
                }
            }
        }
    }

    public override void OnInteract(Transform playerTransform)
    {
        base.OnInteract(playerTransform);

        foreach (var b in berries)
            b.SetActive(false);

        for (int i = 0; i < health; i++)
        {
            berries[i].gameObject.SetActive(true);
        }

        UIManager.Singleton.UpdateItem(resourceType, 1);
    }
}
