using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationBehaviour : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void Initialize(PlayerController playerController)
    {
       
    }

    public void SetAnimation(bool IsWalking)
    {
        _animator.SetBool("IsWalking", IsWalking);
    }
}
