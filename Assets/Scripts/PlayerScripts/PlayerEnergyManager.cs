using System.Collections;
using UnityEngine;

public class PlayerEnergyManager : MonoBehaviour
{
    public EnergySO playerEnergyData;
    Coroutine rechargeCoroutine;
    bool isRecharging = false;

    void OnEnable()
    {
        AbilityHandler.OnAbilityFinished += TriggerRecharge;
        AbilityHandler.OnAbilityUsed += OnEnergyUse;
    }

    void OnDisable()
    {
        AbilityHandler.OnAbilityFinished -= TriggerRecharge;
        AbilityHandler.OnAbilityUsed -= OnEnergyUse;
    }

    public void TriggerRecharge()
    {
        if (CanRecharge() == false) return;
        rechargeCoroutine = StartCoroutine(RechargeOvertime());

    }

    public IEnumerator RechargeOvertime()
    {
        while (playerEnergyData.currentEnergy < playerEnergyData.maxEnergy)
        {
            isRecharging = true;
            playerEnergyData.currentEnergy += playerEnergyData.rechargeRate;

            if (playerEnergyData.currentEnergy > playerEnergyData.maxEnergy)
            {
                playerEnergyData.currentEnergy = playerEnergyData.maxEnergy;
                UIEvents.RaiseEnergyChanged(playerEnergyData.currentEnergy, playerEnergyData.maxEnergy);
                isRecharging = false;
                yield break;
            }

            UIEvents.RaiseEnergyChanged(playerEnergyData.currentEnergy, playerEnergyData.maxEnergy);
            yield return new WaitForSeconds(.1f);
        }
        isRecharging = false;
        rechargeCoroutine = null;
}

    /*public IEnumerator InstantRechargeCoroutine(AbilityData abilityData)
    {
        yield return new WaitForSeconds(rechargeInterval);
        currentEnergy = maxEnergy;
        rechargeCoroutine = null;
    }*/

    public bool CanRecharge()
    {
        return playerEnergyData.currentEnergy != playerEnergyData.maxEnergy && !isRecharging;
    }

    public void OnEnergyUse(AbilityData abilityData)
    {
        float energyAfterUse = playerEnergyData.currentEnergy - abilityData.energyCost;
        if (Mathf.Approximately(energyAfterUse, 0))
        {
            playerEnergyData.currentEnergy = 0;
            UIEvents.RaiseEnergyChanged(playerEnergyData.currentEnergy, playerEnergyData.maxEnergy);
            return;
        }

        playerEnergyData.currentEnergy -= abilityData.energyCost;
        UIEvents.RaiseEnergyChanged(playerEnergyData.currentEnergy, playerEnergyData.maxEnergy);
    }
}
