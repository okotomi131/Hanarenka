using UnityEngine;
using System.Collections.Generic;

/*
 ===================
 §μFϋό΄
 TvFSEπΗ·ιXNvg
 ===================
 */
public class SEManager : MonoBehaviour
{
    //- ρ^θ`(SE)
    public enum E_SoundEffect
    {
        //* ΤΞΦA */
        Explosion,  // ­
        YanagiFire, // φΤΞ
        TonboFire,  // g{ΤΞ
        DragonFire, // hSΤΞ
        BarrierDes, // oAjσ
        Belt,       // ΕΏγ°
        BossBelt,   // {XΕΏγ°
        //* NbJ[ΦA */
        Brust,      // jτ
        Reservoir,  // ­ί
        Ignition,   // Ξ
        //*  ΦA */
        Generated,  // Ά¬
        Extinction, // ΑΕ
        //* V[ΦA */
        Click,      // NbN
        Select,     // {^Iπ
        Clear,      // NA
        Failure,    // Έs
        Slide,      // XCh
        //* JoΦA */
        Opening,    // Jn
        //* JbgCΦA */
        Letterapp,  // Άo»
    }

    //---------------------------------------
    //- CXyN^[Ι\¦
    [SerializeField, HideInInspector]
    public AudioClip explosion;
    [SerializeField, HideInInspector]
    public AudioClip yanagifire;
    [SerializeField, HideInInspector]
    public AudioClip tonbofire;
    [SerializeField, HideInInspector]
    public AudioClip dragonfire;
    [SerializeField, HideInInspector]
    public AudioClip barrierdes;
    [SerializeField, HideInInspector]
    public AudioClip belt;
    [SerializeField, HideInInspector]
    public AudioClip bossbelt;
    [SerializeField, HideInInspector]
    public AudioClip brust;
    [SerializeField, HideInInspector]
    public AudioClip reservoir;
    [SerializeField, HideInInspector]
    public AudioClip ignition;
    [SerializeField, HideInInspector]
    public AudioClip generated;
    [SerializeField, HideInInspector]
    public AudioClip extinction;
    [SerializeField, HideInInspector]
    public AudioClip click;
    [SerializeField, HideInInspector]
    public AudioClip select;
    [SerializeField, HideInInspector]
    public AudioClip clear;
    [SerializeField, HideInInspector]
    public AudioClip failure;
    [SerializeField, HideInInspector]
    public AudioClip slide;
    [SerializeField, HideInInspector]
    public AudioClip opening;
    [SerializeField, HideInInspector]
    public AudioClip letterapp;
    [SerializeField, HideInInspector]
    [Range(0f,1f)] public float volume;
    [SerializeField, HideInInspector]
    [Range(0f,1f)] public float pitch;
    [SerializeField, HideInInspector]
    public bool loop;
    //---------------------------------------

    //- SEManagerΜCX^XπΫ·ιΟ
    private static SEManager _instance;

    //- SEManagerΜCX^XπζΎ·ιΧΜvpeB
    public static SEManager Instance { get { return _instance; } }

    //- AudioSourceR|[lgπΫ·ιΟ
    private AudioSource _audioSource;

    //- enumΜ^ΖAAudioClipΜ}bsOπi[·ι
    private Dictionary<E_SoundEffect, AudioClip> audioClips;
    
    private void Awake()
    {
        //- ωΙCX^Xͺ ικΝA©gπjό·ι
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        //- CX^XπΫ·ι
        _instance = this;

        //- V[πJΪ΅ΔΰIuWFNgπjό΅Θ’
        DontDestroyOnLoad(this.gameObject);

        //- AudioSourceR|[lgπζΎ·ι
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        //- DictionaryΜ^πζΎ·ι
        audioClips = new Dictionary<E_SoundEffect, AudioClip>();

        //--- enumΖAudioClipπΦAt―³ΉιΧΜϊ»
        // ΤΞΦA
        FireWorksSE();
        // NbJ[ΦA
        CrackerSE();
        //  ΦA
        ResurrectionBoxSE();
        // V[ΦA
        SceneSE();
        // JoΦA
        OpeningSE();
        // JbgCΦA
        CutInSE();
    }

    private void FireWorksSE()
    {
        audioClips.Add(E_SoundEffect.Explosion, explosion);
        audioClips.Add(E_SoundEffect.YanagiFire, yanagifire);
        audioClips.Add(E_SoundEffect.TonboFire, tonbofire);
        audioClips.Add(E_SoundEffect.DragonFire, dragonfire);
        audioClips.Add(E_SoundEffect.BarrierDes, barrierdes);
        audioClips.Add(E_SoundEffect.Belt, belt);
        audioClips.Add(E_SoundEffect.BossBelt, bossbelt);
    }

    private void CrackerSE()
    {
        audioClips.Add(E_SoundEffect.Brust, brust);
        audioClips.Add(E_SoundEffect.Reservoir, reservoir);
        audioClips.Add(E_SoundEffect.Ignition, ignition);
    }

    private void ResurrectionBoxSE()
    {
        audioClips.Add(E_SoundEffect.Generated, generated);
        audioClips.Add(E_SoundEffect.Extinction, extinction);
    }

    private void SceneSE()
    {
        audioClips.Add(E_SoundEffect.Click, click);
        audioClips.Add(E_SoundEffect.Select, select);
        audioClips.Add(E_SoundEffect.Clear, clear);
        audioClips.Add(E_SoundEffect.Failure, failure);
        audioClips.Add(E_SoundEffect.Slide, slide);
    }

    private void OpeningSE()
    {
        audioClips.Add(E_SoundEffect.Opening, opening);
    }

    private void CutInSE()
    {
        audioClips.Add(E_SoundEffect.Letterapp, letterapp);
    }

    public float Pitch
    {
        get { return pitch; }
        set { pitch = value; }
    }

    /// <summary>
    /// SEπΔΆ³ΉιΦ
    /// </summary>
    /// <param name="E_SoundEffect">Ή</param>
    public void SetPlaySE(E_SoundEffect E_SoundEffect)
    {
        //- AudioClipͺΆέ΅Δ’Θ’κ
        if (!audioClips.ContainsKey(E_SoundEffect))
        {
            Debug.LogError(E_SoundEffect.ToString() + "AudioCilp not Sound");
            return;
        }

        //- ψΕn³κ½AudioClipπΔΆ·ι
        _audioSource.pitch = Pitch;
        _audioSource.PlayOneShot(audioClips[E_SoundEffect], volume);
    }

    /// <summary>
    /// SEπΔΆ³ΉιΦ(GtFNgκp)
    /// </summary>
    /// <param name="E_SoundEffect">Ή</param>
    /// <param name="Volume">ΉΚ</param>
    /// <param name="Loop">[v</param>
    public void EffectSetPlaySE(E_SoundEffect E_SoundEffect, float Volume)
    {
        //- AudioClipͺΆέ΅Δ’Θ’κ
        if (!audioClips.ContainsKey(E_SoundEffect))
        {
            Debug.LogError(E_SoundEffect.ToString() + "AudioCilp not Sound");
            return;
        }

        //- ψΕn³κ½AudioClipπΔΆ·ι
        _audioSource.PlayOneShot(audioClips[E_SoundEffect], Volume);
    }
}