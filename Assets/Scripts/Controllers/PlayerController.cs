using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerItemDetectorBehaviour PlayerItemDetectorBehaviour => _playerItemDetectorBehaviour;
    public PlayerAnimationBehaviour PlayerAnimationBehaviour => _playerAnimationBehaviour;

    [SerializeField] private PlayerAnimationBehaviour _playerAnimationBehaviour;
    [SerializeField] private PlayerMovementBehaviour _playerMovementBehaviour;
    [SerializeField] private PlayerItemDetectorBehaviour _playerItemDetectorBehaviour;

    private void Start()
    {
        Initialzie();
    }

    public void Initialzie()
    {
        //_playerAnimationBehaviour.Initialize(this);
        _playerMovementBehaviour.Initialize(this);
        //_playerItemDetectorBehaviour.Initialize(this);

    }
}
