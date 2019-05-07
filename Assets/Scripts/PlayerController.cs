using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    // Inherits the custom physics functionality provided by PhysicsObject
    // Gives us the ability to input the player's horizontal velocity, do cancellable jumps and switch animation states

    public float maxSpeed = 7; // maximum speed of the player
    public float jumpTakeOffSpeed = 16; // jump power of the player
    private SpriteRenderer spriteRenderer; // the sprite of the parent object
    private Animator animator; // the Animator of the parent object

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // store reference to the player's sprite
        animator = GetComponent<Animator>(); // store reference to the player's Animator
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero; // clear previous movement data
        // accept input from either the left joystick or the dpad
        if (Input.GetAxis("DPadXAxis") != 0)
        {
            move.x = (Input.GetAxis("DPadXAxis")); // needs to have an entry in the input manager for Joystick axis 6 (xbox360 controller D pad)
        }
        else if (Input.GetAxis("Horizontal") != 0)
        {
            move.x = (Input.GetAxis("Horizontal"));
        }
        if (Input.GetButtonDown("Jump") && grounded) // only permit jumps if we're stood on something - could be extended to allow double jumps etc.
        {
            velocity.y = jumpTakeOffSpeed; // put the configured 'jump power' into the 'velocity' Vector2 variable.
        }
        else if (Input.GetButtonUp("Jump")) // if the jump button was released mid jump (aka jump cancelled)...
        {
            if (velocity.y > 0) // ...and if the player is still moving vertically... (i.e. they've not hit the ground yet)
            {
                velocity.y = velocity.y * 0.5f; // ... then slow the player's vertical movement by half.
            }
        }
        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX; // make the sprite face in the direction it is moving.
        }
        // send some data to the Animator so that it can change animation according to the player's current movement
        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        animator.SetFloat("velocityY", Mathf.Abs(velocity.y) / maxSpeed);
        // set the player's target direction + speed (i.e. vector2) based on the above contraints, input, and calculations.
        // 'targetVelocity' will be used by the parent class 'PhysicsObject' to move the player's transform directly, bypassing Unity's real world physics.
        targetVelocity = move * maxSpeed;
    }
}