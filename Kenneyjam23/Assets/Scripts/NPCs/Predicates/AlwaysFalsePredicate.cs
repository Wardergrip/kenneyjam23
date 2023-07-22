using UnityEngine;

[CreateAssetMenu(menuName = "Dialog Predicates/AlwaysFalsePredicate")]
public class AlwaysFalse : DialogPredicate
{
    public override bool Validate()
    {
        return false;
    }
}