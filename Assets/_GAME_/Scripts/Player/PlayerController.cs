using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    //Enums
    private enum Directions {UP, DOWN, LEFT, RIGHT};

    // Editor Data
    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 50f;
    [Header("Dependencies")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    public VectorValue startingPosition;
    
    // Internal Data
    private Vector2 _moveDir = Vector2.zero;
    private Directions _facing = Directions.RIGHT;
    private readonly int _animMoveRight = Animator.StringToHash("AnimPlayerMove");
    private readonly int _animIdle = Animator.StringToHash("AnimPlayerIdle");

    void Start(){
        transform.position = startingPosition.initialValue;
    }
    
    // Tick
    private void Update(){
        GatherInput();
        CalculateFacingDirection();
        UpdateAnimation();
    }

    private void FixedUpdate(){
        MovementUpdate();
    }

    // Input Logic
    private void GatherInput(){
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");
    }

    // Movement Logic
    private void MovementUpdate(){
        _rb.velocity = _moveDir.normalized * _moveSpeed * Time.fixedDeltaTime;
    }

    // Animation Logic
    private void CalculateFacingDirection(){
        if(_moveDir.x != 0){
            if(_moveDir.x > 0){ //moving right
                _facing = Directions.RIGHT;
            }
            else if (_moveDir.x < 0){
                _facing = Directions.LEFT;
            }
        }
    }

    private void UpdateAnimation(){
        if(_facing == Directions.LEFT){
            _spriteRenderer.flipX = true;
        }
        else if(_facing == Directions.RIGHT){
            _spriteRenderer.flipX = false;
        }

        if(_moveDir.SqrMagnitude() > 0){ //moving
            _animator.CrossFade(_animMoveRight, 0);
        } else {
            _animator.CrossFade(_animIdle, 0);
        }
    }


}
