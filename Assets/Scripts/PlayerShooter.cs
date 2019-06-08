using UnityEngine;

// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : MonoBehaviour {
    
    
    public enum AimState { Idle, HipFire}

    private Camera playerCamera;
    
    public AimState aimState { get; private set; }

    private float releaseGunAfter = 3f;
    
    public Gun gun; // 사용할 총
    public Transform gunPivot; // 총 배치의 기준점
    public Transform leftHandMount; // 총의 왼쪽 손잡이, 왼손이 위치할 지점
    public Transform rightHandMount; // 총의 오른쪽 손잡이, 오른손이 위치할 지점

    private PlayerMovement _playerMovement;

    private PlayerInput playerInput; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트

    private Vector3 smoothedVelocity;

    private bool linedUp
    {
        get
        {
            if (Mathf.Abs(playerCamera.transform.eulerAngles.y - transform.eulerAngles.y) > 0.1f)
            {
                return false;
            }

            return true;
        }
    }
    
    private void Start()
    {
        playerCamera = Camera.main;
        // 사용할 컴포넌트들을 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable() {
        
        // 슈터가 활성화될 때 총도 함께 활성화
        gun.gameObject.SetActive(true);
    }

    private void OnDisable() {
        // 슈터가 비활성화될 때 총도 함께 비활성화
        gun.gameObject.SetActive(false);
    }

    private void Update() {
        if (playerInput.reload)
        {
            // 재장전 입력 감지시 재장전
            if (gun.Reload())
            {
                // 재장전 성공시에만 재장전 애니메이션 재생
                playerAnimator.SetTrigger("Reload");
            }
        }


        if (aimState == AimState.Idle)
        {
            if (playerInput.fire)
            {
                if (!linedUp)
                {
                    _playerMovement.Rotate();    
                }
                else
                {
                    aimState = AimState.HipFire;
                }
            }
        }
        else if (aimState == AimState.HipFire)
        {
            _playerMovement.Rotate();

            if (playerInput.fire)
            {
                gun.Fire(playerCamera.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0f)),playerCamera.transform.forward);
            }
            else
            {
                aimState = AimState.Idle;
            }
            
                   

        }
        
        gunPivot.localEulerAngles = Vector3.right * playerCamera.transform.localEulerAngles.x;

        // 남은 탄약 UI를 갱신
        UpdateUI();
    }

    // 탄약 UI 갱신
    private void UpdateUI() {
        if (gun != null && UIManager.instance != null)
        {
            // UI 매니저의 탄약 텍스트에 탄창의 탄약과 남은 전체 탄약을 표시
            UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        }
    }

    private Vector3 previousRightElbowPosition;

    // 애니메이터의 IK 갱신
    private void OnAnimatorIK(int layerIndex) {
        // 총의 기준점 gunPivot을 3D 모델의 오른쪽 팔꿈치 위치로 이동

        
 

            // IK를 사용하여 왼손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

            playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand,
                leftHandMount.position);
            playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand,
                leftHandMount.rotation);

            // IK를 사용하여 오른손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

            playerAnimator.SetIKPosition(AvatarIKGoal.RightHand,
                rightHandMount.position);
            playerAnimator.SetIKRotation(AvatarIKGoal.RightHand,
                rightHandMount.rotation);
                
            playerAnimator.SetLookAtWeight(1.0f);
            playerAnimator.SetLookAtPosition(playerCamera.ViewportToWorldPoint(new Vector3(0.5f,0.5f,100.0f)));

            if (previousRightElbowPosition != Vector3.zero)
            {
                var offset = playerAnimator.bodyPosition - previousRightElbowPosition;

                gunPivot.Translate(0, offset.y, 0);
            }

            previousRightElbowPosition = playerAnimator.bodyPosition;



    }
}