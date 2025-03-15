using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] PlayerAnimator _playerAnimator;
    PlayerStats _playerStats;
    [SerializeField] Rigidbody2D _playerRb;
    [SerializeField] PlayerHPManager _playerHPManager;
    public PlayerAnimator PlayerAnimator { get { return _playerAnimator; } }
    public PlayerStats PlayerStats { get { return _playerStats; } set { _playerStats = value; } }
    public Rigidbody2D PlayerRb { get { return _playerRb; } }
    public PlayerHPManager PlayerHPManager { get { return _playerHPManager; } }

    private void Start()
    {
        _playerStats = new PlayerStats(GameManager.gm.selectedCharacter.PlayerStats);
        _playerHPManager.Initialize(this);
    }
}
