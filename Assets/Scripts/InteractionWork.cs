using UnityEngine;
using UnityEngine.UI;

public class InteractionWork : MonoBehaviour
{
    [SerializeField] private PickedPropTransform _handTransform;
    [SerializeField] private Button _dropButton;
    [SerializeField] private float _pickUpDistance = 3f;
    [SerializeField] private float _dropForce = 70f;

    public static event OnSwipeInput SwipeEvent;
    public delegate void OnSwipeInput(Vector2 derection);

    private Vector2 tapPosition;
    private Vector2 swipeDelta;

    private InteractProp _prop;
    private float deadZone = 80f;
    private bool isMobile;
    private bool isSwiping;

    private void Start()
    {
        isMobile = Application.isMobilePlatform;
        _dropButton.onClick.AddListener(DropItem);
    }

    private void Update()
    {
        Ray ray;

        if (!isMobile)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                isSwiping = true;
                tapPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (isSwiping == true)
                {
                    CheckProp(ray);
                }

                ResetSwipe();
            }
        }
        else
        {
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    isSwiping = true;
                    tapPosition = Input.mousePosition;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
                {
                    if (isSwiping == true)
                    {
                        CheckProp(ray);
                    }

                    ResetSwipe();
                }
            }
        }

        CheckSwipe();
    }

    private void CheckProp(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.TryGetComponent<InteractProp>(out InteractProp prop) && Vector3.Distance(transform.position, prop.transform.position) < _pickUpDistance)
            {
                PickUpItem(prop);
            }
        }
    }

    private void PickUpItem(InteractProp prop)
    {
        if(_prop == null)
        {
            _prop = prop;
            _prop.GetComponent<Rigidbody>().isKinematic = true;
            _prop.transform.position = _handTransform.transform.position;
            _prop.transform.SetParent(_handTransform.transform);
            _dropButton.gameObject.SetActive(true);
        }
    }

    private void DropItem()
    {
        if (_prop != null)
        {
            _prop.transform.SetParent(null);
            _prop.transform.rotation = GetComponentInParent<PlayerMovement>().transform.rotation;
            _prop.GetComponent<Rigidbody>().isKinematic = false;
            _prop.GetComponent<Rigidbody>().AddForce(_prop.transform.forward * _dropForce);
            _prop = null;
            _dropButton.gameObject.SetActive(false);
        }
    }

    private void CheckSwipe()
    {
        swipeDelta = Vector2.zero;

        if (isSwiping)
        {
            if (!isMobile && Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - tapPosition;
            }
            else if (Input.touchCount > 0)
            {
                swipeDelta = Input.GetTouch(0).position - tapPosition;
            }
        }

        if (swipeDelta.magnitude > deadZone)
        {
            if (SwipeEvent != null)
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    SwipeEvent(swipeDelta.x > 0 ? Vector2.right : Vector2.left);
                }
                else
                {
                    SwipeEvent(swipeDelta.y > 0 ? Vector2.up : Vector2.down);
                }
            }

            ResetSwipe();
        }
    }

    private void ResetSwipe()
    {
        isSwiping = false;

        tapPosition = Vector2.zero;
        swipeDelta = Vector2.zero;
    }

    private void OnDestroy()
    {
        _dropButton.onClick.RemoveAllListeners();
    }
}
