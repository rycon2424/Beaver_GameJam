using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public enum PlayerAnimationStates
{
    Idle,
    Running,
    Smack,
    Chomp
}

public class PlayerAnimation : SerializedMonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void SetRunning(bool running)
    {
        anim.SetBool("Running", running);
    }

    public void PlayAnimation(PlayerAnimationStates animationToPlay)
    {
        anim.Play(animationToPlay.ToString(), 0, 0);
    }
}
