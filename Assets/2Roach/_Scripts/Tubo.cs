using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tubo
{
    private List<Stack> _stackQueue = new List<Stack>();

    public void PickupFirstStackInQueue(Stack grabber) {
        if (_stackQueue.Count <= 0 || grabber == null) return;

        grabber.CombineStacks(_stackQueue[0]);

        _stackQueue.RemoveAt(0);
    }

    public void PlaceStackInQueue(Stack placer) {
        if (_stackQueue.Count <= 0 || placer == null) return;

        _stackQueue.Add(placer);
        placer.ResetStack();
    }
}
