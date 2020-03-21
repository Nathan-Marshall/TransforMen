using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UnitAction
{
    System.Action GetAction(GameObject target);

    System.Action GetStopAction();
}
