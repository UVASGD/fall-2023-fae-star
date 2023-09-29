using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Transition
{
    public void Transition(int selected);

    public void ReverseTransition();
}
