using System;
using System.Collections;
using UnityEngine;

public class KidsManager : MonoBehaviour
{
    public Animator[] kidsAnim;
    public GameObject babyCam;
    public GameObject eatingBeries;

    private Coroutine stateHandlers;

    public static KidsManager Singleton;

    private void Awake()
    {
        if (Singleton != null)
            Destroy(Singleton.gameObject);
        Singleton = this;
    }

    private void Start()
    {
        babyCam.SetActive(false);
        stateHandlers = StartCoroutine(States());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            if (UIManager.Singleton.itemVisuals.ContainsKey(ItemTypes.Berries))
            {
                int currentBerries = UIManager.Singleton.itemVisuals[ItemTypes.Berries].count;

                GameManager.Singleton.babiesCurrentFood += currentBerries;

                UIManager.Singleton.UpdateItem(ItemTypes.Berries, -currentBerries);

                PlayAnimation("EATING");
                eatingBeries.SetActive(true);
                StartCoroutine(Eating());

            }
        }
    }

    private IEnumerator Eating()
    {
        yield return new WaitForSeconds(2);
        eatingBeries.SetActive(false);
    }

    public void PlayAnimation(string animationName, bool continueStates = true)
    {
        StopCoroutine(stateHandlers);

        foreach (Animator anim in kidsAnim)
        {
            anim.Play(animationName);
        }

        if (continueStates)
        {
            stateHandlers = StartCoroutine(States());
        }
    }

    private IEnumerator States()
    {
        yield return new WaitForSeconds(2);
        while (GameManager.Singleton.gameOver == false)
        {
            int foodpercentage = Mathf.CeilToInt(GameManager.Singleton.babiesCurrentFood / GameManager.Singleton.babiesMaxFood * 100);
            babyCam.SetActive(foodpercentage < 50);
            if (foodpercentage >= 50)
            {
                foreach (Animator anim in kidsAnim)
                {
                    anim.Play("CALM");
                }
            }
            else if (foodpercentage > 25)
            {
                foreach (Animator anim in kidsAnim)
                {
                    anim.Play("SCREAM SOFT");
                }
            }
            else
            {
                foreach (Animator anim in kidsAnim)
                {
                    anim.Play("SCREAM HARD");
                }
            }

            yield return new WaitForSeconds(1);
        }
    }
}
