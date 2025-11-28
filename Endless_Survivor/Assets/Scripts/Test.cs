using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] MaterialManager materialManager;
    [SerializeField] MaterialOverride[] materials;
    private void Start()
    {
        foreach (var mat in materials)
            materialManager.SetMaterialOverride(mat);
    }
}