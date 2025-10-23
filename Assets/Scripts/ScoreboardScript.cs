using UnityEngine;
using UnityEngine.UI;

public class ScoreboardScript : MonoBehaviour
{
    public int score = 0;
    public int highScore = 0;
    int sizeY = 4;
    int sizeX = 5;
    public GameObject gridSpriteBlack;
    public GameObject gridSpriteWhite;

    public GameObject digits0;
    public GameObject digits1;
    public GameObject digits2;
    public GameObject digits3;

    public GameObject highScoreDigits0;
    public GameObject highScoreDigits1;
    public GameObject highScoreDigits2;
    public GameObject highScoreDigits3;

    GameManager gameManager;

    private void CreateGrid()
    {
        GameObject gridParent = new GameObject();
        gridParent.transform.parent = transform;
        gridParent.transform.position = Vector3.zero;
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                GameObject gridSprite;
                if ((i + j) % 2 == 0)
                {
                    gridSprite = gridSpriteBlack;
                }
                else
                {
                    gridSprite = gridSpriteWhite;
                }
                GameObject grid = Instantiate(gridSprite, new Vector2(transform.position.x - sizeX / 2f + j + 0.5f, transform.position.y - sizeY / 2f + i + 0.5f), transform.rotation, transform);
                grid.transform.GetComponent<SpriteRenderer>().sortingOrder = -1;
                grid.transform.parent = gridParent.transform;
            }
        }
    }

    void ScoreToSevenSegment()
    {
        // Current Score
        int temp = score;
        digits0.GetComponent<SevenSegment>().DigitsToInputs(temp % 10);
        temp /= 10;

        digits1.GetComponent<SevenSegment>().DigitsToInputs(temp % 10);
        temp /= 10;

        digits2.GetComponent<SevenSegment>().DigitsToInputs(temp % 10);
        temp /= 10;

        digits3.GetComponent<SevenSegment>().DigitsToInputs(temp % 10);


        // HighScore
        temp = highScore;
        highScoreDigits0.GetComponent<SevenSegment>().DigitsToInputs(temp % 10);
        temp /= 10;

        highScoreDigits1.GetComponent<SevenSegment>().DigitsToInputs(temp % 10);
        temp /= 10;

        highScoreDigits2.GetComponent<SevenSegment>().DigitsToInputs(temp % 10);
        temp /= 10;

        highScoreDigits3.GetComponent<SevenSegment>().DigitsToInputs(temp % 10);
    }

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        CreateGrid();
    }

    void Update()
    {
        score = gameManager.score;
        highScore = gameManager.highScore;
        ScoreToSevenSegment();
    }
}
