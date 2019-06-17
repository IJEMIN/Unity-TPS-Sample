using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// 씬 관리자 관련 코드

// UI 관련 코드

// 필요한 UI에 즉시 접근하고 변경할 수 있도록 허용하는 UI 매니저
public class UIManager : MonoBehaviour
{
    private static UIManager m_instance; // 싱글톤이 할당될 변수

    public Text ammoText; // 탄약 표시용 텍스트
    public Crosshair crosshair;

    public GameObject gameoverUI; // 게임 오버시 활성화할 UI 
    public Text healthText;
    public Text lifeText;
    public Text scoreText; // 점수 표시용 텍스트

    public Text waveText; // 적 웨이브 표시용 텍스트

    // 싱글톤 접근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (m_instance == null) m_instance = FindObjectOfType<UIManager>();

            return m_instance;
        }
    }


    // 탄약 텍스트 갱신
    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = magAmmo + "/" + remainAmmo;
    }

    // 점수 텍스트 갱신
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
    }

    // 적 웨이브 텍스트 갱신
    public void UpdateWaveText(int waves, int count)
    {
        waveText.text = "Wave : " + waves + "\nEnemy Left : " + count;
    }

    public void UpdateLifeText(int count)
    {
        lifeText.text = "Life : " + count;
    }


    // 게임 오버 UI 활성화
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    public void UpdateCrossHairPosition(Vector3 worldPosition)
    {
        crosshair.UpdatePosition(worldPosition);
    }

    public void SetActiveCrosshair(bool active)
    {
        crosshair.SetActiveReticle(active);
    }

    public void UpdateHealthText(float health)
    {
        healthText.text = Mathf.Floor(health).ToString();
    }


    // 게임 재시작
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}