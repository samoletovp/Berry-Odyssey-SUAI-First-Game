using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb; // создаем приватную переменную rb класса Rigidbody2d для изменения позиции
    private BoxCollider2D coll;
    private SpriteRenderer sprite; // создаем приватную переменную sprite класса SpriteRenderer для флипа персонажа в другую сторону при беге
    private Animator anim; // переменная для смены анимации

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask oneWayPlatformLayer;

    private float dirX = 0f; // переменная для бега по горизонтали
    [SerializeField] private float moveSpeed = 7f; // переменная скорости бега, в начале чтобы пункт появлялся в юнити
    [SerializeField] private float jumpForce = 14f;// переменная высоты прыжка, в начале чтобы пункт появлялся в юнити

    private enum MovementState { idle, running, jumping, falling }

    [SerializeField] private AudioSource jumpSoundEffect;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // вызываем RigidBody2D и записываем в переменную rb
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal"); // получение значения ввода по горизонтали
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y); // меняем положение персонажа в зависимости от нажатой кнопки

        if (Input.GetButtonDown("Jump") && IsGrounded()) // условие для прыжка, если нажато, то изменяем прыжок на величину jumpForce
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        UpdateAnimationState(); // вызываем функцию смены анимации
        
    }

    private void UpdateAnimationState() // функция смены анимации
    {
        MovementState state;
        
        if (dirX > 0f) // если вправо то меняем анимацию на бег, а также флипу присваиваем false
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f) // если влево, меняем анимацию на бег, а также флипу присваем true, чтобы персонаж развернулся
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else // если 0 - меняем анимацию бега на стояние
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
        // Проверяем, находится ли игрок на земле или на платформе с тегом "OneWayPlatform"
        RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
        if (hit.collider != null)
        {
            return true;
        }

        // Проверяем, находится ли игрок на односторонней платформе
        hit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, oneWayPlatformLayer);
        return hit.collider != null;
    }
}