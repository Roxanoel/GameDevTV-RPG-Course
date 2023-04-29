using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        // This class is responsible for fading a canvas in and out

        CanvasGroup canvasGroup;

        Coroutine currentlyActiveFade = null;

        private void Start()
        {
            if (canvasGroup != null) return;
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 1;
        }

        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }

        public Coroutine FadeOut(float time)
        {
            return Fade(1, time);
        }

        public Coroutine Fade(float alphaTarget, float time)
        {
            // Stop any pre-existing fades from running
            if (currentlyActiveFade != null)
            {
                StopCoroutine(currentlyActiveFade);
            }

            // Run fade out coroutine
            currentlyActiveFade = StartCoroutine(FadeRoutine(alphaTarget, time));

            return currentlyActiveFade;
        }


        private IEnumerator FadeRoutine(float alphaTarget, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, alphaTarget))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, alphaTarget, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}
