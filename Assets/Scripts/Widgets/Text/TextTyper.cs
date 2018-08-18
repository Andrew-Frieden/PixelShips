using UnityEngine;
using System.Collections;
using TMPro;

public class TextTyper : MonoBehaviour
{
    [SerializeField] private float TimeToWrite;
    [SerializeField] private TextMeshProUGUI textMesh;

    public void Type()
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
                yield break;
            }

            counter += 1;
            yield return new WaitForSeconds(timeBetweenCharacters);
        }
    }
}