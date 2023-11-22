using System.Collections;
using System.Collections.Generic;

public interface IActivator
{
    void Activate(int activationStyle, int source);

    void Deactivate(int deactivationStyle, int source);
}
