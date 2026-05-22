using MarbleBash.Encounters;
using UnityEngine;

public class StartEncounterButton : PhysicalButton
{
    [Header("To Spawn:")]
    [SerializeField] private EncounterElement[] _elements;
    [SerializeField] private float _radius = 15f;

    protected override void Activate()
    {
        Vector3 pos = transform.position + (Vector3.up * 5f);
        
        Encounter newEncounter = new Encounter(pos, _radius);

        foreach (EncounterElement e in _elements)
        {
            newEncounter.AddElement(e);
        }
    
        EncounterManager.StartEncounter(newEncounter);

    }


}
