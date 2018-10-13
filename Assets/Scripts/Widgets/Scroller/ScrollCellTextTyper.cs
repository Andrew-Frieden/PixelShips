﻿using System.Collections;
using UnityEngine;

namespace Widgets.Scroller
{
    public class ScrollCellTextTyper : TextTyper
    {
        public delegate void TyperFinishedEvent();
        public static event TyperFinishedEvent scrollCellTyperFinishedEvent;
        
        protected override IEnumerator ShowCharacters(float delay)
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
                visibleCount = counter % (totalCharacters + 3);
                textMesh.maxVisibleCharacters = visibleCount;

                //Start the next cell
                if (visibleCount >= totalCharacters)
                {
                    var siblingIndex = gameObject.transform.parent.GetSiblingIndex();
                    for (var i = siblingIndex; i < gameObject.transform.parent.parent.childCount; i++)
                    {
                        //End if we are on the last cell
                        if (i == gameObject.transform.parent.parent.childCount - 1)
                        {
                            scrollCellTyperFinishedEvent?.Invoke();
                            yield break;
                        }
                    
                        //  TODO don't search for components at runtime like this. We should setup previous/next references between scrollcells
                        var nextScrollCellTransform = gameObject.transform.parent.parent.GetChild(i + 1);
                        var textTyper = nextScrollCellTransform.GetComponentInChildren<TextTyper>();
                        var nextCell = nextScrollCellTransform.GetComponent<ScrollCell>();

                        if (textTyper != null && textTyper.isActiveAndEnabled)
                        {
                            if (nextCell != null)
                                nextCell.OnCellStarted();

                            textTyper.TypeText(delay);
                            yield break;
                        }
                    }
                }

                counter += 3;
                yield return new WaitForSeconds(timeBetweenCharacters);
            }
        }
    }
}