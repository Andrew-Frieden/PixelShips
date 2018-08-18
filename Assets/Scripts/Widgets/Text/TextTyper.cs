using UnityEngine;
using System.Collections;
using TMPro;

public class TextTyper : MonoBehaviour
{
    [SerializeField] private float TimeToWrite = 2.0f;
    [SerializeField] private TextMeshProUGUI textMesh;

    public void HideText()
    {
        textMesh.maxVisibleCharacters = 0;
    }

    public void ShowText()
    {
        textMesh.maxVisibleCharacters = textMesh.textInfo.characterCount;
    }

    public void TypeText()
    {
        StartCoroutine(ShowCharacters());
    }

    IEnumerator ShowCharacters()
    {
        // Force an update of the mesh to get valid information.
        textMesh.ForceMeshUpdate();

        int totalCharacters = textMesh.textInfo.characterCount;
        int counter = 0;
        int visibleCount = 0;
        float timeBetweenCharacters = TimeToWrite / totalCharacters;

        while (true)
        {
            visibleCount = counter % (totalCharacters + 1);
            textMesh.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalCharacters)
            {
                yield return new WaitForSeconds(5);
                //yield break;
            }

            counter += 1;
            yield return new WaitForSeconds(timeBetweenCharacters);
        }
    }
}