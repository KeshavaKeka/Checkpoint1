using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerBattle1 : MonoBehaviour
{
    public GameObject[] enemies;
    private float xPos = 13;
    private float zPos = 40;
    private float startTime = 2.5f;
    private int number = 0;
    public int maxEnemies = 25;
    public PC script;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI levelCompletedText;
    public Damage damage;
    public Damage damage2;
    public Button shoot;
    public Button strike;
    public FixedJoystick joystick;
    public Button resume;
    public Button pause;
    public bool isGameActive;
    public GameObject background;
    public GameObject charac;
    public Button restart;
    public Button quit;
    private float strikeAgain = 0;

    void Start()
    {
        if (script == null || gameOverText == null || damage == null || damage2 == null)
        {
            Debug.LogError("One or more GameObjects are not assigned in the Inspector!");
            return;
        }
        isGameActive = true;
        Invoke("SpawnEnemy", startTime);
    }

    void Update()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && number >= maxEnemies)
        {
            GameOver();
        }
        if(strikeAgain>0)
        {
            strikeAgain -= Time.deltaTime;
        }
    }

    public void Pause()
    {
        pause.gameObject.SetActive(false);
        resume.gameObject.SetActive(true);
        background.SetActive(true);
        charac.SetActive(true);
        restart.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        resume.gameObject.SetActive(false);
        pause.gameObject.SetActive(true);
        background.SetActive(false);
        charac.SetActive(false);
        restart.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        resume.gameObject.SetActive(false);
        pause.gameObject.SetActive(true);
        background.SetActive(false);
        charac.SetActive(false);
        restart.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        if (damage.currentHealth <= 0 || damage2.currentPlayerHealth <= 0)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
            isGameActive = false;
            shoot.gameObject.SetActive(false);
            joystick.gameObject.SetActive(false);
            pause.gameObject.SetActive(false);
            strike.gameObject.SetActive(false);
            background.SetActive(true);
            restart.gameObject.SetActive(true);
            charac.SetActive(true);
            gameOverText.gameObject.SetActive(true);
        }
        else if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            isGameActive = false;
            shoot.gameObject.SetActive(false);
            joystick.gameObject.SetActive(false);
            pause.gameObject.SetActive(false);
            background.SetActive(true);
            restart.gameObject.SetActive(true);
            strike.gameObject.SetActive(false);
            charac.SetActive(true);
            levelCompletedText.gameObject.SetActive(true);
        }
    }

    public void Shoot()
    {
        if (script != null && strikeAgain <= 0)
        {
            script.ShootArrow();
            strikeAgain = 0.7f;
        }
    }

    void SpawnEnemy()
    {
        int index = Random.Range(0, 3) % 2;
        if (number < maxEnemies && isGameActive)
        {
            float[] pos = new float[2];
            pos[0] = Random.Range(-50, xPos);
            pos[1] = Random.Range(xPos,50);
            int i = Random.Range(0, 2);
            Vector3 sPos = new Vector3(pos[i], enemies[index].transform.position.y, Random.Range(15, zPos));
            GameObject newEnemy = Instantiate(enemies[index], sPos, enemies[index].transform.rotation);
            Animator anim = newEnemy.GetComponentInChildren<Animator>();
            if (anim != null)
            {
                AnimatorOverrideController overrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
                anim.runtimeAnimatorController = overrideController;
            }
            number++;
            float nextInvoke = Random.Range(2, 4);
            Invoke("SpawnEnemy", nextInvoke);
        }
        else
        {
            GameOver();
        }
    }
}