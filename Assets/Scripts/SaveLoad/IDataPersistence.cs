using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence // the existence of this interface allows us to check what things have data that needs to be saved. currently not needed, but could be useful later
{
    void LoadData(SaveData data);

    void SaveData(SaveData data);
}