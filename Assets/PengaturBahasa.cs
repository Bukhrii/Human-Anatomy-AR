using UnityEngine;

public class PengaturBahasa : MonoBehaviour
{
    public void GantiBahasa(int index)
    {
        // Ubah variabel statis global
        OrganDescription.bahasaAktif = index;
        
        // Hanya perbarui UI pada organ yang sedang memegang kendali layar
        if (OrganDescription.organAktifDiLayar != null)
        {
            OrganDescription.organAktifDiLayar.UpdateUI();
        }
    }
}