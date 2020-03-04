using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourMap : MonoBehaviour
{
    public Dictionary<UnitController.TargetType, System.Type> behaviourMap = new Dictionary<UnitController.TargetType, System.Type>();

    public List<UnitController.TargetType> targetTypes = new List<UnitController.TargetType>();
}
