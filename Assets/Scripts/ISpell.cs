using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpell
{
    void SetTarget(GameObject newTarget);

    void Prepare();

    void Execute();
}
