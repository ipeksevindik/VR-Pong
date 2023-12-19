using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbableHandler : XRGrabInteractable
{
    public Transform handler;
    public IXRSelectInteractor interactor;
    private PhotonView pView;

    private void Start()
    {
        pView= GetComponent<PhotonView>();
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        transform.position = handler.transform.position;
        transform.rotation = handler.transform.rotation;

        Rigidbody rbhandler = handler.GetComponent<Rigidbody>();
        rbhandler.velocity = Vector3.zero;
        rbhandler.angularVelocity = Vector3.zero;

    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        interactor = args.interactorObject;
        pView.RequestOwnership();

    }


    private void Update()
    {
        if(Vector3.Distance(handler.position, transform.position) > 0.3f)
        {
            interactionManager.CancelInteractorSelection(interactor);
        }
    }
}
