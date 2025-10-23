using UnityEngine;

public class SevenSegment : MonoBehaviour
{
    bool[,] digits = { {true, true, true, true, true, true, false},
                        {false, true, true, false, false, false, false},
                        {true, true, false, true, true, false, true},
                        {true, true, true, true, false, false, true},
                        {false, true, true, false, false, true, true},
                        {true, false, true, true, false, true, true},
                        {true, false, true, true, true, true, true },
                        {true, true, true, false, false, false, false},
                        {true, true, true, true, true, true, true },
                        {true, true, true, true, false, true, true}};
    public bool[] inputs = new bool[7];

    public bool a;
    public bool b;
    public bool c;
    public bool d;
    public bool e;
    public bool f;
    public bool g;

    public GameObject aSprite;
    public GameObject bSprite;
    public GameObject cSprite;
    public GameObject dSprite;
    public GameObject eSprite;
    public GameObject fSprite;
    public GameObject gSprite;

    SpriteRenderer aRenderer;
    SpriteRenderer bRenderer;
    SpriteRenderer cRenderer;
    SpriteRenderer dRenderer;
    SpriteRenderer eRenderer;
    SpriteRenderer fRenderer;
    SpriteRenderer gRenderer;

    public void DigitsToInputs(int digit)
    {
        for (int i = 0; i < inputs.Length; i++) {
            inputs[i] = digits[digit, i];
        }
    }
    void LEDS()
    {
        if (inputs[0] == true)
        {
            aRenderer.color = Color.red;
        }
        if (inputs[0] == false)
        {
            aRenderer.color = new Color(0, 0, 0, 0);
        }
        if (inputs[1] == true)
        {
            bRenderer.color = Color.red;
        }
        if (inputs[1] == false)
        {
            bRenderer.color = new Color(0, 0, 0, 0);
        }
        if (inputs[2] == true)
        {
            cRenderer.color = Color.red;
        }
        if (inputs[2] == false)
        {
            cRenderer.color = new Color(0, 0, 0, 0);
        }
        if (inputs[3] == true)
        {
            dRenderer.color = Color.red;
        }
        if (inputs[3] == false)
        {
            dRenderer.color = new Color(0, 0, 0, 0);
        }
        if (inputs[4] == true)
        {
            eRenderer.color = Color.red;
        }
        if (inputs[4] == false)
        {
            eRenderer.color = new Color(0, 0, 0, 0);
        }
        if (inputs[5] == true)
        {
            fRenderer.color = Color.red;
        }
        if (inputs[5] == false)
        {
            fRenderer.color = new Color(0, 0, 0, 0);
        }
        if (inputs[6] == true)
        {
            gRenderer.color = Color.red;
        }
        if (inputs[6] == false)
        {
            gRenderer.color = new Color(0, 0, 0, 0);
        }

    }

    void Start()
    {
        aRenderer = aSprite.GetComponent<SpriteRenderer>();
        bRenderer = bSprite.GetComponent<SpriteRenderer>();
        cRenderer = cSprite.GetComponent<SpriteRenderer>();
        dRenderer = dSprite.GetComponent<SpriteRenderer>();
        eRenderer = eSprite.GetComponent<SpriteRenderer>();
        fRenderer = fSprite.GetComponent<SpriteRenderer>();
        gRenderer = gSprite.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        LEDS();
    }
}
