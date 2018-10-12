using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeOmsEngine : MonoBehaviour
{
    public static FakeOmsEngine Instance;
    [SerializeField] protected Patient patient;

    public static System.Action<int> onChangeHealth;

    Dictionary<string, System.Action> possibleActions = new Dictionary<string, System.Action>();
    Dictionary<string, bool> actionsReady = new Dictionary<string, bool>();

    protected virtual void Awake()
    {
        AssignActions();

        Instance = this;
        StartPatientStateCoroutine();
    }

    protected virtual void StartPatientStateCoroutine()
    {
        StartCoroutine("ChangePatientStateCoroutine");
    }

    void AssignActions()
    {
        possibleActions = new Dictionary<string, System.Action>()
        {
            {"GivePill", GivePill},
            {"Kick", Kick},
            {"Kill", Kill},
        };

        foreach (string actionKey in possibleActions.Keys)
        {
            actionsReady.Add(actionKey, true);
        }
    }

    public virtual bool DoAction(string actionKey, ActiveObject activeObject)
    {
        if (possibleActions.ContainsKey(actionKey))
        {
            if (IsActionReady(actionKey))
            {
                activeObject.PerformAction();
                possibleActions[actionKey].Invoke();
                StartCoroutine("ActionCooldown", actionKey);

                return true;
            }
        }

        return false;
    }

    void GivePill()
    {
        if (!patient.isDead)
        {
            SetPatientHealth(patient.health + 20);
        }
    }

    void Kick()
    {
        SetPatientHealth(patient.health - 20);
    }

    void Kill()
    {
        SetPatientHealth(0);
    }

    bool IsActionReady(string actionKey)
    {
        return actionsReady[actionKey];
    }

    protected void SetPatientHealth(int newHealth)
    {
        newHealth = Mathf.Clamp(newHealth, 0, 100);

        if (patient.health != newHealth)
        {
            patient.health = newHealth;
            if (onChangeHealth != null)
            {
                onChangeHealth(patient.health);
            }
        }
    }

    IEnumerator ChangePatientStateCoroutine()
    {
        do
        {
            yield return new WaitForSeconds(1);
            SetPatientHealth(patient.health - 1);
        } while (true);
    }

    IEnumerator ActionCooldown(string actionKey)
    {
        actionsReady[actionKey] = false;
        yield return new WaitForSeconds(1);
        actionsReady[actionKey] = true;
    }

}
