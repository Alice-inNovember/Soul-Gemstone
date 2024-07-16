using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UniqueGem : MonoBehaviour
{
    [SerializeField] private bool random;
    [SerializeField] private GemShapeList shapeListData;
    [SerializeField] private int seed = 0;
    [SerializeField] private GameObject shape;
    [SerializeField] private Material material;
    private Renderer _renderer;

    public void Create()
    {
        if (random)
            seed = Random.Range(0, int.MaxValue);
        Random.InitState(seed);
        if (shape)
            Destroy(shape);
        shape = Instantiate(shapeListData.shapeList[Random.Range(0, shapeListData.shapeList.Count)]);
        _renderer = shape.GetComponentInChildren<Renderer>();
        _renderer.material = material;
        _renderer.material.SetColor(ShaderID.Color, Random.ColorHSV(0, 1, 0.5f, 1, 0.5f, 1));
        _renderer.material.SetFloat(ShaderID.FragResolution, Random.Range(1.0f, 3f));
        _renderer.material.SetFloat(ShaderID.FragLuminance1 , Random.Range(1.0f, 8f));
        _renderer.material.SetFloat(ShaderID.FragDensity1 , Random.Range(0f, 5f));
        _renderer.material.SetFloat(ShaderID.FragOffset1 , Random.Range(1.0f, int.MaxValue));
        _renderer.material.SetFloat(ShaderID.FragLuminance2 , Random.Range(1.0f, 8f));
        _renderer.material.SetFloat(ShaderID.FragDensity2 , Random.Range(1.0f, 6f));
        _renderer.material.SetFloat(ShaderID.FragOffset2 , Random.Range(1.0f, int.MaxValue));
        _renderer.material.SetFloat(ShaderID.VertDensity , Random.Range(3.0f, 25f));
        _renderer.material.SetFloat(ShaderID.VertStrength , Random.Range(-1.0f, 1f));
        _renderer.material.SetFloat(ShaderID.VertOffset , Random.Range(1.0f, int.MaxValue));
        
    }
    private struct ShaderID
    {
        internal static readonly int Color = Shader.PropertyToID("_Color");
        internal static readonly int FragResolution  = Shader.PropertyToID("_FragResolution");
        internal static readonly int FragLuminance1  = Shader.PropertyToID("_FragLuminance1");
        internal static readonly int FragDensity1  = Shader.PropertyToID("_FragDensity1");
        internal static readonly int FragOffset1  = Shader.PropertyToID("_FragOffset1");
        internal static readonly int FragLuminance2  = Shader.PropertyToID("_FragLuminance2");
        internal static readonly int FragDensity2  = Shader.PropertyToID("_FragDensity2");
        internal static readonly int FragOffset2  = Shader.PropertyToID("_FragOffset2");
        internal static readonly int VertDensity  = Shader.PropertyToID("_VertDensity");
        internal static readonly int VertStrength  = Shader.PropertyToID("_VertStrength");
        internal static readonly int VertOffset  = Shader.PropertyToID("_VertOffset");
    }
}
