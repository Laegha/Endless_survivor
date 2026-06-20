using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root != PlayerControl.pc.transform)
            return;

        GameProgressionManager.gpm.EndRun(PlayerControl.pc.CharacterData.ExitRunAnimation, PlayerControl.pc.CharacterData.ExitRunParticles, GameProgressionManager.RunEndType.Win);
    }
}
