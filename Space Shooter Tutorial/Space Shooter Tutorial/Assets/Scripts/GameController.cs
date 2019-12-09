using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public GameObject starfield;
    public GameObject stardistant;
    public float fSliderValue;
    public float dSliderValue;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public float speed;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Text winText;
    public Text hardText;
    public Text easyText;
    private bool gameOver;
    private bool restart;
    private int score;
    private bool hardMode;
    private bool easyMode;

    private AudioSource win;
    private AudioSource lose;
    private AudioSource bgm;

    private ParticleSystem starfield1;
    private ParticleSystem starfield2;
    

    void Start()
    {
        gameOver = false;
        restart = false;
        hardMode = false;
        easyMode = false;
        restartText.text = "";
        gameOverText.text = "";
        winText.text = "";
        hardText.text = "Hard Mode Deactived: Press 'H'";
        easyText.text = "Easy Mode Deactived: Press 'E'";
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
        var source = GetComponents<AudioSource>();
        bgm = source[0];
        win = source[1];
        lose = source[2];
        bgm.Play();
        starfield1 = GameObject.Find("part_starField").GetComponent<ParticleSystem>();
        starfield2 = GameObject.Find("part_starField_distant").GetComponent<ParticleSystem>();
        fSliderValue = 2.0f;
        dSliderValue = 2.0f;

    }

    void Update()
    {

        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                SceneManager.LoadScene("Main");
            }
        }

        var starfieldmain = starfield1.main;
        starfieldmain.simulationSpeed = fSliderValue;
        var starfield2main = starfield2.main;
        starfield2main.simulationSpeed = dSliderValue;
    }

    void FixedUpdate()
    {
        if(Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            hardMode = true;
            hardText.text = "Hard Mode Activated";
            restartText.text = "Press 'U' for Restart";
            restart = true;
            spawnWait = 0.2f;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            easyMode = true;
            easyText.text = "Easy Mode Activated";
            restartText.text = "Press 'U' for Restart";
            restart = true;
            spawnWait = 0.8f;
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }

            if (gameOver)
            {
                restartText.text = "Press 'U' for Restart";
                restart = true;
                break;
            }
        }
    }

    public void AddScore (int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Points: " + score;

        var scoreNeeded = 100;
        if (hardMode == true)
            scoreNeeded = 300;

        if (easyMode == true)
            scoreNeeded = 50;

        if (score >= scoreNeeded)
        {
            winText.text = "You win! " + "Game created by Heather Parrett";
            gameOverText.text = "";
            gameOver = true;
            restart = true;
            bgm.Stop();
            win.Play();
            GameObject.Find("Background").GetComponent<BGScroller>().scrollSpeed = -15;
            fSliderValue = 20.0f;
            dSliderValue = 20.0f;



        }
    }

    public void GameOver ()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
        bgm.Stop();
        lose.Play();
        fSliderValue = 0.0f;
        dSliderValue = 0.0f;

    }
}