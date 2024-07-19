using DG.Tweening;
using Script.ScriptableObject;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Gem
{
    public class UniqueGem : MonoBehaviour
    {
        [SerializeField] private bool random;
        [SerializeField] private GemShapeList shapeListData;
        [SerializeField] private int seed = 0;
        [SerializeField] private GameObject shape;
        [SerializeField] private Material material;
        private Renderer _renderer;

        private void Start()
        {
            transform.DORotate(new Vector3(0.5f, 1.0f, 1.5f), 0.1f).SetLoops(-1, LoopType.Incremental);
        }

        public void Create()
        {
            if (random)
                seed = Random.Range(0, int.MaxValue);
            Random.InitState(seed);
            if (shape)
                Destroy(shape);
            shape = Instantiate(shapeListData.shapeList[Random.Range(0, shapeListData.shapeList.Count)], transform);
            _renderer = shape.GetComponentInChildren<Renderer>();
            _renderer.material = material;
            _renderer.material.SetColor(ShaderID.Color1, Random.ColorHSV(0, 1, 0.3f, 1, 0.3f, 1));
            _renderer.material.SetColor(ShaderID.Color2, Random.ColorHSV(0, 1, 0.3f, 1, 0.7f, 1));
            _renderer.material.SetFloat(ShaderID.FragResolution, Random.Range(1.0f, 3f));
            _renderer.material.SetFloat(ShaderID.FragLuminance1 , Random.Range(1.0f, 8f));
            _renderer.material.SetFloat(ShaderID.FragDensity1 , Random.Range(1f, 9f));
            _renderer.material.SetFloat(ShaderID.FragOffset1 , Random.Range(1.0f, 100));
            _renderer.material.SetFloat(ShaderID.FragLuminance2 , Random.Range(1.0f, 8f));
            _renderer.material.SetFloat(ShaderID.FragDensity2 , Random.Range(2f, 6f));
            _renderer.material.SetFloat(ShaderID.FragOffset2 , Random.Range(1.0f, 100));
            _renderer.material.SetFloat(ShaderID.VertDensity , Random.Range(3.0f, 50f));
            _renderer.material.SetFloat(ShaderID.VertStrength , Random.Range(-1.0f, 1f));
            _renderer.material.SetFloat(ShaderID.VertOffset , Random.Range(1.0f, 100));
            var scale = 1 - _renderer.material.GetFloat(ShaderID.VertStrength) / 3;
            transform.localScale = new Vector3(scale, scale, scale);
        }
        private struct ShaderID
        {
            internal static readonly int Color1 = Shader.PropertyToID("_Color1");
            internal static readonly int Color2 = Shader.PropertyToID("_Color2");
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
}
