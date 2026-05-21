using UnityEngine;

public abstract class PhysicalButton : MonoBehaviour
{
    [SerializeField] private bool _hasCooldown;
    [SerializeField] private float _cooldownTime;
    private float _cooldownTimer;
    [SerializeField] private MeshRenderer _buttonMeshRenderer;
    private Material _mat;

    private void Awake()
    {
        _mat = new Material(_buttonMeshRenderer.material);
        _buttonMeshRenderer.material = _mat;

        _cooldownTimer = -1f;
        _mat.SetColor("_BaseColor", Color.softGreen);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_cooldownTimer < 0)
            {
                Activate();
                if (_hasCooldown)
                {
                    _cooldownTimer = _cooldownTime;
                }
            }
        }
    }

    protected virtual void Update()
    {
        if (_hasCooldown)
        {
            Color buttonColor = Color.Lerp(Color.softGreen, Color.softRed, (_cooldownTimer / _cooldownTime));
            _mat.SetColor("_BaseColor", buttonColor);
            _cooldownTimer -= Time.deltaTime;
        }

    }

    protected abstract void Activate();
}
