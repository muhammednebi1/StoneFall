using System.Collections.Generic;
using UnityEngine;

public class NextBlockIndicator : MonoBehaviour
{
    public int sizeY = 9;
    public int sizeX = 5;

    [SerializeField]
    GameObject gridSpriteBlack;
    [SerializeField]
    GameObject gridSpriteWhite;

    [SerializeField]
    GameObject firstLocation;
    [SerializeField]
    GameObject secondLocation;

    public List<int> blocksAndSprites;
    public Sprite[] blockSprites;

    [SerializeField]
    GameObject squareBlock;
    [SerializeField]
    GameObject lBlock;
    [SerializeField] 
    GameObject stickBlock;
    [SerializeField]
    GameObject tBlock;

    GameObject firstBlock;
    GameObject secondBlock;

    [SerializeField]
    GameObject gameField;
    GameField gameFieldScript;

    public void NextBlocks()
    {
        void SpawnBlock(GameObject go, int spriteIndex, GameObject nextLocation, int firstOrSecond)
        {
            GameObject nextBlock = Instantiate(go, nextLocation.transform.position, transform.rotation);

            foreach (Transform child in nextBlock.transform)
            {
                foreach (Transform grandChild in child)
                {
                    grandChild.GetComponent<SpriteRenderer>().sprite = blockSprites[spriteIndex];
                    grandChild.GetComponent<SpriteRenderer>().sortingOrder = 0;
                    
                }
            }
            if (firstOrSecond == 0)
            {
                firstBlock = nextBlock;
            }
            else if (firstOrSecond == 1)
            {
                secondBlock = nextBlock;
            }
        }

        Destroy(firstBlock);

        if (blocksAndSprites[0] == 0)
        {
            SpawnBlock(squareBlock, blocksAndSprites[1], firstLocation, 0);
        }
        else if (blocksAndSprites[0] == 1)
        {
             SpawnBlock(lBlock, blocksAndSprites[1], firstLocation, 0);
        }
        else if (blocksAndSprites[0] == 2)
        {
            SpawnBlock(stickBlock, blocksAndSprites[1], firstLocation, 0);
        }
        else if (blocksAndSprites[0] == 3)
        {
            SpawnBlock(tBlock, blocksAndSprites[1], firstLocation, 0);
        }

        Destroy(secondBlock);

        if (blocksAndSprites[2] == 0)
        {
            SpawnBlock(squareBlock, blocksAndSprites[3], secondLocation, 1);
        }
        else if (blocksAndSprites[2] == 1)
        {
            SpawnBlock(lBlock, blocksAndSprites[3], secondLocation, 1);
        }
        else if (blocksAndSprites[2] == 2)
        {
            SpawnBlock(stickBlock, blocksAndSprites[3], secondLocation, 1);
        }
        else if (blocksAndSprites[2] == 3)
        {
            SpawnBlock(tBlock, blocksAndSprites[3], secondLocation, 1);
        }
    }

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
                GameObject grid = GameObject.Instantiate(gridSprite, new Vector2(transform.position.x - sizeX / 2f + j + 0.5f,transform.position.y - sizeY / 2f + i + 0.5f), transform.rotation, transform);
                grid.transform.GetComponent<SpriteRenderer>().sortingOrder = -1;
                grid.transform.parent = gridParent.transform;
            }
        }
    }


    void Start()
    {
        gameFieldScript = gameField.GetComponent<GameField>();

        CreateGrid();




    }

    void Update()
    {
        NextBlocks();
    }
}
