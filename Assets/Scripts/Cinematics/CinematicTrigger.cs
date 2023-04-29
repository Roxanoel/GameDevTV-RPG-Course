using UnityEngine;
using UnityEngine.Playables;
using GameDevTV.Saving;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        // This class is responsible for triggering cinematic when player enters trigger collider
        
        bool cinematicHasPlayed = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && !cinematicHasPlayed)
            {
                GetComponent<PlayableDirector>().Play();
                cinematicHasPlayed = true;
            }
        }


        // ISaveable methods below

        public object CaptureState()
        {
            return cinematicHasPlayed;
        }

        public void RestoreState(object state)
        {
            cinematicHasPlayed = (bool)state;
        }
    }
}
