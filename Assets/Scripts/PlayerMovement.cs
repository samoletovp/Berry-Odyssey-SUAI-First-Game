using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb; // ������� ��������� ���������� rb ������ Rigidbody2d ��� ��������� �������
    private BoxCollider2D coll;
    private SpriteRenderer sprite; // ������� ��������� ���������� sprite ������ SpriteRenderer ��� ����� ��������� � ������ ������� ��� ����
    private Animator anim; // ���������� ��� ����� ��������

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask oneWayPlatformLayer;

    private float dirX = 0f; // ���������� ��� ���� �� �����������
    [SerializeField] private float moveSpeed = 7f; // ���������� �������� ����, � ������ ����� ����� ��������� � �����
    [SerializeField] private float jumpForce = 14f;// ���������� ������ ������, � ������ ����� ����� ��������� � �����

    private enum MovementState { idle, running, jumping, falling }

    [SerializeField] private AudioSource jumpSoundEffect;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // �������� RigidBody2D � ���������� � ���������� rb
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal"); // ��������� �������� ����� �� �����������
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y); // ������ ��������� ��������� � ����������� �� ������� ������

        if (Input.GetButtonDown("Jump") && IsGrounded()) // ������� ��� ������, ���� ������, �� �������� ������ �� �������� jumpForce
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        UpdateAnimationState(); // �������� ������� ����� ��������
        
    }

    private void UpdateAnimationState() // ������� ����� ��������
    {
        MovementState state;
        
        if (dirX > 0f) // ���� ������ �� ������ �������� �� ���, � ����� ����� ����������� false
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f) // ���� �����, ������ �������� �� ���, � ����� ����� �������� true, ����� �������� �����������
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else // ���� 0 - ������ �������� ���� �� �������
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        // ���������, ��������� �� ����� �� ����� ��� �� ��������� � ����� "OneWayPlatform"
        RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
        if (hit.collider != null)
        {
            return true;
        }

        // ���������, ��������� �� ����� �� ������������� ���������
        hit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, oneWayPlatformLayer);
        return hit.collider != null;
    }
}