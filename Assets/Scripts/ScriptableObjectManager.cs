using UnityEngine;

public class ScriptableObjectManager
{
    private DataSettings dataSettings;
    
    
    public DataSettings GetDataSettings()
    {
        if (dataSettings == null)
        {
            dataSettings = Resources.Load<DataSettings>("Data/DataSettings");
        }

        return dataSettings;
    }
}