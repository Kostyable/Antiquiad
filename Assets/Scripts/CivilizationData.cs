using System;
using System.Collections.Generic;

[Serializable]
public class CivilizationData
{
    public CivilizationName name;
    public Dictionary<ResourceType, float> ResourcesValues;
    public Dictionary<ResourceType, float> ResourcesAdds;
}