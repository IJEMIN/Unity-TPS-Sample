using System.Collections;
using UnityEngine;

// 총을 구현한다
public class Gun : MonoBehaviour
{
    // 총의 상태를 표현하는데 사용할 타입을 선언한다
    public enum State
    {
        Ready, // 발사 준비됨
        Empty, // 탄창이 빔
        Reloading // 재장전 중
    }

    public int ammoRemain = 100; // 남은 전체 탄약

    private LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러
    private float currentSpreadVelocity;

    public float damage = 25; // 공격력

    [HideInInspector] public LayerMask excludeTarget;
    public float fireDistance = 100f; // 사정거리

    public Transform leftHandMount;

    public Transform fireTransform; // 총알이 발사될 위치
    private AudioSource gunAudioPlayer; // 총 소리 재생기


    private PlayerShooter gunHolder;
    private float lastFireTime; // 총을 마지막으로 발사한 시점

    public int magAmmo; // 현재 탄창에 남아있는 탄약
    public int magCapacity = 25; // 탄창 용량

    
    [Range(0f, 10f)] public float maxSpread = 3f;
    private float spread;

    
    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public AudioClip reloadClip; // 재장전 소리
    public float reloadTime = 1.8f; // 재장전 소요 시간
    [Range(0.01f, 3f)] public float restoreFromRecoilSpeed = 2f;
    public ParticleSystem shellEjectEffect; // 탄피 배출 효과
    public AudioClip shotClip; // 발사 소리
    [Range(1f, 10f)] public float stability = 1f;


    public float timeBetFire = 0.12f; // 총알 발사 간격
    public State state { get; private set; } // 현재 총의 상태

    
    private void Awake()
    {
        // 사용할 컴포넌트들의 참조를 가져오기
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        // 사용할 점을 두개로 변경
        bulletLineRenderer.positionCount = 2;
        // 라인 렌더러를 비활성화
        bulletLineRenderer.enabled = false;
    }


    public void Setup(PlayerShooter gunHolder)
    {
        this.gunHolder = gunHolder;
    }

    private void OnEnable()
    {
        spread = 0;
        // 현재 탄창을 가득채우기
        magAmmo = magCapacity;
        // 총의 현재 상태를 총을 쏠 준비가 된 상태로 변경
        state = State.Ready;
        // 마지막으로 총을 쏜 시점을 초기화
        lastFireTime = 0;
    }

    // 발사 시도
    public bool Fire(Vector3 aimTarget)
    {
        // 현재 상태가 발사 가능한 상태
        // && 마지막 총 발사 시점에서 timeBetFire 이상의 시간이 지남
        if (state == State.Ready
            && Time.time >= lastFireTime + timeBetFire)
        {
            var xError = Utils.GedRandomNormalDistribution(0f, spread);
            var yError = Utils.GedRandomNormalDistribution(0f, spread);


            var fireDirection = aimTarget - fireTransform.position;

            fireDirection = Quaternion.AngleAxis(yError, Vector3.up) * fireDirection;
            fireDirection = Quaternion.AngleAxis(xError, Vector3.right) * fireDirection;

            spread += 1f / stability;

            // 마지막 총 발사 시점을 갱신
            lastFireTime = Time.time;
            // 실제 발사 처리 실행
            Shot(fireTransform.position, fireDirection);

            return true;
        }

        return false;
    }

    // 실제 발사 처리
    private void Shot(Vector3 startPoint, Vector3 direction)
    {
        // 레이캐스트에 의한 충돌 정보를 저장하는 컨테이너
        RaycastHit hit;
        // 총알이 맞은 곳을 저장할 변수
        var hitPosition = Vector3.zero;

        // 레이캐스트(시작지점, 방향, 충돌 정보 컨테이너, 사정거리)
        if (Physics.Raycast(startPoint, direction, out hit, fireDistance, ~excludeTarget))
        {
            // 레이가 어떤 물체와 충돌한 경우

            // 충돌한 상대방으로부터 IDamageable 오브젝트를 가져오기 시도
            var target =
                hit.collider.GetComponent<IDamageable>();

            // 상대방으로 부터 IDamageable 오브젝트를 가져오는데 성공했다면
            if (target != null)
            {
                DamageMessage damageMessage;

                damageMessage.damager = gunHolder.gameObject;
                damageMessage.amount = damage;
                damageMessage.hitPoint = hit.point;
                damageMessage.hitNormal = hit.normal;

                // 상대방의 OnDamage 함수를 실행시켜서 상대방에게 데미지 주기
                target.ApplyDamage(damageMessage);
            }
            else
            {
                EffectManager.Instance.PlayHitEffect(hit.point, hit.normal, hit.transform);
            }

            // 레이가 충돌한 위치 저장
            hitPosition = hit.point;
        }
        else
        {
            // 레이가 다른 물체와 충돌하지 않았다면
            // 총알이 최대 사정거리까지 날아갔을때의 위치를 충돌 위치로 사용
            hitPosition = startPoint + direction * fireDistance;
        }

        // 발사 이펙트 재생 시작
        StartCoroutine(ShotEffect(hitPosition));

        // 남은 탄환의 수를 -1
        magAmmo--;
        if (magAmmo <= 0)
            // 탄창에 남은 탄약이 없다면, 총의 현재 상태를 Empty으로 갱신
            state = State.Empty;
    }

    // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // 총구 화염 효과 재생
        muzzleFlashEffect.Play();
        // 탄피 배출 효과 재생
        shellEjectEffect.Play();

        // 총격 소리 재생
        gunAudioPlayer.PlayOneShot(shotClip);

        // 선의 시작점은 총구의 위치
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        // 선의 끝점은 입력으로 들어온 충돌 위치
        bulletLineRenderer.SetPosition(1, hitPosition);
        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
        bulletLineRenderer.enabled = false;
    }

    // 재장전 시도
    public bool Reload()
    {
        if (state == State.Reloading ||
            ammoRemain <= 0 || magAmmo >= magCapacity)
            // 이미 재장전 중이거나, 남은 총알이 없거나
            // 탄창에 총알이 이미 가득한 경우 재장전 할수 없다
            return false;

        // 재장전 처리 시작
        StartCoroutine(ReloadRoutine());
        return true;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine()
    {
        // 현재 상태를 재장전 중 상태로 전환
        state = State.Reloading;
        // 재장전 소리 재생
        gunAudioPlayer.PlayOneShot(reloadClip);

        // 재장전 소요 시간 만큼 처리를 쉬기
        yield return new WaitForSeconds(reloadTime);

        // 탄창에 채울 탄약을 계산한다
        var ammoToFill = magCapacity - magAmmo;

        // 탄창에 채워야할 탄약이 남은 탄약보다 많다면,
        // 채워야할 탄약 수를 남은 탄약 수에 맞춰 줄인다
        if (ammoRemain < ammoToFill) ammoToFill = ammoRemain;

        // 탄창을 채운다
        magAmmo += ammoToFill;
        // 남은 탄약에서, 탄창에 채운만큼 탄약을 뺸다
        ammoRemain -= ammoToFill;

        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;
    }

    private void Update()
    {
        spread = Mathf.SmoothDamp(spread, 0f, ref currentSpreadVelocity, 1f / restoreFromRecoilSpeed);
        spread = Mathf.Clamp(spread, 0f, maxSpread);
    }
}