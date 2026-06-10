using UnityEngine;
using TMPro;

public class OrganDescription : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public AudioSource audioSource; 

    [Header("Bahasa Indonesia")]
    [TextArea(2, 5)] public string[] teksID;
    public AudioClip[] audioID;

    [Header("English")]
    [TextArea(2, 5)] public string[] teksEN;
    public AudioClip[] audioEN;

    [Header("Français")]
    [TextArea(2, 5)] public string[] teksFR;
    public AudioClip[] audioFR;

    private int currentIndex = 0;
    public static int bahasaAktif = 0; 
    
    // VARIABEL BARU PENANDA ORGAN
    public static OrganDescription organAktifDiLayar; 

    // FUNGSI BARU PENGGANTI ONENABLE
    public void BukaOrganIni()
    {
        organAktifDiLayar = this; // Daftarkan organ ini sebagai organ yang sedang disorot
        currentIndex = 0;
        UpdateUI();
    }

    void OnEnable()
    {
        // Dikosongkan agar tidak bentrok dengan perintah Vuforia
    }

    public void NextDeskripsi()
    {
        if (currentIndex < GetCurrentTextArray().Length - 1)
        {
            currentIndex++;
            UpdateUI();
            StopAudio(); 
        }
        else {
            currentIndex = 0;
            UpdateUI();
            StopAudio();
        }
    }

    public void PrevDeskripsi()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateUI();
            StopAudio();
        }
        else {
            currentIndex = GetCurrentTextArray().Length - 1;
            UpdateUI();
            StopAudio();
        }
    }

    public void UpdateUI()
    {
        if (textUI == null) return;
        string[] teksDipilih = GetCurrentTextArray();
        
        if (teksDipilih.Length > 0 && currentIndex < teksDipilih.Length)
        {
            textUI.text = teksDipilih[currentIndex];
        }
    }

    public void MainkanAudio()
    {
        if (audioSource == null) return;
        AudioClip[] audioDipilih = GetCurrentAudioArray();
        
        if (audioDipilih.Length > 0 && currentIndex < audioDipilih.Length && audioDipilih[currentIndex] != null)
        {
            audioSource.Stop(); 
            audioSource.clip = audioDipilih[currentIndex]; 
            audioSource.Play(); 
        }
    }

    private void StopAudio()
    {
        if (audioSource != null && audioSource.isPlaying) audioSource.Stop();
    }

    private string[] GetCurrentTextArray()
    {
        switch (bahasaAktif) {
            case 1: return teksEN;
            case 2: return teksFR;
            default: return teksID;
        }
    }

    private AudioClip[] GetCurrentAudioArray()
    {
        switch (bahasaAktif) {
            case 1: return audioEN;
            case 2: return audioFR;
            default: return audioID;
        }
    }
}