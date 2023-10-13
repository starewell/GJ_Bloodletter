using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TollSite : HoldInteractable {


    public override void Interact() {
        StartCoroutine(RifleSite(bloodletter));
    }

    public IEnumerator RifleSite(Bloodletter bloodletter) {
        interacting = true;
        DebugUI.instance.StartCoroutine(DebugUI.instance.DisplayHoldInteract(this));
        while (Input.GetMouseButton(0) && interacting && inRange &&
        content > 0) {
            while (!bloodletter.tick) {
                yield return null;
                if (!Input.GetMouseButton(0)) {
                    interacting = false;
                    break;
                }
            }
            if (!Input.GetMouseButton(0)) {
                    interacting = false;
                    break;
            }
            
            content -= consumptionRate;

            if (!inRange) {
                interacting = false;
                break;
            }
            yield return null;
        }
// USED ALL BLOOD
        if (content <= 0) {
            ExhaustSite();
        }
        interacting = false;    
    }

    protected override void ExhaustSite() {
        base.ExhaustSite();
        bloodletter.tollCount += Random.Range(1,2);
        DebugUI.instance.textPopUp.DismissMessage();
    }

}