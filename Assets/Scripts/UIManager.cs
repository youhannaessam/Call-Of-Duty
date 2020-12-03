using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _bestScoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Button _PlayAgain;
    [SerializeField]
    private Button _HighScores;
    [SerializeField]
    private Button _Exit;
    private int _BestScore = 0;
    public int score;
    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _PlayAgain.gameObject.SetActive(false);
        _HighScores.gameObject.SetActive(false);
        _Exit.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
        score = playerScore;
        /*_score = playerScore;
        CheckBestScore();*/
    }/*
    public void CheckBestScore()
    {
        if (_score > _BestScore)
        {
            _BestScore = _score;
            PlayerPrefs.SetInt("Best: ", _BestScore);
            _bestScoreText.text = "Best : " + _BestScore;
        }
            
    }*/
    public void UpdateLives (int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];

        if(currentLives == 0)
        {
            GameOverSequence();
        }
    }
    IEnumerator ExampleCoroutine()
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(1);
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(ExampleCoroutine());
       // _restartText.gameObject.SetActive(true);
        //StartCoroutine(GameOverFlickerRoutine());
        _PlayAgain.gameObject.SetActive(true);
        _HighScores.gameObject.SetActive(true);
        _Exit.gameObject.SetActive(true);
    }
    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }
    public void HighScore()
    {

    }
    public void Exit()
    {
        Application.Quit();
    }
}
