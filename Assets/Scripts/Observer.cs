using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    private float fspeed = 0f;
    private float sspeed = 0f;
    private float vspeed = 0f;
    private float hspeed = 0f;

    private float moveMultiplier = 0.01f;
    private float strafeMultiplier = 0.01f;
    private float horizontalMultiplier = 0.1f;
    private float verticalMultiplier = 0.1f;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.fspeed += 1.0f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            this.fspeed -= 1.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.sspeed -= 1.0f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.sspeed += 1.0f;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            this.sspeed = 0.0f;
            this.fspeed = 0.0f;
            this.hspeed = 0.0f;
            this.vspeed = 0.0f;

            this.transform.position = Vector3.zero;
            this.transform.rotation = Quaternion.identity;
        }

        if (Input.GetMouseButtonDown(0))
        {
        }
        else if (Input.GetMouseButton(0))
        {
            var v = Input.GetAxis("Mouse Y");
            var h = Input.GetAxis("Mouse X");
            this.vspeed += v;
            this.hspeed -= h;
        }

        if (Mathf.Abs(this.vspeed) >= 0.001f || Mathf.Abs(this.hspeed) >= 0.001f)
        {
            Rotate();
        }

        if (Mathf.Abs(this.fspeed) >= 0.001f || Mathf.Abs(this.sspeed) >= 0.001f)
        {
            Move();
        }
    }

    private void Move()
    {
        this.transform.position += this.transform.forward * (this.fspeed * this.moveMultiplier) + 
            this.transform.right * (this.sspeed * this.strafeMultiplier);
        this.fspeed *= 0.98f;
        this.sspeed *= 0.98f;
    }

    private void Rotate()
    {        
        this.transform.Rotate(this.vspeed * this.verticalMultiplier, this.hspeed * this.horizontalMultiplier, 0);
        this.vspeed *= 0.98f;
        this.hspeed *= 0.98f;
    }
}
