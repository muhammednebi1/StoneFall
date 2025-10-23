using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BackgroundBlockSpawner : MonoBehaviour
{
    public int bgX;
    public int bgY;
    [SerializeField]
    private GameObject gridSpriteBlack;
    [SerializeField]
    private GameObject gridSpriteWhite;

    [SerializeField]
    GameObject lBlock3D;
    [SerializeField]
    GameObject squareBlock3D;
    [SerializeField]
    GameObject stickBlock3D;

    [SerializeField]
    Sprite[] blockSprites;

    List<GameObject> blocks = new List<GameObject>();

    [SerializeField]
    int spawnerY = 16;

    float timeRemaining = 0;
    [SerializeField]
    float spawnInterval;

    private void BlockListing()
    {
        blocks.Add(lBlock3D);
        blocks.Add(squareBlock3D);
        blocks.Add(stickBlock3D);
    }
    void SpawnBlock()
    {
        if (timeRemaining < 0)
        {
            GameObject block = Instantiate(blocks[Random.Range(0, blocks.Count - 1)], new Vector2(Random.Range(-20, 20), Random.Range(spawnerY, spawnerY + 10)), transform.rotation, transform);

            float scaleMultiplier = Random.Range(1f, 0.25f);

            block.transform.localScale *= scaleMultiplier;

            Rigidbody bRb = block.GetComponent<Rigidbody>();
            bRb.linearDamping = 1 / scaleMultiplier;

            int spriteIndex = Random.Range(0, blockSprites.Length);

            foreach (Transform child in block.transform)
            {
                foreach (Transform grandChild in child.transform)
                {
                    grandChild.GetComponent<SpriteRenderer>().sprite = blockSprites[spriteIndex];
                }
            }

            timeRemaining = spawnInterval;
        }
        timeRemaining -= Time.deltaTime;
    }

    private void BackgroundTexture()
    {
        for(int i = -bgY; i < bgY; i++)
        {
            for (int j = -bgX; j < bgX; j++)
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
                grid.transform.GetComponent<SpriteRenderer>().sortingOrder = -3;

            }
        }
    }

    private void Start()
    {
        BlockListing();
    }
    private void Update()
    {
        SpawnBlock();   
    }
}
