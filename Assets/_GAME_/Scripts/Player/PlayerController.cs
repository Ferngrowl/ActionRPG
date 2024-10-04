using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    // Enums
    private enum Directions { UP, DOWN, LEFT, RIGHT };

    // Editor Data
    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 50f;
    [Header("Dependencies")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    public VectorValue startingPosition;

    // Internal Data
    private Vector2 _moveDir;
    private Directions _facing;
    private readonly int _animMoveRight = Animator.StringToHash("AnimPlayerMoveRight");
    private readonly int _animMoveUp = Animator.StringToHash("AnimPlayerMoveUp");
    private readonly int _animMoveDown = Animator.StringToHash("AnimPlayerMoveDown");
    private readonly int _animIdle = Animator.StringToHash("AnimPlayerIdle");

    void Start() => transform.position = startingPosition.initialValue;

    private void Update()
    {
        GatherInput();
        CalculateFacingDirection();
        UpdateAnimation();
    }

    private void FixedUpdate() => _rb.velocity = _moveDir.normalized * _moveSpeed * Time.fixedDeltaTime;

    private void GatherInput()
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");
    }

    private void CalculateFacingDirection()
    {
        if (_moveDir.x != 0)
            _facing = _moveDir.x > 0 ? Directions.RIGHT : Directions.LEFT;
        else if (_moveDir.y != 0)
            _facing = _moveDir.y > 0 ? Directions.UP : Directions.DOWN;
    }

    private void UpdateAnimation()
    {
        _spriteRenderer.flipX = _facing == Directions.LEFT;

        if (_moveDir.SqrMagnitude() > 0) // moving
        {
            switch (_facing)
            {
                case Directions.RIGHT: _animator.CrossFade(_animMoveRight, 0); break;
                case Directions.UP: _animator.CrossFade(_animMoveUp, 0); break;
                case Directions.DOWN: _animator.CrossFade(_animMoveDown, 0); break;
                case Directions.LEFT: _animator.CrossFade(_animMoveRight, 0); break;
            }
        }
        else // idle
        {
            _animator.CrossFade(_animIdle, 0);
        }
    }
}
