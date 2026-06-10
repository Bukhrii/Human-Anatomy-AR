using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class InteractionManager : MonoBehaviour
{
    [Header("Settings Rotasi & Skala")]
    public float rotationSpeed = 0.5f;
    public float scaleSpeed = 2f;
    public float mouseScrollSpeed = 0.2f;
    public float minScale = 0.01f;
    public float maxScale = 500.0f;

    private GameObject selectedObject;
    private Camera cam;
    private bool isPinching = false;

    void OnEnable()
    {
        // Wajib dipanggil untuk mengaktifkan fitur Touch di Input System baru
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            cam = FindObjectOfType<Camera>();
        }
    }

    void Update()
    {
        if (cam == null) return;

        // --- 1. HANDLE INPUT TOUCH (UNTUK HP) ---
        if (Touch.activeTouches.Count > 0)
        {
            if (Touch.activeTouches.Count == 1)
            {
                isPinching = false;

                var touch = Touch.activeTouches[0];

                if (touch.phase == TouchPhase.Began)
                {
                    HandleSelection(touch.screenPosition);
                }
                else if (touch.phase == TouchPhase.Moved && selectedObject != null)
                {
                    float rotX = touch.delta.x * rotationSpeed;
                    float rotY = touch.delta.y * rotationSpeed;

                    selectedObject.transform.Rotate(cam.transform.up, -rotX, Space.World);
                    selectedObject.transform.Rotate(cam.transform.right, rotY, Space.World);
                }
            }
            else if (Touch.activeTouches.Count == 2 && selectedObject != null)
            {
                var touch0 = Touch.activeTouches[0];
                var touch1 = Touch.activeTouches[1];

                float currentDistance = Vector2.Distance(
                    touch0.screenPosition,
                    touch1.screenPosition);

                float previousDistance = Vector2.Distance(
                    touch0.screenPosition - touch0.delta,
                    touch1.screenPosition - touch1.delta);

                float delta = (currentDistance - previousDistance) * 15f;

                ScaleObject(delta);
            }
        }
        // --- 2. HANDLE INPUT MOUSE (UNTUK TEST DI PC / EDITOR) ---
        else
        {
            if (Mouse.current != null)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    HandleSelection(Mouse.current.position.ReadValue());
                }
                else if (Mouse.current.leftButton.isPressed && selectedObject != null)
                {
                    Vector2 deltaPos = Mouse.current.delta.ReadValue();

                    float rotX = deltaPos.x * rotationSpeed * 0.5f;
                    float rotY = deltaPos.y * rotationSpeed * 0.5f;

                    selectedObject.transform.Rotate(cam.transform.up, -rotX, Space.World);
                    selectedObject.transform.Rotate(cam.transform.right, rotY, Space.World);
                }

                Vector2 scroll = Mouse.current.scroll.ReadValue();
                if (selectedObject != null && scroll.y != 0)
                {
                    // Scroll.y biasanya nilainya besar (120 atau -120), jadi kita kecilkan
                    float normalizedScroll = Mathf.Sign(scroll.y);
                    ScaleObject(-normalizedScroll * mouseScrollSpeed);
                }
            }
        }
    }

    private void HandleSelection(Vector2 screenPosition)
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = cam.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            selectedObject = hit.transform.gameObject;

            Animator anim = selectedObject.GetComponent<Animator>();
            if (anim != null)
            {
                bool isBeating = anim.GetBool("IsBeating");
                anim.SetBool("IsBeating", !isBeating);
            }
        }
    }

    private void ScaleObject(float delta)
    {
        if (selectedObject == null) return;

        float currentScale = selectedObject.transform.localScale.x;

        currentScale += delta * 0.01f;

        currentScale = Mathf.Clamp(currentScale, minScale, maxScale);

        selectedObject.transform.localScale =
            new Vector3(currentScale, currentScale, currentScale);
    }
}
