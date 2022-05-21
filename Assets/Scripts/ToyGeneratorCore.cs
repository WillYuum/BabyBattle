using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyGeneratorCore : MonoBehaviour
{

    private List<IToyGeneratorMethods> _listOfToyGenerators;

    void Awake()
    {
        _listOfToyGenerators = new List<IToyGeneratorMethods>();
        InvokeRepeating(nameof(Generate), 0, 1);
    }


    private void Generate()
    {
        foreach (var data in _listOfToyGenerators)
        {
            data.GenerateToys(data.ToysGeneratedPerSec);
        }
    }

    public void Add(IToyGeneratorMethods ability)
    {

        _listOfToyGenerators.Add(ability);
    }


    public void Remove(GainToysAbility ability)
    {
        if (_listOfToyGenerators == null)
        {
            return;
        }

        _listOfToyGenerators.Remove(ability);
    }
}
