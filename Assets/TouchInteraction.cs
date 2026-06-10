using UnityEngine;
using UnityEngine.InputSystem; // Wajib ditambahkan untuk memanggil sistem baru
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchInteraction : MonoBehaviour
{
    private Camera arCamera;

    void Start()
    {
        arCamera = GetComponent<Camera>();
    }

    void Update()
    {
        // Memastikan ada perangkat input penunjuk yang aktif (Mouse/Touchscreen)
        if (Pointer.current == null) return;

        // Mendeteksi apakah terjadi tekanan/sentuhan pada frame ini
        if (Pointer.current.press.wasPressedThisFrame &&
            arCamera != null &&
            Touch.activeTouches.Count <= 1)
        {
            // Mengambil koordinat X, Y dari titik yang disentuh
            Vector2 pointerPosition = Pointer.current.position.ReadValue();

            Ray ray = arCamera.ScreenPointToRay(pointerPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                string namaObjek = hit.transform.name;
                Debug.Log("Objek yang disentuh: " + namaObjek);

                // Logika eksekusi tetap sama
                if (namaObjek.Contains("Tombol_Next"))
                {
                    OrganDescription organ = hit.transform.GetComponentInParent<OrganDescription>();
                    if (organ != null) organ.NextDeskripsi();
                }
                else if (namaObjek.Contains("Tombol_Prev"))
                {
                    OrganDescription organ = hit.transform.GetComponentInParent<OrganDescription>();
                    if (organ != null) organ.PrevDeskripsi();
                }
                else if (namaObjek.Contains("Tombol_Audio"))
                {
                    OrganDescription organ = hit.transform.GetComponentInParent<OrganDescription>();
                    if (organ != null) organ.MainkanAudio();
                }
                else
                {
                    Animator anim = hit.transform.GetComponent<Animator>();
                    if (anim == null) anim = hit.transform.GetComponentInParent<Animator>();
                    if (anim != null) anim.SetTrigger("Touch");
                }
            }
        }
    }
}