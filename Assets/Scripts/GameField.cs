using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameField : MonoBehaviour
{
    public int sizeX = 8;
    public int sizeY = 16;
    [SerializeField]
    private GameObject gridSpriteBlack;
    [SerializeField]
    private GameObject gridSpriteWhite;

    [SerializeField] private GameObject squareBlock;
    [SerializeField] private GameObject lBlock;
    [SerializeField] private GameObject stickBlock;
    [SerializeField] private GameObject tBlock;


    public bool[] grids;

    public List<BlockPositions> blockPositions = new List<BlockPositions>();

    public bool inPlay = false;
    public bool gameOver = false;

    public GameObject gameManager;
    public GameManager gameManagerScript;

    public List<int> blocksAndSprites;
    public GameObject nextBlockIndicator;
    public NextBlockIndicator nextBlockScript;

    float miniPauseDuration = 0;

    public Material burnOutMaterial;

    public AudioResource blockAnchorAudio;

    // Clears Array in X dimension
    public void ReleaseRow(int y)
    {
        for(int i = 0; i < sizeX; i++) {
            grids[y*sizeX + i] = false;
        }
    }
    // Clears Blocks in Game
    public void ReleaseRowBlocks(int y)
    {
        foreach(BlockPositions bp in blockPositions.ToArray())
        {
            if (bp.GetAbsolutePosition().y == y) {
                blockPositions.Remove(bp);
                Destroy(bp.gameObject);
            }
        }
    }
    // Clears a Given point from array
    public void Release(int y, int x)
    {
        grids[y*sizeX + x] = false;
    }
    // Sets a Given point in array
    public void Hold(int y, int x)
    {
        if (y >= sizeY)
        {
            GameOver();
            Debug.Log("GameOver");
            return;
        }
        grids[(y*sizeX + x)] = true;
    }
    // Check if the point is set in the array
    public bool IsOccupied(int y, int x)
    {
        if (x >= sizeX) return true;
        if (y < 0 || x < 0) return true;
        if (y < sizeY && x < sizeX && y >= 0 && x >= 0)
        {
            if (grids[y * sizeX + x] == false)
            {
                return false;
            }
        }
        if (y >= sizeY) return false;
        return true;
    }
    // Adds BlockPositions to blockPosition list
    public void AddBlockPosition(BlockPositions bp)
    {
        blockPositions.Add(bp);
    }
    // Removes BlockPositions from blockPosition list
    public void RemoveBlockPosition(BlockPositions bp)
    {
        blockPositions.Remove(bp);
    }
    // Blocks must be seperated from their parents in order to destroy them 1by1;
    private void Boom()
    {
        List<int> rows = new List<int>();
        int rowCount = 0;

        for (int i = 0;i < sizeY;i++)
        {
            int columnOccupiedCount = 0;
            for (int j = 0; j < sizeX; j++)
            {
                if (IsOccupied(i, j))
                {
                    columnOccupiedCount++;
                }
                else
                {
                    columnOccupiedCount = 0;
                }
            }

            if (columnOccupiedCount == sizeX)
            {
                rows.Add(i);
                rowCount++;
                ReleaseRowBlocks(i);
                ReleaseRow(i);
                PullDownOnce(i);
                i = -1;

            }
        }
        gameManagerScript.ScreenShake(0.1f * rowCount, 0.25f * rowCount);
        gameManagerScript.AddScore(rowCount);
    }
    // Gravity for after Boom function (y: the point that gravity will be applied, rowCount: how many rows will block go down)
    private void PullDownOnce(int y)
    {

        foreach (BlockPositions bp in blockPositions)
        {
            if (bp.GetAbsolutePosition().y > y)
            {
                Release((int)bp.GetAbsolutePosition().y, (int)bp.GetAbsolutePosition().x);
                bp.transform.position = new Vector2(bp.transform.position.x, bp.transform.position.y - 1);
            }
        }
        foreach (BlockPositions bp in blockPositions)
        {
            Hold((int)bp.GetAbsolutePosition().y, (int)bp.GetAbsolutePosition().x);
        }


    }
    // Visible Grid
    private void CreateGrid()
    {
        for (int i = 0; i < sizeY; i++)
        {
            for(int j = 0;j < sizeX; j++)
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
                GameObject grid = GameObject.Instantiate(gridSprite, new Vector2(j + 0.5f, i + 0.5f), transform.rotation, transform);
                grid.transform.GetComponent<SpriteRenderer>().sortingOrder = -1;
            }
        }
    }
    private void Spawn(int rand, int spriteIndex)
    {
        if (!inPlay)
        {
            void SpawnBlock(GameObject go, int x, int y)
            {
                GameObject block = Instantiate(go, new Vector2(x, y), transform.rotation);
                block.transform.GetComponent<Block>().RandomSprite0(spriteIndex);
            }

            if (rand == 0)
            {
                SpawnBlock(squareBlock, (int)(sizeX / 2), sizeY + 3);
            }
            else if (rand == 1)
            {
                SpawnBlock(lBlock, (int)(sizeX / 2), sizeY + 3);

            }
            else if (rand == 2)
            {
                SpawnBlock(stickBlock, (int)(sizeX / 2), sizeY + 3);

            }
            else if (rand == 3)
            {
                SpawnBlock(tBlock, (int)(sizeX / 2), sizeY + 3);

            }
            inPlay = true;

            ReOrganizeBCList();

        }
    }
    private void Randomize()
    {
        for (int i = 0; i < 2; i++)
        {
            int randBlock = Random.Range(0, 3);
            int randSprite = Random.Range(0, 7);

            blocksAndSprites.Add(randBlock);
            blocksAndSprites.Add(randSprite);
        }    
    }
    private void ReOrganizeBCList() 
    {
        blocksAndSprites.RemoveAt(0);
        blocksAndSprites.RemoveAt(0);

        int randBlock = Random.Range(0, 4);
        int randSprite = Random.Range(0, 7);

        blocksAndSprites.Add(randBlock);
        blocksAndSprites.Add(randSprite);

    }
    private void GameOver()
    {
        gameOver = true;
        gameManagerScript.gameOver = true;
    }
    public void PlayAnchoredSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        audioSource.resource = blockAnchorAudio;
        audioSource.Play();
    }

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        
        nextBlockScript = nextBlockIndicator.GetComponent<NextBlockIndicator>();

        grids = new bool[sizeY * sizeX];

        nextBlockScript.blocksAndSprites = blocksAndSprites;
        CreateGrid();
        Randomize();
    }

    void Update()
    {
        Boom();

        if (!gameOver)
        {
            Spawn(blocksAndSprites[0], blocksAndSprites[1]);
        }
    }
}
