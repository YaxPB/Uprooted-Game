using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacWall : MonoBehaviour
{
    public SacrificeAbility sacking;
    public Animator anim;
    public Collider2D box;

    private void Update()
    {
        if(sacking.sacrificed)
        {
            anim.SetBool("isBroken", true);
            box.enabled = false;
        }
    }
}
