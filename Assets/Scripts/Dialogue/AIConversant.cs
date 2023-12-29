using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;

public class AIConversant : MonoBehaviour, IRaycastable
{
    public CursorType GetCursorType()
    {
        return CursorType.Dialogue;
    }

    public bool HandleRaycast(PlayerController callingController)
    {
        return true;
    }
}
