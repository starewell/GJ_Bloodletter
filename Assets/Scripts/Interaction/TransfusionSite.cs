using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEditor;
using UnityEngine;

public class TransfusionSite : HoldInteractable {

    
    public float infectionHeal, infectionDilution;    

    public override void Interact() {
        StartCoroutine(TransfuseBlood(bloodletter));
    }

    public IEnumerator TransfuseBlood(Bloodletter bloodletter) {
        interacting = true;
        DebugUI.instance.StartCoroutine(DebugUI.instance.DisplayHoldInteract(this));
        while (Input.GetMouseButton(0) && interacting && inRange &&
        content > 0  && bloodletter.bloodLevel < 100) {
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
            
            if (bloodletter.bloodLevel < 100)
                bloodletter.bloodLevel += consumptionRate;
            if (bloodletter.infectionPotency > bloodletter.potencyRange.x)
                bloodletter.infectionPotency -= infectionDilution;
            if (bloodletter.infectionLevel > 0)
                bloodletter.infectionLevel -= infectionHeal;
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



}