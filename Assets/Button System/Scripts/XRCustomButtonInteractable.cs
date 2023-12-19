using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(BoxCollider))]
public class XRCustomButtonInteractable : XRSimpleInteractable
{
   
    public UnityEvent OnClicked;

    [SerializeField] private Material mainMaterial;
    [SerializeField] private Material highlightedMaterial;

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material = mainMaterial;
    }

    public void HighlightUIObject(bool pValue)
    {
        if (pValue)
        {
            _renderer.material = highlightedMaterial;
        }
        else
        {
            _renderer.material = mainMaterial;
           
        }
    }

    public UnityEvent GetUnityEvent()
    {
        return OnClicked;
    }

    [ContextMenu("ClickButton")]
    public void Call()
    {
        OnClicked?.Invoke();
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        HandUIController interactiveUI = null;

        if (args.interactorObject.transform.TryGetComponent(out interactiveUI))
        {
            HighlightUIObject(true);
            
            interactiveUI.ListenForTriggerButton(GetUnityEvent());
            
        }
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        HandUIController interactiveUI = null;

        if (args.interactorObject.transform.TryGetComponent(out interactiveUI))
        {
            HighlightUIObject(false);
            interactiveUI.CancelListenForTriggerButton();

        }

    }

}


