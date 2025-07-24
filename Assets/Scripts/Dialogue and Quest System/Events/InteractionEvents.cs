using UnityEngine;
using System;

public class InteractionEvents
{
    public event Action<GameObject> onInteract;

    public void Interact(GameObject target)
    {
        onInteract?.Invoke(target);
        //Debug.Log("Event System: Interaction logged");
    }


}
