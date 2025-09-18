using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GremCombinationsSO", menuName = "Grems/Grem Combinations")]
public class GremCombinationsSO : ScriptableObject
{
    public List<GremCombination> combinations;

    public GremCombination GetCombination(string gremA, string gremB)
    {
        foreach (var combo in combinations)
        {
            if (combo.Matches(gremA, gremB))
                return combo;
        }
        return null;
    }
}

[System.Serializable]
public class GremCombination
{
    public string resultGremID;          
    public CombinationElement[] inputs;  

    public bool Matches(string a, string b)
    {
        if (inputs.Length != 2) return false;
        return (inputs[0].id == a && inputs[1].id == b) ||
               (inputs[0].id == b && inputs[1].id == a);
    }
}


