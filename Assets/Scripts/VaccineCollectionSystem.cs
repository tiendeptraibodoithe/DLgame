using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VaccineCollectionSystem : MonoBehaviour
{
    [Header("Vaccine Mission")]
    [SerializeField] private int requiredVaccines = 3;
    private int collectedVaccines = 0;

    [Header("Zombie Mission")]
    [SerializeField] private int requiredZombieKills = 3;
    private int zombieKillCount = 0;

    [Header("UI Elements")]
    [SerializeField] private Text vaccineCountText;
    [SerializeField] private Text zombieCountText;
    [SerializeField] private GameObject missionCompleteUI;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button quitButton;

    [Header("Player Settings")]
    [SerializeField] private MonoBehaviour playerController;
    [SerializeField] private MonoBehaviour playerCamera; // Reference to player camera controller if you have one

    private bool isMissionComplete = false;

    private void Awake()
    {
        // Đảm bảo TimeScale được set đúng ngay khi object được tạo
        ResetGameState();
    }

    private void Start()
    {
        // Khởi tạo UI và game state
        InitializeGame();
    }

    private void InitializeGame()
    {
        // Reset game state để đảm bảo
        ResetGameState();

        // Setup UI
        missionCompleteUI.SetActive(false);
        UpdateVaccineUI();
        UpdateZombieUI();

        // Setup button listeners
        if (nextLevelButton != null) nextLevelButton.onClick.AddListener(LoadNextLevel);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);

        // Lock cursor
        SetCursorState(false);

        Debug.Log($"Game initialized with TimeScale: {Time.timeScale}");
    }

    private void Update()
    {
        // Chỉ cho phép pickup khi game đang chạy
        if (!isMissionComplete && Time.timeScale > 0)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                CheckForVaccinePickup();
            }
        }

        // Escape chỉ hoạt động khi mission complete
        if (Input.GetKeyDown(KeyCode.Escape) && isMissionComplete)
        {
            QuitGame();
        }
    }

    private void CheckForVaccinePickup()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Vaccine"))
            {
                CollectVaccine(hitCollider.gameObject);
                break;
            }
        }
    }

    private void CollectVaccine(GameObject vaccineObject)
    {
        collectedVaccines++;
        UpdateVaccineUI();

        VaccineBeacon beacon = vaccineObject.GetComponent<VaccineBeacon>();
        if (beacon != null)
        {
            beacon.DeactivateBeacon();
        }

        Destroy(vaccineObject);
        CheckMissionCompletion();
    }

    public void OnZombieKilled()
    {
        zombieKillCount++;
        UpdateZombieUI();
        CheckMissionCompletion();
    }

    private void UpdateVaccineUI()
    {
        if (vaccineCountText != null)
        {
            vaccineCountText.text = $"Vaccines: {collectedVaccines}/{requiredVaccines}";
        }
    }

    private void UpdateZombieUI()
    {
        if (zombieCountText != null)
        {
            zombieCountText.text = $"Zombies Killed: {zombieKillCount}/{requiredZombieKills}";
        }
    }

    private void CheckMissionCompletion()
    {
        if (collectedVaccines >= requiredVaccines && zombieKillCount >= requiredZombieKills)
        {
            CompleteMission();
        }
    }

    private void CompleteMission()
    {
        isMissionComplete = true;

        // Show UI
        if (missionCompleteUI != null)
        {
            missionCompleteUI.SetActive(true);
        }

        // Show cursor
        SetCursorState(true);

        // Stop game
        Time.timeScale = 0;

        // Disable player controls
        DisablePlayerControls();

        Debug.Log("Mission Complete - TimeScale set to 0");
    }

    private void LoadNextLevel()
    {
        ResetGameState();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex >= totalScenes - 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    private void QuitGame()
    {
        ResetGameState();
        SceneManager.LoadScene(0);
    }

    private void ResetGameState()
    {
        // Reset time
        Time.timeScale = 1f;

        // Reset mission state
        isMissionComplete = false;

        // Hide cursor
        SetCursorState(false);

        // Enable player controls
        EnablePlayerControls();

        Debug.Log($"Game state reset - TimeScale: {Time.timeScale}");
    }

    private void SetCursorState(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void EnablePlayerControls()
    {
        if (playerController != null) playerController.enabled = true;
        if (playerCamera != null) playerCamera.enabled = true;
    }

    private void DisablePlayerControls()
    {
        if (playerController != null) playerController.enabled = false;
        if (playerCamera != null) playerCamera.enabled = false;
    }

    private void OnDestroy()
    {
        // Cleanup khi object bị hủy
        ResetGameState();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}