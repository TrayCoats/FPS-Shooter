using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    //Componets and variables//
    public static gameManager instance;                     //The single instance of the gameManager singleton

    [Header("---------- Player Stuff ----------")]                    //Player game object and controller
    [SerializeField] public GameObject player;
    [SerializeField] public playerController playerScript;
    [SerializeField] public GameObject cameraObject;
    [SerializeField] public cameraControls cameraScript;
    [SerializeField] public GameObject playerSpawnLocation;
    [SerializeField] public Image HPBar;
    [SerializeField] public Image HPBarDelay;
    [SerializeField] public Image SprintBar;
    [SerializeField] public Image critHeathImg;
    [SerializeField] public Image dmgIndicator;
    [SerializeField] public PlayerStats playerStats;


    [Header("---------- UI Stuff ----------")]                        //UI menus and HUD elements
    [SerializeField] public GameObject activeMenu;
    [SerializeField] public GameObject mainMenu;
    [SerializeField] public GameObject HTPMenu;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject winMenu;
    [SerializeField] public GameObject loseMenu;
    [SerializeField] public GameObject checkPoint;
    [SerializeField] public GameObject reticle;

    [SerializeField] public bool isPaused;


    [Header("---------- Time Delayed Text ----------")]                        //UI menus and HUD elements
    [SerializeField] public TextMeshProUGUI endGoalText;
    public float endGoalTextDelayTimer;
    [SerializeField] public TextMeshProUGUI enemiesText;
    public float enemiesTextDelayTimer;
    [SerializeField] public TextMeshProUGUI weaponsText;
    public float weaponsTextDelayTimer;
    [SerializeField] public TextMeshProUGUI lavaText;
    public float lavaTextDelayTimer;
    [SerializeField] public TextMeshProUGUI bossArrivalText;
    [SerializeField] public TextMeshProUGUI introSkipText;
    public float introSkipTextDelayTimer;


    [Header("----------Enemy Stuff----------")]
    public TextMeshProUGUI enemiesRemainingText;
    public int enemiesRemaining;
    [SerializeField] public enemyAI bossEnemyScript;
    [SerializeField] public GameObject bossEnemy;
    private bool bossSpawned = false;

    [Header("----------Audio Stuff----------")]
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip backgroundMusic;
    [SerializeField] public AudioClip bossBattleMusic;
    public GameObject  fader;


    float timeScaleOriginal;
    [Header("BulletUpgrades")]
    public bool rapidUpgrade = false;
    public bool gravityUpgrade = false;
    public bool iceUpgrade = false;
    bool bossTime = false;
    void Awake()
    {
        instance = this;
        timeScaleOriginal = Time.timeScale;
        player = GameObject.FindGameObjectWithTag("Player");
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        //bossEnemy = GameObject.FindGameObjectWithTag("Boss");
        playerSpawnLocation = GameObject.FindGameObjectWithTag("Spawn Location");
        if (player != null)
        {
            playerScript = player.GetComponent<playerController>();
            cameraScript = cameraObject.GetComponent<cameraControls>();
            bossEnemyScript = bossEnemy.GetComponent<enemyAI>();
        }
      
       
    }

    private void Start()
    {
       if (player!= null)
        {
            // play music //
            PlayBackgroundMusic();
            reticle.SetActive(false);

            // intro text display // 
            if (!cameraScript.enableIntroSequence)
            {
                StopAllCoroutines();
            }
            else
            {
                endGoalText.gameObject.SetActive(false);     // end goal
                StartCoroutine(endGoalTextFunction());
                enemiesText.gameObject.SetActive(false);     // enemies
                StartCoroutine(enemiesTextFunction());
                weaponsText.gameObject.SetActive(false);     // weapons 
                StartCoroutine(weaponsTextFunction());
                lavaText.gameObject.SetActive(false);        // lava
                StartCoroutine(lavaTextFunction());
                bossArrivalText.gameObject.SetActive(false); // enemy boss
                StartCoroutine(bossArrivalTextFunction());
                introSkipText.gameObject.SetActive(false);   // intro skip 
                StartCoroutine(introSkipTextFunction());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the boss is destroyed and switch music
        if (player != null){
            if (bossEnemyScript.isBossDestroyed && audioSource.clip != backgroundMusic)
            {
                PlayBackgroundMusic();
            }

            if (Input.GetButtonDown("Cancel") && activeMenu == null)    //Check for escape key press
            {

                isPaused = !isPaused;                           //Toggle paused and set pause menu as active (or inactive)
                activeMenu = pauseMenu;
                activeMenu.SetActive(isPaused);

                if (isPaused)                                   //Check for pause state
                {
                    playerScript.gunModel.gameObject.SetActive(false);
                    playerScript.canShoot = false;
                    pauseState();
                }
                else
                {

                    unpauseState();

                }

            }
            if (activeMenu == pauseMenu || activeMenu == loseMenu || activeMenu == winMenu)
            {
                fader.SetActive(false);
            }

            if (!cameraScript.introFinsished && !cameraScript.isSkippingIntro && bossSpawned)
            {
                if (audioSource.clip == bossBattleMusic && audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }

            // start timer for boss spawning after intro is finished or skipped //
            if (cameraScript.introFinsished || cameraScript.isSkippingIntro)
            {
                reticle.SetActive(true);
                spawnBoss();
                if (bossTime == false)
                {
                    bossTime = true;
                    bossEnemyScript.StartBossTimer();
                }
             
            }
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus && isPaused == false)
        {
            isPaused = !isPaused;                           //Toggle paused and set pause menu as active (or inactive)
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
            playerScript.gunModel.gameObject.SetActive(false);
            playerScript.canShoot = false;
            pauseState();
        }
    }

    public void pauseState()
    {
       
        Time.timeScale = 0;                                 //Za Waruldo! Toki wo tomare! Oh, and allows the cursor to move in the window
        Cursor.visible = true;                              /*The World! Time is stopped!*/
        Cursor.lockState = CursorLockMode.Confined;
        
    }

    public void unpauseState()
    {
        Time.timeScale = timeScaleOriginal;                 //Active window is deactivated, cursor is locked, and time is set back
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
        }
      

       activeMenu = null;
    }
    public void unpauseStateWithCursor()
    {
        Time.timeScale = timeScaleOriginal;                 //Active window is deactivated, cursor is locked, and time is set back
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
        }

        activeMenu = null;
    }

    public void playerDead()
    {
        pauseState();                                       //Set lose menu to active menu and pause the game
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }

    private void spawnBoss()
    {
        if (bossSpawned) return;
       
        bossEnemyScript.startBossRising();
        StartCoroutine(PlayBossBattleMusicAfterDelay(bossEnemyScript.riseDelay));
        StartCoroutine(bossArrivalTextFunction());
        bossSpawned = true;
    }

    private void PlayAudioClip(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void PlayBackgroundMusic()
    {
        audioSource.clip = backgroundMusic;
        audioSource.Play();
    }

    public void PlayBossBattleMusic()
    {
        audioSource.clip = bossBattleMusic;
        audioSource.Play();
    }


    IEnumerator PlayBossBattleMusicAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (bossEnemy.activeInHierarchy && bossSpawned)
        {
            PlayBossBattleMusic();
        }
    }

    IEnumerator endGoalTextFunction()
    {
        yield return new WaitForSeconds(endGoalTextDelayTimer);
        if (!cameraScript.isSkippingIntro)
        {
            endGoalText.gameObject.SetActive(true);
            yield return new WaitForSeconds(4);
            endGoalText.gameObject.SetActive(false);
        }
    }

    IEnumerator enemiesTextFunction()
    {
        yield return new WaitForSeconds(enemiesTextDelayTimer);
        if (!cameraScript.isSkippingIntro)
        {
            enemiesText.gameObject.SetActive(true);
            yield return new WaitForSeconds(4);
            enemiesText.gameObject.SetActive(false);
        }
    }

    IEnumerator weaponsTextFunction()
    {
        yield return new WaitForSeconds(weaponsTextDelayTimer);
        if (!cameraScript.isSkippingIntro)
        {
            weaponsText.gameObject.SetActive(true);
            yield return new WaitForSeconds(4);
            weaponsText.gameObject.SetActive(false);
        }
    }

    IEnumerator lavaTextFunction()
    {
        yield return new WaitForSeconds(lavaTextDelayTimer);
        if (!cameraScript.isSkippingIntro)
        {
            lavaText.gameObject.SetActive(true);
            yield return new WaitForSeconds(4);
            lavaText.gameObject.SetActive(false);
        }
    }

    IEnumerator bossArrivalTextFunction()
    {
        yield return new WaitForSeconds(bossEnemyScript.riseDelay);
        if (!cameraScript.isSkippingIntro)
        {
            bossArrivalText.gameObject.SetActive(true);
            yield return new WaitForSeconds(4);
            bossArrivalText.gameObject.SetActive(false);
        }
    }

    IEnumerator introSkipTextFunction()
    {
        yield return new WaitForSeconds(introSkipTextDelayTimer);
        if (!cameraScript.isSkippingIntro)
        {
            introSkipText.gameObject.SetActive(true);

            // Wait until intro is done or skipped, then disable intro skip text //
            while (!cameraScript.introFinsished && !cameraScript.isSkippingIntro)
            {
                yield return null;
            }

            introSkipText.gameObject.SetActive(false);
        }
    }
}
