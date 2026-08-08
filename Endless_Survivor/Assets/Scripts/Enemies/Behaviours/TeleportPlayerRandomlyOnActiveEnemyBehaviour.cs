using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TeleportPlayerRandomlyOnActiveEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => 1;

    [SerializeField] CustomAnimation _onTeleportAnim;
    [SerializeField] float _teleportDelay;
    [SerializeField] SFXInfo _teleportSfx;
    float _teleportTimer;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var teleportPlayerOriginal = original as TeleportPlayerRandomlyOnActiveEnemyBehaviour;
        _onTeleportAnim = new(EnemyControl.Animator, teleportPlayerOriginal._onTeleportAnim);
        _teleportDelay = teleportPlayerOriginal._teleportDelay;
        _teleportSfx = teleportPlayerOriginal._teleportSfx;

        EnemyControl.Animator.AddAnimations(new() { _onTeleportAnim });
        _teleportTimer = _teleportDelay;
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        EnemyControl.Animator.ChangeAnim(_onTeleportAnim.AnimationName);
        if (_teleportTimer > 0)
        {
            _teleportTimer -= Time.deltaTime;
            return;
        }

        var possibleTiles = MapManager.mm.LoadedTiles.Where(tile => !tile.IsWall).ToList();
        Vector2 teleportedPos = possibleTiles[Random.Range(0, possibleTiles.Count)].transform.position;
        PlayerControl.pc.transform.position = teleportedPos;
        _teleportTimer = _teleportDelay;
        KillBehaviour();
    }
}
