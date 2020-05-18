using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.AI.Planner;
using UnityEngine.AI.Planner.Controller;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using UnityEngine.AI.Planner.DomainLanguage.TraitBased;
using Generated.AI.Planner.StateRepresentation;


public class plannerCallbacks : MonoBehaviour
{
    Moves moves;
    UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        moves = this.GetComponent<Moves>();
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void Steal(GameObject treasure)
    {
        Debug.Log("Steal");
        treasure.GetComponent<Renderer>().enabled = false;
    }

    public IEnumerator Seek(GameObject treasure, GameObject copGO, GameObject robberGO)
    {
        Debug.Log("Approach");
        agent.SetDestination(treasure.transform.position);
        while ((Vector3.Distance(treasure.transform.position, transform.position) > 2f) &&
               (Vector3.Distance(treasure.transform.position, copGO.transform.position) >= 10f))
            yield return null;
        if (Vector3.Distance(treasure.transform.position, copGO.transform.position) < 10f)
        { // Go to Wander: cop.farAway
            TraitComponent tc = copGO.GetComponent<TraitComponent>();
            ITraitData tdt = tc.GetTraitData<Cop>();
            tdt.SetValue("farAway", false);
        }
        else
        { // Go to Steal: robber.ready2steal
            TraitComponent tc = robberGO.GetComponent<TraitComponent>();
            Debug.Log(tc.name);
            ITraitData tdt = tc.GetTraitData<Robber>();
            tdt.SetValue("ready2steal", true);
            Debug.Log(tdt.GetValue("ready2steal"));
        }
    }

    public IEnumerator Wander(GameObject cop, GameObject treasure)
    {
        Debug.Log("Wander");
        while (Vector3.Distance(treasure.transform.position, cop.transform.position) < 10f)
        {
            moves.Wander();
            yield return null;
        }
    }    
}
