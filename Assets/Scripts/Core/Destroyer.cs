using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] GameObject objectToDestroy;

        public void DestroyGameObject()
        {
            Destroy(objectToDestroy);
        }
    }
}
