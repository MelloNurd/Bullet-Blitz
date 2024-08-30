using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{

    [SerializeField] TMP_Text waveCountText;
    [SerializeField] Image timerBar;

    [SerializeField] float timeToNextWave;
    float waveTimer;

    [SerializeField] GameObject basicZombie, hardZombie, hardestZombie;
    List<GameObject> zombies = new List<GameObject>();

    float totalElapsedTime;
    
    GameObject player;

    int waveCount;

    bool roundStarted;

    [SerializeField] GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        timeToNextWave = 10;
        player = GameObject.Find("Player");

        player.GetComponent<Player>().NewWaveSetup();
        NewWave();
        StartCoroutine(StartRound());
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            Time.timeScale = pauseMenu.activeSelf ? 0f : 1f;
        }

        if (roundStarted)
        {
            if (waveTimer - Time.time <= 0)
            {
                // START NEW WAVE
                waveCountText.text = "!";
                timerBar.transform.localScale = Vector3.one;
                roundStarted = false;
                NewWave();
                StartCoroutine(StartRound());
            }

            timerBar.transform.localScale = new Vector3((waveTimer - Time.time) / timeToNextWave, 1, 1);
        }
    }

    public void Unpause() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ReloadScene() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }

    public void NewWave()
    {
        waveCount++;
        if (timeToNextWave < 30) timeToNextWave += 5;
        GetComponent<BulletPoolScript>().ClearAllObjects();

        for(int i = 0; i < 3; i++)
            SpawnNewZombie(basicZombie);

        if (waveCount % 3 == 0)
            SpawnNewZombie(hardZombie);

        if (waveCount % 5 == 0)
            SpawnNewZombie(hardestZombie);

        foreach(GameObject zombie in zombies)
        {
            StartCoroutine(zombie.GetComponent<EnemyBehaviour>().EnableShooting());
            zombie.GetComponent<EnemyBehaviour>().SetNewMoveOffset();
            do
            {
                float xPos = Random.Range(-41, 41);
                float yPos = Random.Range(-41, 40);
                zombie.transform.position = new Vector3(xPos, yPos, 0);
            }
            while (Vector2.Distance(zombie.transform.position, player.transform.position) < 15);
                
        }
    }

    void SpawnNewZombie(GameObject type)
    {
        GameObject newZombie = Instantiate(type);

        do
        {
            float xPos = Random.Range(-41, 41);
            float yPos = Random.Range(-41, 40);
            newZombie.transform.position = new Vector3(xPos, yPos, 0);
        } while (Vector2.Distance(newZombie.transform.position, player.transform.position) < 15);

        zombies.Add(newZombie);
    }

    IEnumerator StartRound()
    {
        yield return new WaitForSeconds(3f);
        waveCountText.text = waveCount.ToString();
        waveTimer = Time.time + timeToNextWave;
        roundStarted = true;
    }
}
