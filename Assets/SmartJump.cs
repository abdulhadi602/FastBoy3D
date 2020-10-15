using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartJump : MonoBehaviour
{
 
    void OnTriggerStay()
    {
        Movement.isGrounded = true;
    }
    void OnTriggerExit()
    {
        Movement.isGrounded = false;
    }
    public void Jump()
    {
        if (Movement.isGrounded)
        {
            Movement.rb.AddForce(Movement.jump * Movement.jumpForce, ForceMode.Impulse);
            Movement.isGrounded = false;
        }
    }

}
