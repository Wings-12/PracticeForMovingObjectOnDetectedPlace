using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class JumpWithCharacterController : MonoBehaviour
{
    private CharacterController characterController;//  CharacterControllerを使うための変数
    private Vector3 moveDirection;//  CharacterControllerを動かすための変数
    public float JumpPower;//  ジャンプ力

    void Start()
    {
        characterController = GetComponent<CharacterController>();//  characterControllerにCharacterControllerの値を代入する
    }

    void Update()
    {
        if (characterController.isGrounded)//  もし地面についていたら、
        {
            if (Input.GetKeyDown(KeyCode.Space))//  もし、スペースキーがおされたら、
            {
                moveDirection.y = JumpPower;//  y座標をジャンプ力の分だけ動かす
            }
        }

        moveDirection.y += Physics.gravity.y * Time.deltaTime; //常にy座標を重力の分だけ動かす(重力処理)
        characterController.Move(moveDirection * Time.deltaTime); //CharacterControlloerをmoveDirectionの方向に動かす
    }
}