using Unity.VisualScripting;
using UnityEngine;

public class PlayerWalkSound : MonoBehaviour
{
    public PlayerController controller;

    public float stepTimer, stepInterval = 0.3f;

    private void Update()
    {
        WalkSound();
    }
    private void WalkSound()
    {
        if (controller.moving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                SoundPool.Singleton.PlayRandomSound("Walk Fast 1", "Walk Fast 2", "Walk Fast 3", "Walk Fast 4", "Walk Fast 5", "Walk Fast 6");
                stepTimer = stepInterval;
            }
        }

        else
        {
            stepTimer = 0f; // Reset so footsteps play instantly when moving starts again
        }
    }

}
