using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Upgrade;

public class UpgradeButton : MonoBehaviour
{
    public SpriteRenderer graphic;

    public bool mouseState = false;

    Vector3 baseScale = Vector3.one;
    Vector3 growScale = new Vector3(1.1f, 1.1f, 1.1f);

    public float growSpeed = 25.0f;

    private Upgrade upgrade;

    public GameObject upgradeScreen;
    public Upgrade.Upgrades upgradeType;

    // Start is called before the first frame update
    void Start()
    {
        upgradeScreen = GameObject.Find("UpgradeScreen");
        upgrade = upgradeScreen.GetComponent<Upgrade>();
    }

    void Update()
    {
         
        if(mouseState)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, growScale, Time.deltaTime * growSpeed);
            graphic.color = Color.white;

            if(Input.GetMouseButtonDown(0))
            {
                if(upgradeType == Upgrade.Upgrades.UpgradeBaseValue)
                {
                    DiceManager diceManager= GameObject.Find("DiceManager").GetComponent<DiceManager>();
                    diceManager.baseVal++;
                }
                else if (upgradeType == Upgrade.Upgrades.AddDice)
                {
                    Deck deck = GameObject.Find("Deck").GetComponent<Deck>();

                    DiceProps diceProps = ScriptableObject.CreateInstance<DiceProps>();
                    diceProps.name = "Dice_" + deck.diceDeck.Count.ToString();
                    deck.diceDeck.Add(diceProps);
                }
                else if (upgradeType == Upgrade.Upgrades.UpgradeArea)
                {
                    DiceManager diceManager = GameObject.Find("DiceManager").GetComponent<DiceManager>();
                    diceManager.baseArea+=0.1f;
                }
                upgradeScreen.SetActive(false);

                GameObject.Find("Requirments_AddDice").GetComponent<AddDice>().UpdateRequirments(false);

            }
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, baseScale, Time.deltaTime * growSpeed);
            graphic.color = new Color(1.0f, 1.0f, 1.0f, 0.8f);
        }

    }

    private void OnEnable()
    {
        mouseState = false;
    }

        private void OnMouseEnter()
    {        
        mouseState = true;
    }

    private void OnMouseExit()
    {
        mouseState = false;
    }
}
