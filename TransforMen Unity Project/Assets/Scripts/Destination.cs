using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination
{
    public enum DestinationType
    {
        PositionalTarget,
        UnitTarget
    }

    public Vector3 Position {
        get {
            switch (Type) {
                case DestinationType.PositionalTarget:
                    return PositionalTarget.Value;
                case DestinationType.UnitTarget:
                    return UnitTarget.GetComponent<Collider>().bounds.center;
                default:
                    throw new System.Exception("Invalid DestinationType");
            }
        }
    }

    public DestinationType Type { get; private set; }
    public Vector3? PositionalTarget { get; private set; }
    public GameObject UnitTarget { get; private set; }

    public Destination(Vector3 positionalTarget) {
        PositionalTarget = positionalTarget;
        Type = DestinationType.PositionalTarget;
    }

    public Destination(GameObject unitTarget) {
        UnitTarget = unitTarget;
        Type = DestinationType.UnitTarget;
    }

    public bool Exists() {
        switch (Type) {
            case DestinationType.PositionalTarget:
                return PositionalTarget.HasValue;
            case DestinationType.UnitTarget:
                return UnitTarget != null;
            default:
                return false;
        }
    }

    public Vector3 ClosestPoint(Vector3 reference) {
        switch (Type) {
            case DestinationType.PositionalTarget:
                return PositionalTarget.Value;
            case DestinationType.UnitTarget:
                return UnitTarget.GetComponent<Collider>().ClosestPoint(reference);
            default:
                throw new System.Exception("Invalid DestinationType");
        }
    }
}
