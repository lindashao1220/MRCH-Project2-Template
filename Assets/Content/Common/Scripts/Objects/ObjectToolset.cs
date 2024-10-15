using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class ObjectToolset : MonoBehaviour
{
    [InfoBox("Includes ToggleComponentEnabled for components and ToggleObjectEnabled for objects")]
    public virtual void ToggleComponentEnabled(Component component)
    {
        if (component is Behaviour behaviourComponent)
        {
            behaviourComponent.enabled = !behaviourComponent.enabled;
        }
        else
        {
            Debug.LogWarning("The provided component doesn't have an 'enabled' property.");
        }
    }

    public virtual void ToggleObjectEnabled(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
