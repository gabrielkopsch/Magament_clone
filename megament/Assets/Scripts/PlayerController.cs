using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerCollider))]
public class PlayerController : MonoBehaviour
{
    const float speed = 5;
    const float fallMultiplier = 2.5f;
    const float lowJumpMultiplier = 2f;
    const float jumpForce = 10;
    const float dashForce = 10;

    bool jumping, dashing;
    Vector2 direction;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    SpriteRenderer render;
    Rigidbody2D rb;
    PlayerCollider playerCollider;

    Inputs inputs;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<PlayerCollider>();

        inputs = new Inputs();

        inputs.Player.Jump.performed += ctx => Jump();
        inputs.Player.Jump.canceled += ctx => jumping = false;
        inputs.Player.Move.performed += ctx => direction = ctx.ReadValue<Vector2>();
        inputs.Player.Dash.performed += ctx => StartCoroutine(Dash());
        inputs.Player.Fire.performed += ctx => Shoot();
    }

    private void Update()
    {
        SetGravity();
        Movement();
    }

    private void Movement()
    {
        if(!dashing)
        {
            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
        }

        render.flipX = rb.velocity.x < 0;
    }

    private void SetGravity()
    {
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity * fallMultiplier * Time.deltaTime;
        }else if(rb.velocity.y > 0 && !jumping)
        {
            rb.velocity += Vector2.up * Physics2D.gravity * lowJumpMultiplier * Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (playerCollider.OnGround)
        {
            jumping = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private IEnumerator Dash()
    {
        if (!dashing)
        {
            dashing = true;
            rb.velocity = new Vector2(dashForce * direction.x, 0);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

            yield return new WaitForSeconds(0.5f);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            dashing = false;
        }
    }

    private void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }
}
