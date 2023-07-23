using UnityEngine;

public abstract class DialogPredicate : ScriptableObject
{
    public abstract bool Validate();
}