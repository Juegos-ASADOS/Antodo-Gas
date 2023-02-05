using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CallOnTriggerEfector : MonoBehaviour
{
    [SerializeField] List<string> InteractorObjectTags;
    [SerializeField] UnityEvent InteractiableObjectEvent;

    private void OnTriggerEnter(Collider other)
    {
        //Si el tag del objeto se encuentra en la lista de los interactor, se lanza el metodo
        if (InteractorObjectTags.Contains(other.gameObject.tag))
        {
            InteractiableObjectEvent.Invoke();
        }
    }
}
