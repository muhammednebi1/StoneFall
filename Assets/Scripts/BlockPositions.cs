using UnityEngine;

public class BlockPositions : MonoBehaviour
{
    GameField gameField;
    public Vector3 relativePosition;
    public Vector3 absolutePosition;
    SpriteRenderer spriteRenderer;
    

    private void CalculateRelativePosition()
    {
        relativePosition = new Vector3(transform.localPosition.x - 0.5f, transform.localPosition.y - 0.5f, transform.localPosition.z);
    }
    public Vector3 GetRelativePosition()
    {
        CalculateRelativePosition();
        return relativePosition;
    }
    // Recalculating absolute position based on bottom-left corner pivot point
    public void CalculateAbsolutePosition()
    {
        absolutePosition = new Vector3(transform.position.x - 0.5f, transform.position.y - 0.5f, transform.position.z);
    }
    public Vector3 GetAbsolutePosition()
    {
        CalculateAbsolutePosition();
        return absolutePosition;
    }
    private void DontRenderOutOfGrid()
    {
        if (GetAbsolutePosition().y >= gameField.sizeY)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
        }
    }
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        gameField = GameObject.FindWithTag("GameField").GetComponent<GameField>();
        CalculateRelativePosition();

        if (transform.parent.tag != "GhostBlock")
        {
            spriteRenderer.enabled = false;

        }
    }

    void Update()
    {
        CalculateAbsolutePosition();
        DontRenderOutOfGrid();
    }
}
