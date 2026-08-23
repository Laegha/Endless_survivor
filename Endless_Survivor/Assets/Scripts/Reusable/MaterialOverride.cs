using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MaterialOverride
{
    public const int maxAuthority = 5;
    //authorities should go from 0 to 5
    [Range(0, maxAuthority)]public int authority;
    public Material material;

    public MaterialOverride(int authority, Material material)
    {
        this.authority = authority;
        this.material = material;
    }
}

public class MaterialAuthorityComparer : IComparer<MaterialOverride>
{
    public int Compare(MaterialOverride materialOverrideA, MaterialOverride materialOverrideB)
    {
        return materialOverrideA.authority.CompareTo(materialOverrideB.authority);
    }
}