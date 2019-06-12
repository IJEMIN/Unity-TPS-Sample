using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Cursor = UnityEngine.Cursor;

// 플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
// 감지된 입력값을 다른 컴포넌트들이 사용할 수 있도록 제공
public class PlayerInput : MonoBehaviour
{

    public string moveVerticalAxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string moveHorizontalAxisName = "Horizontal"; // 좌우 회전을 위한 입력축 이름

    public string rotateHorizontalAxisName = "Mouse X";
    public string rotateVerticalAxisName = "Mouse Y";

    public string fireButtonName = "Fire1"; // 발사를 위한 입력 버튼 이름
    public string aimDownSightName = "Fire2"; // 발사를 위한 입력 버튼 이름

    public string reloadButtonName = "Reload"; // 재장전을 위한 입력 버튼 이름
    public string jumpButtonName = "Jump";

    // 값 할당은 내부에서만 가능


    public Vector2 moveInput{get; private set;}
    public bool sprint { get; private set; }
    public float mouseX { get; private set; }
    public float mouseY { get; private set; }

    public bool fire { get; private set; } // 감지된 발사 입력값
    
    public bool aimDownSight { get; private set; } // 감지된 발사 입력값
    
    public bool reload { get; private set; } // 감지된 재장전 입력값

    public bool jump {get; private set;}


    private void OnEnable()
    {
        ToggleCursorVisible(false);
    }

    // 매프레임 사용자 입력을 감지
    private void Update()
    {
        // 게임오버 상태에서는 사용자 입력을 감지하지 않는다
        if (GameManager.instance != null
            && GameManager.instance.isGameover)
        {

            mouseX = 0f;
            mouseY = 0f;

            fire = false;
            reload = false;
            jump = false;
            return;
        }

        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (moveInput.sqrMagnitude > 1)
        {
            moveInput = moveInput.normalized;
        }
        
        jump = Input.GetButtonDown(jumpButtonName);

        sprint = Input.GetKey(KeyCode.LeftShift);

        // fire에 관한 입력 감지
        fire = Input.GetButton(fireButtonName);
        aimDownSight = Input.GetMouseButton(1);
        
        // reload에 관한 입력 감지
        reload = Input.GetButtonDown(reloadButtonName);
    }

    public void ToggleCursorVisible(bool active)
    {
        Cursor.visible = active;
    }
}