using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public class HandUIController : MonoBehaviour
{
    
    private InputActionProperty triggerAction;

    [SerializeField]private UnityEvent _event;

    private void Awake()
    {
        triggerAction = GetComponent<ActionBasedController>().activateAction;
    }

    protected void OnEnable()
    {
        
        triggerAction.action.Enable();

    }

    protected void OnDisable()
    {
        
        triggerAction.action.Disable();
    }

    public void ListenForTriggerButton(UnityEvent unityEvent)
    {
        triggerAction.action.started += OnTriggerButtonPressed;
        _event = unityEvent;

    }

    public void CancelListenForTriggerButton()
    {
        triggerAction.action.started -= OnTriggerButtonPressed;
        _event = null;

    }

    private void OnTriggerButtonPressed(InputAction.CallbackContext obj)
    {
        _event.Invoke();
        
    }

 

}
