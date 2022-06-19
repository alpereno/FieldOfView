using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class Player : MonoBehaviour
{
    // these scripts control the player

    [SerializeField] float speed = 7f;
    Camera mainCamera;
    Rigidbody playerRb;
    Vector3 velocity;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        playerLook();
        movement();
    }

    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + velocity * Time.deltaTime);
    }

    void playerLook()
    {
        // find the position of mouse in world space 
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 correntPoint = new Vector3(mousePos.x, transform.position.y, mousePos.z);
        transform.LookAt(correntPoint);
    }

    void movement()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        velocity = moveInput.normalized * speed;
    }
}
