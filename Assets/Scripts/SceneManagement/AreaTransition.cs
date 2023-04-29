using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using RPG.Control;

namespace RPG.SceneManagement
{
    public class AreaTransition : MonoBehaviour
    {
        // This class manages transitions between areas; placed on each area transition portal
        
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }
        
        [SerializeField] int indexOfSceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [Header("Fading in and out")]
        [SerializeField] float fadeTime = 3.0f;
        [SerializeField] float waitTimeAfterFade = 1.0f;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (indexOfSceneToLoad < 0)
            {
                Debug.LogError("Scene to load is not set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            DisableControl();

            yield return fader.FadeOut(fadeTime);


            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(indexOfSceneToLoad);
            DisableControl();

            savingWrapper.Load();

            AreaTransition newAreaEntrance = GetNewAreaEntrance();
            UpdatePlayer(newAreaEntrance);

            savingWrapper.Save();

            yield return new WaitForSeconds(waitTimeAfterFade);
            fader.FadeIn(fadeTime);

            EnableControl();

            Destroy(gameObject);
        }

        private void DisableControl()
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl()
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = true;
        }

        private AreaTransition GetNewAreaEntrance()
        {
            foreach (AreaTransition a in FindObjectsOfType<AreaTransition>())
            {
                if (a == this || !CheckIfDestinationMatches(a)) continue;

                return a;
            }
            return null;
        }

        private void UpdatePlayer(AreaTransition newAreaEntrance)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = newAreaEntrance.spawnPoint.position;
            player.transform.rotation = newAreaEntrance.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }
        private bool CheckIfDestinationMatches(AreaTransition areaTransitionToCheck)
        {
            return this.destination == areaTransitionToCheck.destination;
        }
    }
}
