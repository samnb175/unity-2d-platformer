using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private bool canMelee;
    private enum MeleeState {punch1};
    private MeleeState meleeState;
    private Animator anim;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        MeleeAttack();
        //UpdateAnimation();

        
    }

    private void MeleeAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            canMelee = true;

        }

        if (canMelee)
        {
            meleeState = MeleeState.punch1;
            anim.SetInteger("Melee", (int)meleeState);
            anim.SetBool("canAttack", canMelee);

        }

    }


    private void FinishMeleeAttack()
    {
        canMelee = false;
        anim.SetBool("canAttack", canMelee);

    }

}
