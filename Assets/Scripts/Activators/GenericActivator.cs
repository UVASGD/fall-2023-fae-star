using System.Collections;
using System.Collections.Generic;

public interface IActivator
{
    void Activate(int activationStyle);

    void Deactivate(int deactivationStyle);
}
