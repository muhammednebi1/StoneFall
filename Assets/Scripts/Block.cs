using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Block : MonoBehaviour
{

    // Block Types
    [SerializeField] private bool squareBlock;
    [SerializeField] private bool lBlock;
    [SerializeField] private bool stickBlock;

    [SerializeField]
    Sprite[] blockSprites;
    public Sprite sprite;

    // Next Rotation 0->90->180.. Degree Rotated Blocks
    [SerializeField] private GameObject nextRotation;
    [SerializeField] private GameObject nextTestRotation;

    // Ghost Version of the current block type
    [SerializeField] private GameObject ghostVersion;
    private GameObject ghost;


    public float remainingTime = 3;

    // Check if this block is still under player's control or already anchored
    [SerializeField] public bool playable = true;

    // List of block positions script of childrens
    List<BlockPositions> bps = new List<BlockPositions>();

    GameField gameFieldScript;
    GameManager gameManager;

    bool gamePause = false;

    AudioResource blockAnchorAudio;

    void Falling()
    {
        if (playable)
        {
            if (remainingTime <= 0)
            {
                foreach (BlockPositions bp in bps)
                {
                    Vector2 nextWorldPosOfBlock = new Vector2(transform.position.x + bp.relativePosition.x, transform.position.y + bp.relativePosition.y - 1);

                    if (gameFieldScript.IsOccupied((int)nextWorldPosOfBlock.y, (int)nextWorldPosOfBlock.x))
                    {
                        foreach (BlockPositions bpi in bps)
                        {
                            Vector2 worldPosOfBlock = new Vector2(transform.position.x + bpi.relativePosition.x, transform.position.y + bpi.relativePosition.y);
                            gameFieldScript.Hold((int)worldPosOfBlock.y, (int)worldPosOfBlock.x);
                        }

                        Anchor();
                        break;

                    }
                }
            }
            if (remainingTime <= 0)
            {
                remainingTime = 2 / (float)gameManager.difficulity;
                transform.position = new Vector2(transform.position.x, transform.position.y - 1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                remainingTime -= Time.deltaTime * 2;
            }
            else remainingTime -= Time.deltaTime;
        }
    }
    // Rotates ClockWise
    void Rotate()
    {
        bool rotatable = true;
        

        if (Input.GetKeyDown(KeyCode.R) && playable)
        {
            GameObject testRotation = Instantiate(nextTestRotation, transform.position, transform.rotation);
            foreach (BlockPositions bp in testRotation.GetComponentsInChildren<BlockPositions>())
            {
                if (gameFieldScript.IsOccupied((int)bp.GetAbsolutePosition().y, (int)bp.GetAbsolutePosition().x))
                {
                    rotatable = false;
                }
            }
            Destroy(testRotation);

            void SuccessorSprite(GameObject go)
            {
                foreach (Transform child in go.transform)
                {
                    foreach (Transform grandChild in child)
                    {
                        if (grandChild.TryGetComponent(out SpriteRenderer spriteRenderer))
                        {
                            spriteRenderer.sprite = sprite;
                        }
                    }
                }
                go.GetComponent<Block>().sprite = sprite;
            }

            if (rotatable)
            {
                GameObject successor = Instantiate(nextRotation, transform.position, transform.rotation);
                successor.GetComponent<Block>().remainingTime = remainingTime;

                SuccessorSprite(successor);
                Destroy(gameObject);
            }
        }
    }
    // Right-Left Movements
    void Move()
    {
        bool moveable = true;
        if (Input.GetKeyDown(KeyCode.A) && playable)
        {
            foreach (BlockPositions bp in bps)
            {
                Vector2 worldPosOfBlock = new Vector2(transform.position.x + bp.relativePosition.x - 1, transform.position.y + bp.relativePosition.y);

                if (gameFieldScript.IsOccupied((int)worldPosOfBlock.y, (int)worldPosOfBlock.x))
                {
                    moveable = false;
                    break;
                }
            }
            if (moveable)
            {
                transform.position = new Vector2(transform.position.x - 1, transform.position.y);
            }
        }

        if (Input.GetKeyDown(KeyCode.D) && playable)
        {
            foreach (BlockPositions bp in bps)
            {
                Vector2 worldPosOfBlock = new Vector2(transform.position.x + bp.relativePosition.x + 1, transform.position.y + bp.relativePosition.y);

                if (gameFieldScript.IsOccupied((int)worldPosOfBlock.y, (int)worldPosOfBlock.x))
                {
                    moveable = false;
                    break;
                }
            }
            if (moveable)
            {
                transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && playable) 
        {
            bool shake = false;
            while (moveable)
            {
                foreach (BlockPositions bp in bps)
                {
                    Vector2 worldPosOfBlock = new Vector2(transform.position.x + bp.relativePosition.x, transform.position.y + bp.relativePosition.y - 1);

                    if (gameFieldScript.IsOccupied((int)worldPosOfBlock.y, (int)worldPosOfBlock.x))
                    {
                        moveable = false;
                        break;
                    }
                }
                if (moveable)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - 1);
                    shake = true;
                }
            }
            if (shake)
            {
                gameManager.ScreenShake(0.12f, 0.2f);
            }

        }
    }
    // Makes block unplayable
    void Anchor()
    {
        foreach (BlockPositions bp in bps)
        {
            gameFieldScript.AddBlockPosition(bp);
        }
        gameFieldScript.PlayAnchoredSound();
        gameFieldScript.inPlay = false;

        List<Transform> children = new List<Transform>();

        foreach (Transform child in transform)
        {
            children.Add(child);
        }
        foreach(Transform child in children.ToArray())
        {
            child.transform.parent = gameFieldScript.transform;
        }
        Destroy(gameObject);

    }
    // Listing children
    void ListingChilds()
    {
        foreach (Transform b in transform)
        {
            bps.Add(b.GetComponent<BlockPositions>());
        }
    }
    // Apply Random Sprite (Block color) on spawn (beign called from Game Field)
    public void RandomSprite()
    {
        int rand = Random.Range(0, blockSprites.Length);

        foreach (Transform child in transform)
        {
            foreach (Transform grandChild in child)
            {
                if (grandChild.TryGetComponent(out SpriteRenderer spriteRenderer))
                {
                    spriteRenderer.sprite = blockSprites[rand];
                    sprite = blockSprites[rand];
                }
            }
        }
    }
    public void RandomSprite0(int x)
    {
        int rand = x;

        foreach (Transform child in transform)
        {
            foreach (Transform grandChild in child)
            {
                if (grandChild.TryGetComponent(out SpriteRenderer spriteRenderer))
                {
                    spriteRenderer.sprite = blockSprites[rand];
                    sprite = blockSprites[rand];
                }
            }
        }
    }
    // Fall Point Indicator
    private void GhostBlock()
    {
        void GhostEffect(GameObject ghost)
        {
            foreach(Transform child in ghost.transform)
            {
                foreach(Transform grandChild in child)
                {
                    SpriteRenderer spriteRenderer = grandChild.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = sprite;
                    spriteRenderer.color = new Color(1f, 1f, 1f, 0.2f);

                }
            }
        }

        bool pullDown = true;
        Destroy(ghost);
        ghost = Instantiate(ghostVersion, transform.position, transform.rotation);
        ghost.transform.parent = transform;
        ghost.transform.tag = "GhostBlock";
        GhostEffect(ghost);

        while (true)
        {
            foreach (BlockPositions bps in bps)
            {
                if (gameFieldScript.IsOccupied((int)(ghost.transform.position.y + bps.GetRelativePosition().y - 1), (int)(ghost.transform.position.x + bps.GetRelativePosition().x)))
                {
                    pullDown = false;
                }
            }
            if (!pullDown) break;

            ghost.transform.position = new Vector3(ghost.transform.position.x, ghost.transform.position.y - 1, ghost.transform.position.z);
        }

    }
    // Rendering Out of Grids


    void Start()
    {

        gameFieldScript = GameObject.FindWithTag("GameField").GetComponent<GameField>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        ListingChilds();

        blockAnchorAudio = gameFieldScript.blockAnchorAudio;
    }

    void Update()
    {
        gamePause = gameManager.gamePaused;
        if (!gamePause)
        {

            Falling();
            Rotate();
            Move();
            GhostBlock();
            

        }

    }
}
