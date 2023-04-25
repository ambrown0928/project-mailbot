using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Manages the player on death.
 * !TODO: Implement the DeathState
 */
public class DeathState : BaseState
{


    public override void Enter () { }
    public override void Exit () { }

    public override void Action(Rigidbody body, GameObject corpse)
    {
        GameObject clone = GameObject.Instantiate(corpse, body.position, body.rotation);
        clone.transform.parent = null;
    }
}
