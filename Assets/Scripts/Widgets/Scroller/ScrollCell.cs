using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollCell : MonoBehaviour
{
    public RectTransform RectTransform;
    public TextMeshProUGUI Text;

    public Vector3 textBounds_size
    {
        get
        {
            return Text.textInfo.textComponent.textBounds.size;
        }
    }
    public Vector3 textBounds_center
    {
        get
        {
            return Text.textInfo.textComponent.textBounds.center;
        }
    }
    public Vector3 rectTransform_anchoredPostion
    {
        get
        {
            return Text.textInfo.textComponent.rectTransform.anchoredPosition;
        }
    }

    void Start()
    {
        this.RectTransform = GetComponent<RectTransform>();
        Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetupEmptyCell()
    {
        // reset text

        //var myDictionary = new Dictionary<string, int>();
        //foreach (var item in myDictionary.Keys)
        //{
        //    var value = myDictionary[item];
        //}
    }

    public void ChangeSize()
    {
        RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, RectTransform.sizeDelta.y * 1.5f);
    }
    
    public void RecycleCell()
    {
        //  empty stuff / make inactive?
    }
    
    public void SetDisplay(string text)
    {
        //   set some display stuff
        Text.text = text;
        //ChangeSize();

        //RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, Text.bounds.size.y);
        //Debug.Log(RectTransform.sizeDelta + " " + Text.bounds.size);
    }

    public void SetupTest()
    {
        Text.text = GetTestOutput();
    }

    private string GetTestOutput()
    {
        return TestOutputs[Random.Range(0, TestOutputs.Count)];
    }

    private List<string> _testOutputs;
    private List<string> TestOutputs
    {
        get
        {
            if (_testOutputs == null)
            {
                _testOutputs = new List<string>()
                {
                    "A pirate has warped into the sector. His weapons are already hot!",
                    "You detect the dissipating remains of a warp trail.",
                    "A large [Resourcium Asteroid] makes its way through the sector.",
                    "Your jump drive begins to spin up...",
                    "[Pirate 1ef7] fires a torpedo at you! Impact in [4.5s]",
                    "Your scanners detect a space station in a nearby sector.",
                    "Shields have fully recharged.",
                    "Weapons systems spinning up...",
                    "Weapon systems online.",
                    "Weapon systems damaged!",
                    "Shields depleted!",
                    "[Space Debris] causes <scratching> damage to your hull!",
                    "A rogue [Plasma Flare] causes <penetrating> damage to your hull!",
                    "[Pirate 1ef7]'s plasma cannon <rends> your hull!",
                    "[Pirate 4ab3]'s blaster causes <incinerating> damage to your hull!",
                    "A [Space Drone 5da2] is destroyed.",
                    "A [Passenger Cruiser 6fr0] warps into the sector.",
                    "The [Space Station 97hk] slowly rotates here.",
                    "A small [Techinite Asteroid] falls through space.",
                    "A far off shooting star streaks across the horizon."
                };
            }
            return _testOutputs;
        }
    }
}