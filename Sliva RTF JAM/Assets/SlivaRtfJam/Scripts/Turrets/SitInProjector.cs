using UnityEngine;
using Projector = SlivaRtfJam.Scripts.Projector;

public class SitInProjector : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerMovement playerMovement;
    private bool CanUseProjector => projector is not null;
    private Projector projector;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanUseProjector)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            UseProjector();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Projector newProjector))
        {
            projector = newProjector;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Projector newProjector))
        {
            projector = null;
        }
    }

    private void UseProjector()
    {
        if (projector.IsActive)
        {
            projector.Deactivate();
        }
        else
        {
            projector.Activate();
        }
    }
}