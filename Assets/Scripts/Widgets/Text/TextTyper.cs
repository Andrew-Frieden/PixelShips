using UnityEngine;
using System.Collections;
using TMPro;

public class TextTyper : MonoBehaviour
{
    [SerializeField] private float TimeToWrite = 1.0f;
    [SerializeField] private TextMeshProUGUI textMesh;

    private Coroutine typingRoutine;

    public void HideText()
    {
        textMesh.maxVisibleCharacters = 0;
    }

    public void ShowText()
    {
        textMesh.maxVisibleCharacters = textMesh.textInfo.characterCount;
    }

    void Start()
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void TypeText(float delay)
    {
        if (typingRoutine != null)
        {
            StopCoroutine(typingRoutine);
            HideText();
        }
        typingRoutine = StartCoroutine(ShowCharacters(delay));
    }

    IEnumerator ShowCharacters(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Force an update of the mesh to get valid information.
        textMesh.ForceMeshUpdate();

        int totalCharacters = textMesh.textInfo.characterCount;
        int counter = 0;
        int visibleCount = 0;
        float timeBetweenCharacters = TimeToWrite / totalCharacters;
        
        if (timeBetweenCharacters >= 0.02f)
        {
            timeBetweenCharacters = 0.02f;
        }

        while (true)
        {
            visibleCount = counter % (totalCharacters + 1);
            textMesh.maxVisibleCharacters = visibleCount;

            //Start the next cell
            if (visibleCount >= totalCharacters)
            {
                var siblingIndex = gameObject.transform.parent.GetSiblingIndex();
                for (var i = siblingIndex; i < gameObject.transform.parent.parent.GetChildCount(); i++)
                {
                    //End if we are on the last cell
                    if (i == gameObject.transform.parent.parent.GetChildCount() - 1)
                    {
                        yield break;
                    }
                    
                    var nextScrollCell = gameObject.transform.parent.parent.GetChild(i + 1);
                    var textTyper = nextScrollCell.GetChild(0) != null ? nextScrollCell.GetChild(0).GetComponent<TextTyper>() : null;
                    if (textTyper != null && textTyper.isActiveAndEnabled)
                    {
                        textTyper.TypeText(0.1f);
                        yield break;
                    }
                }
            }

            counter += 1;
            yield return new WaitForSeconds(timeBetweenCharacters);
        }
    }
}