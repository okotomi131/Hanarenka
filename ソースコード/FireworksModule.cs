using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class FireworksModule : MonoBehaviour
{
    // 花火の種類用の列挙型
    public enum FireworksType
    {
        Normal,
        Cracker,
        Hard,
        Double,
        ResurrectionBox,
        ResurrectionPlayer,
        Boss,
        Dragonfly,
        Yanagi,
        Boss2,
        Boss3,
        Boss4,
    }

    //- 引火したもとのオブジェクトの情報
    public class CHitInfo
    {
        public Vector3 objpoint;
        public float hitcount = 0;
    }
    //- インスペクター側から非表示にする
    [SerializeField, Header("花火の種類")]
    public CHitInfo HitInfo;

    //- 共通の項目
    //-- インスペクターに表示
    [SerializeField, Header("花火の種類")]
    private FireworksType _type = FireworksType.Normal;
    [SerializeField, Header("火花用のオブジェクト")]
    private GameObject _particleObject;
    [SerializeField, HideInInspector]
    public GameObject _collisionObject; // 当たり判定用オブジェクト   通常、ハード、マルチブラスト
    [SerializeField, Header("破裂後の表情オブジェクト")]
    public GameObject _eyeObject; // 破裂後表情用オブジェクト

    //-- インスペクターから非表示
    private VibrationManager vibration; // コントローラーの振動用
    private bool _isExploded; // 爆発フラグ
    private bool _isOnce; // 一回だけフラグ
    //-- 外部からの値取得用
    public FireworksType Type => _type;
    public GameObject ParticleObject => _particleObject;
    public bool IsExploded => _isExploded;
    public GameObject CollisionObject => _collisionObject;
    public GameObject EyeObject => _eyeObject;


    //- クラッカーの項目
    //-- インスペクターに表示
    [SerializeField, HideInInspector]
    public int _circleComplementNum; // 円の分割数
    [SerializeField, HideInInspector]
    public float _blastAngle; // 判定の範囲角度(0～180度)
    [SerializeField, HideInInspector]
    public float _blastDis; // 射程
    [SerializeField, HideInInspector]
    public float _modelDeleteTime; // 破裂後モデルの残留時間
    [SerializeField, HideInInspector]
    public bool _isDrawArea = true; // 判定範囲の描画フラグ
    //-- インスペクターから非表示
    private float _destroyTime = 0.4f;    // 完全にオブジェクトを消去する時間
    private LineRenderer _linerend;       // 当たり判定表示用の線
    private ParticleSystem _particleSystem;     // パーティクルシステム
    //-- 外部からの値取得用
    public int CircleComplementNum => _circleComplementNum;
    public float BlastAngle => _blastAngle;
    public float BlastDis => _blastDis;
    public float ModelDeleteTime => _modelDeleteTime;
    public bool IsDrawArea => _isDrawArea;


    //- 二重花火の項目
    //-- インスペクターに表示
    [SerializeField, HideInInspector]
    public GameObject _multiBlast; // ２回目のエフェクト
    [SerializeField, HideInInspector]
    public float _secondAfterTime; // ２回目後の当たり判定の存続時間
    [SerializeField, HideInInspector]
    public GameObject _barrierObj; // 二重花火のバリア
    [SerializeField, HideInInspector]
    public Color _barrierColor;    // バリアの色
    [SerializeField, HideInInspector]
    public GameObject _barrierParticleObj;  // バリア破壊エフェクト
    [SerializeField, HideInInspector]
    public GameObject _parentFireObj;  // 親花火玉用のオブジェクト
    [SerializeField, HideInInspector]
    public Color _parentFireColor;     // 親花火玉の色
    [SerializeField, HideInInspector]
    public GameObject _childFireObj;   // 子花火玉用のオブジェクト
    [SerializeField, HideInInspector]
    public Color _childFireColor;      // 子花火玉の色

    //-- インスペクターから非表示
    private float _MaxInvTime; // 無敵時間用のタイムカウンタ
    //-- 外部からの値取得用
    public float SecondAfterTime => _secondAfterTime;
    public GameObject MultiBlast => _multiBlast;
    //- 色関連 - 
    public GameObject BarrierObj => _barrierObj;
    public Color BarrierColor => _barrierColor;
    public GameObject BarrierParticleObj => _barrierParticleObj;
    public GameObject ParentFireObj => _parentFireObj;
    public Color ParentFireColor => _parentFireColor;
    public GameObject ChildFireObj => _childFireObj;
    public Color ChildFireColor => _childFireColor;

    //- ハード、通常花火の項目
    [SerializeField, HideInInspector]
    public float _blastAfterTime; // 当たり判定の存続時間
    //-- インスペクターに非表示
    private float _afterTimeCount = 0; // 当たり判定のタイムカウンタ
    //-- インスペクターに表示
    public float BlastAfterTime => _blastAfterTime;


    //- ハード、二重花火の項目
    //-- インスペクターに表示
    [SerializeField, HideInInspector]
    public float _firstInvTime = 3.0f; // 爆発後無敵時間
    [SerializeField, HideInInspector]
    //-- インスペクターから非表示
    private float _invTimeCount = 0; // 無敵時間用のタイムカウンタ
    private int _blastCount = 0; // 何回爆発したか
    private Color _initColor; // マテリアルの初期の色
    private bool _isInvinsible = false; // 爆発中かどうか
    private DetonationCollision DetonationCol; // 爆発中かどうか
    //-- 外部からの値取得用
    public float FirstInvTime => _firstInvTime;


    //- ハード専用の項目
    [SerializeField, HideInInspector]
    public int _blastNum = 2;  // 何回目で爆発するか
    public int BlastNum => _blastNum;


    //- 復活箱用の項目
    //-- インスペクターに表示
    [SerializeField, HideInInspector]
    public GameObject _playerPrefab; // 生成するオブジェクト(プレイヤー)
    [SerializeField, HideInInspector]
    public float _delayTime = 0.1f; // 生成までの待ち時間(秒)
    [SerializeField, HideInInspector]
    public float _animationTime = 0.1f; // アニメーション時間(秒)
    [SerializeField, HideInInspector]
    public float _animationDelayTime = 0.1f; // アニメーションの遅延時間(秒)
    [SerializeField, HideInInspector]
    public float _boxDisTime = 0.1f; // 箱の消滅時間(秒)
    //-- インスペクターから非表示
    SceneChange sceneChange;
    //-- 外部からの値取得用
    public GameObject PlayerPrefab => _playerPrefab;
    public float DelayTime => _delayTime;
    public float AnimationTime => _animationTime;
    public float AnimationDelayTime => _animationDelayTime;
    public float BoxDisTime => _boxDisTime;


    //- 復活花火の項目
    //-- インスペクターに表示
    [SerializeField, HideInInspector]
    public float _invTime; // 無敵時間
    //-- インスペクターから非表示
    private float _currentTime = 0.0f;
    private bool _isInv = true; // 無敵時間
    //-- 外部からの値取得用
    public float InvTime => _invTime;


    //- ぬし花火用の項目
    //-- インスペクターに表示
    [SerializeField, HideInInspector]
    public int _ignitionMax = 3;  // 何回目で爆発するか
    [SerializeField, HideInInspector]
    public GameObject _outsideBarrier; // 外側のバリア
    [SerializeField, HideInInspector]
    public Color _outsideBarrierColor; // 外側のバリアの色
    [SerializeField, HideInInspector]
    public GameObject _outsideBarrierParticleObj;  // 外側のバリア破壊エフェクト
    [SerializeField, HideInInspector]
    public GameObject _insideBarrier;  // 内側のバリア
    [SerializeField, HideInInspector]
    public Color _insideBarrierColor;  // 内側のバリアの色
    [SerializeField, HideInInspector]
    public GameObject _insideBarrierParticleObj;  // 内側のバリア破壊エフェクト
    [SerializeField, HideInInspector]
    public GameObject _boss1Obj1; // 変化させるボスオブジェクト1
    [SerializeField, HideInInspector]
    public GameObject _boss1Obj2; // 変化させるボスオブジェクト2
    //-- インスペクターから非表示
    private int ignitionCount = 0; // 何回引火したか
    private float moveTimeCount = 0; // ぬし花火用の挙動用の変数
    //-- 外部からの値取得用
    public int IgnitionMax => _ignitionMax;
    public GameObject OutsideBarrier => _outsideBarrier;
    public Color OutsideBarrierColor => _outsideBarrierColor;
    public GameObject OutsideBarrierParticleObj => _outsideBarrierParticleObj;
    public GameObject InsideBarrier => _insideBarrier;
    public Color InsideBarrierColor => _insideBarrierColor;
    public GameObject InsideBarrierParticleObj => _insideBarrierParticleObj;
    public GameObject Boss1Obj1 => _boss1Obj1;
    public GameObject Boss1Obj2 => _boss1Obj2;

    //- トンボ花火用の項目
    //-- インスペクターに表示
    [SerializeField, HideInInspector] //- 速度
    public float _tonboSpeed;
    [SerializeField, HideInInspector]
    public GameObject _effectTonbo; // 柳花火用のオブジェクト
    //-- インスペクターに非表示
    private bool bIsInit = false; //- 最初だけ処理を実行するための判定フラグ
    [SerializeField, HideInInspector] //- 減速時間
    public Vector2 movedir;              //- トンボ花火が移動する方向
    float CountTime = 0; //- 時間のカウンタ
    //-- 外部からの値取得用
    public float TonboSpeed => _tonboSpeed;
    public GameObject EffectTonbo => _effectTonbo;

    //- 柳花火の項目
    //- インスペクターに表示
    [SerializeField, HideInInspector]
    public float _modelResidueTime; // 柳花火の残留秒数
    [SerializeField, HideInInspector]
    public GameObject _yanagiobj; // 柳花火用のオブジェクト
    [SerializeField, HideInInspector]
    public Color _yanagiColor;    // 柳花火の色
    [SerializeField, HideInInspector]
    public GameObject _reafobj1;  // 葉っぱ用のオブジェクト1
    [SerializeField, HideInInspector]
    public Color _reafColor1;     // 葉っぱの色1
    [SerializeField, HideInInspector]
    public GameObject _reafobj2;  // 葉っぱ用のオブジェクト2
    [SerializeField, HideInInspector]
    public Color _reafColor2;     // 葉っぱの色2
    //- 外部からの値取得用
    public float ModelResidueTime => _modelResidueTime;
    public GameObject YanagiObj => _yanagiobj;
    public Color YanagiColor => _yanagiColor;
    public GameObject ReafObj1 => _reafobj1;
    public Color ReafColor1 => _reafColor1;
    public GameObject ReafObj2 => _reafobj2;
    public Color ReafColor2 => _reafColor2;



    //- ぬし花火用関連の共通項目
    [SerializeField, HideInInspector]
    public GameObject _movieObject; // 演出を管理しているオブジェクト
    [SerializeField, HideInInspector]
    public Color _bossIgniteColor; // 引火時のカラー
    [SerializeField, HideInInspector]
    public float _fadeTime = 0.5f; // 色のフェード時間(秒)
    //- 外部からの値取得用
    public GameObject MovieObject => _movieObject;
    public Color BossIgniteColor => _bossIgniteColor;
    public float FadeTime => _fadeTime;
    //- インスペクターに非表示
    private int enemyNum;   // 花火玉の残存数
    CountEnemy countEnemy;  // 花火玉の残存取得用スクリプト


    //- 2面ぬし花火の項目
    //- インスペクターに表示
    [SerializeField, HideInInspector]
    public float _synchroTime; // 猶予時間
    [SerializeField, HideInInspector]
    public GameObject _boss2barrierObj; // バリアオブジェクト
    [SerializeField, HideInInspector]
    public Color _boss2barrierColor; // バリアカラー
    [SerializeField, HideInInspector]
    public GameObject _boss2barrierParticleObj;  // 外側のバリア破壊エフェクト
    [SerializeField, HideInInspector]
    public GameObject _boss2Obj1; // 変化させるボスオブジェクト1
    [SerializeField, HideInInspector]
    public GameObject _boss2Obj2; // 変化させるボスオブジェクト2
    //-- インスペクターに非表示
    private float TimeCount; //- タイムカウンタ
    private bool bStartMovie; //- 演出が始まったかどうか
    //- 外部からの値取得用
    public float SynchroTime => _synchroTime;
    public GameObject Boss2BarrierObj => _boss2barrierObj;
    public Color Boss2BarrierColor => _boss2barrierColor;
    public GameObject Boss2BarrierParticleObj => _boss2barrierParticleObj;
    public GameObject Boss2Obj1 => _boss2Obj1;
    public GameObject Boss2Obj2 => _boss2Obj2;

    //- 3面ぬし花火の項目
    //- インスペクターに表示
    [SerializeField, HideInInspector]
    public GameObject _boss3obj; // 変化させるゲームオブジェクト
    //-- インスペクターに非表示
    private static int igniteCount = 0;
    private bool bAnime;
    //- 外部からの値取得用
    public GameObject Boss3Obj => _boss3obj;

    //- 4面ぬし花火の項目
    //- インスペクターに表示
    [SerializeField, HideInInspector]
    public GameObject _motionObject; // モーション再生判定フラグ
    [SerializeField, HideInInspector]
    public GameObject _materialObject; // マテリアルがあるオブジェクト

    //- 外部からの値取得用
    public GameObject MotionObject => _motionObject;
    public GameObject MaterialObject => _materialObject;

    public EntryAnime InGR;
    public EntryAnime InGS;
    public EntryAnime Tips;

    private DragonflyRayCheck dragonflyRayCheck;
    // Start is called before the first frame update
    void Start()
    {
        //- 共通項目
        vibration = GameObject.Find("VibrationManager").GetComponent<VibrationManager>();
        _isExploded = false;
        _isOnce = false;
        HitInfo = new CHitInfo();

        //- クラッカーの項目
        _linerend = gameObject.AddComponent<LineRenderer>(); // 線の追加
        vibration = GameObject.Find("VibrationManager").GetComponent<VibrationManager>(); // 振動コンポーネントの取得
        if (_type == FireworksType.Cracker)
        {
            _particleSystem = ParticleObject.transform.GetChild(0).GetComponent<ParticleSystem>(); // パーティクルの取得
        }
        //- ハードの項目
        if (_type == FireworksType.Normal)
        {
            DetonationCol = _collisionObject.GetComponent<DetonationCollision>();
        }

        //- ハードの項目
        if (_type == FireworksType.Hard)
        {
            DetonationCol = _collisionObject.GetComponent<DetonationCollision>();
        }

        //- 二重花火の項目
        if (_type == FireworksType.Double)
        {
            DetonationCol = _collisionObject.GetComponent<DetonationCollision>();
            DetonationCol.IsDoubleBlast = true;
            _barrierObj.GetComponent<Renderer>().material.color = _barrierColor;
            _parentFireObj.GetComponent<Renderer>().material.color = _parentFireColor;
            _childFireObj.GetComponent<Renderer>().material.color = _childFireColor;
        }

        //- 柳花火の項目
        if (_type == FireworksType.Yanagi)
        {
            _yanagiobj.GetComponent<Renderer>().material.color = _yanagiColor;
            _reafobj1.GetComponent<Renderer>().material.color = _reafColor1;
            _reafobj2.GetComponent<Renderer>().material.color = _reafColor2;
        }

        //- 復活箱の項目
        sceneChange = GameObject.Find("Main Camera").GetComponent<SceneChange>();
        countEnemy = GameObject.Find("Main Camera").GetComponent<CountEnemy>();

        //- バリアの項目
        if (_type == FireworksType.Boss)
        {
            _outsideBarrier.GetComponent<Renderer>().material.color = _outsideBarrierColor;
            _insideBarrier.GetComponent<Renderer>().material.color = _insideBarrierColor;
        }

        //- 2面ぬし花火の項目
        if (_type == FireworksType.Boss2)
        {
            _boss2barrierObj.GetComponent<Renderer>().material.color = _boss2barrierColor;
        }

        //- 3面ぬし花火の項目
        igniteCount = 0;

        //- 4面ぬし花火の項目
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isInv && _type == FireworksType.ResurrectionPlayer)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _invTime)
            { _isInv = false; }
        }

        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    _isExploded = true;
        //}

        if (IsExploded)
        { // 爆発した後
            switch (Type)
            {
                case FireworksType.Normal:
                    NormalFire();
                    break;
                case FireworksType.Cracker:
                    CrackerFire();
                    break;
                case FireworksType.Hard:
                    HardFire();
                    break;
                case FireworksType.Double:
                    DoubleFire();
                    break;
                case FireworksType.ResurrectionBox:
                    ResurrectionBoxFire();
                    break;
                case FireworksType.ResurrectionPlayer:
                    ResurrectionPlayerFire();
                    break;
                case FireworksType.Dragonfly:
                    DragonflyFire();
                    break;
                case FireworksType.Yanagi:
                    YanagiFire();
                    break;
                case FireworksType.Boss2:
                    Boss2Fire();
                    break;
                case FireworksType.Boss3:
                    Boss3Fire();
                    break;
                case FireworksType.Boss4:
                    Boss4Fire();
                    break;
                default:
                    break;
            }
        }
        
        if (Type == FireworksType.Cracker)
        {
            if (!Input.GetKey(KeyCode.Alpha1)) return;
            // ===== 当たり判定の表示 =====
            Vector3 pos = this.transform.position;
            Vector3 dir = Vector3.up * _blastDis;
            dir = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * dir;
            dir = Quaternion.Euler(0, 0, _blastAngle / 2) * dir;
            Debug.DrawRay(pos, dir, Color.red, 2.0f);
            float num = 10;
            Vector3 end = pos;
            for (int i = 0; i < num; i++)
            {
                Vector3 start = pos + dir;
                dir = Quaternion.Euler(0, 0, -_blastAngle / num) * dir;
                end = pos + dir;
                Debug.DrawRay(start, end - start, Color.red, 2.0f);
            }
            pos = this.transform.position;
            dir = Vector3.up * _blastDis;
            dir = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * dir;
            dir = Quaternion.Euler(0, 0, -_blastAngle / 2) * dir;
            dir = Vector3.up * _blastDis;
            Debug.DrawRay(end, pos - end, Color.red, 2.0f);
            // ============================
        }
    }

    // 爆発時に子オブジェクト含め描画をやめる処理
    void StopRenderer(GameObject gameObject)
    {
        var renderer = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < renderer.Length; i++)
        {
            renderer[i].enabled = false;
        }
    }

    // 爆発フラグを立てる処理
    public void Ignition(Vector3 _objpoint)
    {
        _isExploded = true;
        HitInfo.objpoint = _objpoint; //- 当たった元のオブジェクトの座標を格納
        HitInfo.hitcount++; //- 当たった回数を更新
    }

    public bool GetIsInv()
    {
        return _isInv;
    }

    // ぬし花火用の引火処理
    public void IgnitionBoss(GameObject obj)
    {
        //- 引火回数を増やす
        ignitionCount++;

        //- 1回目の引火時、外側のバリア破壊
        if (ignitionCount == 1)
        {
            //- バリアオブジェクト破壊
            Destroy(_outsideBarrier);
            //- バリア破壊エフェクト生成
            SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.BarrierDes); // バリア破壊音
            GameObject barrier = Instantiate(
                _outsideBarrierParticleObj,
                new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z),
                Quaternion.Euler(0.0f, 0.0f, 0.0f)
                );
        }
        //- 2回目の引火時、内側のバリア破壊      
        if (ignitionCount == 2)
        {
            //- バリアオブジェクト破壊
            Destroy(_insideBarrier);
            //- バリア破壊エフェクト生成
            SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.BarrierDes); // バリア破壊音
            GameObject barrier = Instantiate(
                _insideBarrierParticleObj,
                new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z),
                Quaternion.Euler(0.0f, 0.0f, 0.0f)
                );
        }

        if (ignitionCount < _ignitionMax) return; // 引火回数が必要回数に満たなければリターン
        _isExploded = true; //- 爆発フラグ

        //- マテリアルの取得
        Material material = _boss1Obj1.GetComponent<Renderer>().material;
        Material material2 = _boss1Obj2.GetComponent<Renderer>().material;
        //- 引火時のフェード処理
        material.DOColor(BossIgniteColor, FadeTime);
        material2.DOColor(BossIgniteColor, FadeTime);

        GameObject.Find("InGameSelect").GetComponent<EntryAnime>().OutMove();
        GameObject.Find("InGameReset").GetComponent<EntryAnime>().OutMove();
        GameObject.Find("InGameTips").GetComponent<EntryAnime>().OutMove();
        //InGS.OutMove();
        //InGR.OutMove();
        //Tips.OutMove();

        SceneChange scenechange = GameObject.Find("Main Camera").GetComponent<SceneChange>();
        scenechange.RequestStopClear(true);
        scenechange.RequestStopMiss(true);
        //- アニメーション処理
        transform.DOMoveY(-15, 1.5f).SetEase(Ease.OutSine).SetLink(gameObject);
        transform.DOMoveY(20, 0.7f).SetEase(Ease.OutSine).SetDelay(1.5f).SetLink(gameObject);
        DOTween.Sequence().SetDelay(1.5f).OnComplete(() =>
        { SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.BossBelt); }); // クリア演出画面でのボス打ち上げ音
        //- 演出用スクリプトの取得
        MovieManager movie = MovieObject.GetComponent<MovieManager>();
        //- 演出フラグ変更
        movie.SetMovieFlag(true);
        //- 演出開始
        DOVirtual.DelayedCall(2.1f, () => movie.StartVillageMovie(), false);
        //- 破壊処理
        Destroy(gameObject, 2.2f);
    }

    private void NormalFire()
    {
        if (!_isOnce)
        { // 爆発直後一回のみ
            _isOnce = true;
            //ShakeByPerlinNoise shakeByPerlinNoise;
            //shakeByPerlinNoise = GameObject.FindWithTag("MainCamera").GetComponent<ShakeByPerlinNoise>();
            //var duration = 0.2f;
            //var strength = 0.1f;
            //var vibrato = 1.0f;
            //- 指定した位置に生成
            GameObject fire = Instantiate(
                ParticleObject,                     // 生成(コピー)する対象
                transform.position,           // 生成される位置
                Quaternion.Euler(0.0f, 0.0f, 0.0f)  // 最初にどれだけ回転するか
                );

            //- コントローラーの振動の設定
            vibration.SetVibration(30, 1.0f);

            //- 当たり判定を有効化する
            // 当たったオブジェクトのColliderを有効にする
            CollisionObject.gameObject.GetComponent<Collider>().enabled = true;
            // 当たり判定の拡大用コンポーネントを有効にする
            DetonationCol.enabled = true;

            //- 爆発時に描画をやめる
            StopRenderer(gameObject);

            //- 爆発音の再生
            SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Explosion);
        }

        _afterTimeCount += Time.deltaTime;
        //- 当たり判定を消す処理
        if (_afterTimeCount >= _blastAfterTime)
        {
            DetonationCol.EndDetonation(); //- 当たり判定の消滅
        }
    }

    private void ResurrectionPlayerFire()
    {
        if (!_isOnce)
        { // 爆発直後一回のみ
            _isOnce = true;
            //- 指定した位置に生成
            GameObject fire = Instantiate(
                ParticleObject,                     // 生成(コピー)する対象
                transform.position,           // 生成される位置
                Quaternion.Euler(0.0f, 0.0f, 0.0f)  // 最初にどれだけ回転するか
                );

            //- SceneChangeスクリプトのプレイヤー生存フラグをfalseにする
            sceneChange.bIsLife = false;

            //- コントローラーの振動の設定
            vibration.SetVibration(30, 1.0f);

            //- 当たり判定を有効化する
            // 当たったオブジェクトのColliderを有効にする
            CollisionObject.gameObject.GetComponent<Collider>().enabled = true;
            // 当たり判定の拡大用コンポーネントを有効にする
            CollisionObject.gameObject.GetComponent<DetonationCollision>().enabled = true;

            //- 爆発時に描画をやめる
            StopRenderer(gameObject);

            //- 爆発音の再生
            SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Explosion);

            Destroy(gameObject, 0.5f);
        }
    }

    private void CrackerFire()
    {
        //- 弾けるタイミングになるまでは、以下の爆破処理を行わない
        //if (!_isExploded) return;

        if (!_isOnce)
        { // 爆破直後一回のみ

            //- タグの変更(残り花火数のタグ検索を回避するため)
            this.tag = "Untagged";
            //- 爆発処理フラグを変更
            _isOnce = true;
            //- 引火前のモデルを非アクティブ化
            transform.GetChild(0).gameObject.SetActive(false);
            //- アニメーション用のモデルをアクティブ化
            transform.GetChild(1).gameObject.SetActive(true);
            //- 一定時間後に発火する
            StartCoroutine(DelayCracker(0.4f));
            //- 着火音再生
            SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Ignition);
            //- クラッカー溜め音再生
            SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Reservoir);
            //- 一定時間後にアニメーション用を非アクティブ化
            StartCoroutine(DelaySetActive(transform.GetChild(1).gameObject, false, 0.8f));
            //- 破裂後モデルをアクティブ化
            StartCoroutine(DelaySetActive(transform.GetChild(2).gameObject, true, 0.8f));
            //- 一定時間後に破裂後モデルを非アクティブ化
            //StartCoroutine(DelaySetActive(transform.GetChild(2).gameObject, false, 0.8f + ModelDeleteTime));
        }
    }

    private void YanagiFire()
    {
        if (!_isOnce)
        { // 爆発直後一回のみ
            _isOnce = true;

            StartCoroutine(MakeYanagiEffect(0.1f, 120));

            //- コントローラーの振動の設定
            vibration.SetVibration(30, 1.0f);

            //- 爆発音の再生
            SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Explosion);

            //- タグの変更(残り花火数のタグ検索を回避するため)
            this.tag = "Untagged";

            //- 失敗判定にならないように設定、花火が消えきったら失敗判定を復活
            SceneChange scenechange = GameObject.Find("Main Camera").GetComponent<SceneChange>();
            scenechange.RequestStopMiss(true);
            DOVirtual.DelayedCall(15.0f, () => scenechange.RequestStopMiss(false));
            //- 爆発後に削除
            DOVirtual.DelayedCall(_modelResidueTime, () => transform.GetChild(0).gameObject.SetActive(false));

            
        }
    }

    private IEnumerator MakeYanagiEffect(float delayTime, int maxEffect)
    {
        //- エフェクトを生成
        for (int i = 0; i < maxEffect; i++)
        {
            //- delayTime秒待機する
            yield return new WaitForSeconds(delayTime);
            SEManager.Instance.EffectSetPlaySE(SEManager.E_SoundEffect.YanagiFire, 0.05f);// 柳のエフェクト音再生
            //- エフェクト生成のために、座標を取得
            Vector3 pos = transform.position;
            //- 生成位置をずらす
            pos.y += 1.6f;
            //- 指定した位置に生成
            GameObject fire = Instantiate(_particleObject, pos, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        }
    }

    private IEnumerator DelayCracker(float delayTime)
    {
        //- delayTime秒待機する
        yield return new WaitForSeconds(delayTime);
        //- クラッカーのエフェクト生成
        GameObject fire = Instantiate(
            _particleObject,                     // 生成(コピー)する対象
            transform.position,           // 生成される位置
            Quaternion.Euler(0.0f, 0.0f, transform.localEulerAngles.z)  // 最初にどれだけ回転するか
            );



        //- 振動の設定
        vibration.SetVibration(30, 1.0f);
        //- 破裂音の再生
        SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Brust);
        //- タグが花火のオブジェクトを全て取得
        GameObject[] Fireworks = GameObject.FindGameObjectsWithTag("Fireworks");
        //- レイの開始点のオブジェクト
        GameObject originObj = this.transform.GetChild(3).gameObject;
        //- 範囲検索用のベクトル開始地点
        Vector3 RangeStartPos = this.transform.position;
        //- レイの開始点
        Vector3 RayStartPos = new Vector3(originObj.transform.position.x, originObj.transform.position.y, originObj.transform.position.z);
        //- 花火のオブジェクトを一つずつ実行
        foreach (var obj in Fireworks)
        {
            //- レイの目標点
            Vector3 targetPos = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
            //- クラッカーから花火へのベクトル(範囲用)
            Vector3 RangeDir = targetPos - transform.position;
            //- クラッカーから花火へのベクトル(レイ用)
            Vector3 RayDir = targetPos - RayStartPos;
            //- 花火との距離を取得
            float dis = Vector3.Distance(RangeStartPos, targetPos);
            //- 花火との距離が射程内じゃなかったら処理しない
            if (dis > BlastDis) continue;

            //- 変数の準備
            float DisDelayRatio;
            float DelayTime;
            //- 「花火へのベクトル」と「クラッカーの向きベクトル」の角度を求める
            var angle = Vector3.Angle((transform.up).normalized, (RangeDir).normalized);
            if (/*angle != 0 && */(angle <= BlastAngle / 2))
            {
                DisDelayRatio = (dis) / (BlastDis * _particleSystem.main.startSpeed.constantMin / 25) / 2.8f;
                DelayTime = (10 / _particleSystem.main.startSpeed.constantMin / 25) + DisDelayRatio;
            }
            else
            {
                continue;
            }

            // 自身から花火に向かうレイを作成
            Ray ray = new Ray(RayStartPos, RayDir);
            // 当たったオブジェクトを格納するための変数
            var HitList = new List<RaycastHit>();
            // レイが当たったオブジェクトをすべて順番に確認していく
            foreach (RaycastHit hit in Physics.RaycastAll(ray, dis))
            {
                //- 最初のオブジェクトなら無条件で格納
                if (HitList.Count == 0)
                {
                    HitList.Add(hit);
                    continue;
                }

                //- 格納フラグ
                bool bAdd = false;
                //- 格納変数と当たったオブジェクトの比較
                for (int i = 0; i < HitList.Count; i++)
                {
                    //- 格納フラグチェック
                    if (bAdd) break;
                    //- 距離が格納箇所データの距離より長ければリターン
                    if (HitList[i].distance < hit.distance) continue;
                    //- 仮のデータを一番最後に格納
                    HitList.Add(new RaycastHit());
                    //- 最後から格納場所までデータをずらす
                    for (int j = HitList.Count - 1; j > i; j--)
                    {
                        //- データを一つ移動
                        HitList[j] = HitList[j - 1];
                    }
                    //- 格納場所に格納
                    HitList[i] = hit;
                    bAdd = true;
                }

                //- 格納フラグが立っていなければ、一番距離が長いオブジェクトなので
                //- 配列の一番最後に格納する
                if (!bAdd) HitList.Add(hit);
            }

            //- 爆発フラグ
            bool bBlast = false;
            //- 距離が短いものから調べる
            for (int i = 0; i < HitList.Count; i++)
            {
                RaycastHit hit = HitList[i];

                //- 当たり判定のデバッグ表示
                if (Input.GetKey(KeyCode.Alpha1))
                {
                    float markdis = 0.1f;
                    Debug.DrawRay(RayStartPos, RayDir, Color.red, 3.0f);
                    Debug.DrawRay(hit.point, new Vector3(+markdis, +markdis, 0), Color.blue, 3.0f);
                    Debug.DrawRay(hit.point, new Vector3(+markdis, -markdis, 0), Color.blue, 3.0f);
                    Debug.DrawRay(hit.point, new Vector3(-markdis, +markdis, 0), Color.blue, 3.0f);
                    Debug.DrawRay(hit.point, new Vector3(-markdis, -markdis, 0), Color.blue, 3.0f);
                }
                if (hit.collider.gameObject.tag != "Stage" &&
                    hit.collider.gameObject.tag != "Warphole") continue; //- ステージオブジェクト以外なら次へ
                if (hit.distance > dis) continue;               //- 花火玉よりステージオブジェクトが奥にあれば次へ

                //- 当たった花火玉より手前にステージオブジェクトが存在する
                bBlast = true; //- フラグ変更
            }

            //- 遅延をかけて爆破
            if (!bBlast) StartCoroutine(DelayDestroyCracker(obj, DelayTime,transform.position));
        }

        // ===== ワープホールチェック ======
        //- タグがワープホールのオブジェクトを全て取得
        GameObject[] Warpholes = GameObject.FindGameObjectsWithTag("Warphole");
        //- レイの開始点のオブジェクト
        originObj = this.transform.GetChild(3).gameObject;
        //- 範囲検索用のベクトル開始地点
        RangeStartPos = this.transform.position;
        //- レイの開始点
        RayStartPos = new Vector3(originObj.transform.position.x, originObj.transform.position.y, originObj.transform.position.z);
        bool IsHitWarphole = false;
        GameObject warpA = GameObject.Find("WarpholeA");
        GameObject warpB = GameObject.Find("WarpholeB");
        //- ワープホールオブジェクトを一つずつ実行
        foreach (var obj in Warpholes)
        {
            //- レイの目標点
            Vector3 targetPos = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
            //- クラッカーからワープホールへのベクトル(範囲用)
            Vector3 RangeDir = targetPos - transform.position;
            //- クラッカーから花火へのベクトル(レイ用)
            Vector3 RayDir = targetPos - RayStartPos;
            //- 花火との距離を取得
            float dis = Vector3.Distance(RangeStartPos, targetPos);
            //- 花火との距離が射程内じゃなかったら処理しない
            if (dis > BlastDis) continue;

            //- 変数の準備
            float DisDelayRatio;
            float DelayTime;
            //- 「花火へのベクトル」と「クラッカーの向きベクトル」の角度を求める
            var angle = Vector3.Angle((transform.up).normalized, (RangeDir).normalized);
            if (/*angle != 0 && */(angle <= BlastAngle / 2))
            {
                DisDelayRatio = (dis) / (BlastDis * _particleSystem.main.startSpeed.constantMin / 25) / 2.8f;
                DelayTime = (10 / _particleSystem.main.startSpeed.constantMin / 25) + DisDelayRatio;
            }
            else
            {
                continue;
            }

            // 自身から花火に向かうレイを作成
            Ray ray = new Ray(RayStartPos, RayDir);
            // 当たったオブジェクトを格納するための変数
            var HitList = new List<RaycastHit>();
            // レイが当たったオブジェクトをすべて順番に確認していく
            foreach (RaycastHit hit in Physics.RaycastAll(ray, dis))
            {
                //- 最初のオブジェクトなら無条件で格納
                if (HitList.Count == 0)
                {
                    HitList.Add(hit);
                    continue;
                }

                //- 格納フラグ
                bool bAdd = false;
                //- 格納変数と当たったオブジェクトの比較
                for (int i = 0; i < HitList.Count; i++)
                {
                    //- 格納フラグチェック
                    if (bAdd) break;
                    //- 距離が格納箇所データの距離より長ければリターン
                    if (HitList[i].distance < hit.distance) continue;
                    //- 仮のデータを一番最後に格納
                    HitList.Add(new RaycastHit());
                    //- 最後から格納場所までデータをずらす
                    for (int j = HitList.Count - 1; j > i; j--)
                    {
                        //- データを一つ移動
                        HitList[j] = HitList[j - 1];
                    }
                    //- 格納場所に格納
                    HitList[i] = hit;
                    bAdd = true;
                }

                //- 格納フラグが立っていなければ、一番距離が長いオブジェクトなので
                //- 配列の一番最後に格納する
                if (!bAdd) HitList.Add(hit);
            }

            //- 爆発フラグ
            bool bBlock = false;
            //- 距離が短いものから調べる
            for (int i = 0; i < HitList.Count; i++)
            {
                RaycastHit hit = HitList[i];

                //- 当たり判定のデバッグ表示
                if (Input.GetKey(KeyCode.Alpha1))
                {
                    float markdis = 0.1f;
                    Debug.DrawRay(RayStartPos, RayDir, Color.red, 3.0f);
                    Debug.DrawRay(hit.point, new Vector3(+markdis, +markdis, 0), Color.blue, 3.0f);
                    Debug.DrawRay(hit.point, new Vector3(+markdis, -markdis, 0), Color.blue, 3.0f);
                    Debug.DrawRay(hit.point, new Vector3(-markdis, +markdis, 0), Color.blue, 3.0f);
                    Debug.DrawRay(hit.point, new Vector3(-markdis, -markdis, 0), Color.blue, 3.0f);
                }
                if (hit.collider.gameObject.tag != "Stage") continue; //- ステージオブジェクト以外なら次へ
                if (hit.distance > dis) continue;               //- 花火玉よりステージオブジェクトが奥にあれば次へ

                //- 当たった花火玉より手前にステージオブジェクトが存在する
                bBlock = true;
            }

            if (obj.name == "WarpholeA") WarpholeCracker(warpA, warpB);
            if (obj.name == "WarpholeB") WarpholeCracker(warpB, warpA);
        }


        //- レイヤーの変更
        gameObject.layer = 0;
        //- 自身を破壊する
        Destroy(this.gameObject, _destroyTime);
    }

    private void WarpholeCracker(GameObject Warp1, GameObject Warp2)
    {
        // ===== 当たり判定の表示 =====
        Vector3 Dis = transform.position - Warp1.transform.position;
        Vector3 pos = Warp2.transform.position + Dis;
        Vector3 dir = Vector3.up * _blastDis;
        dir = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * dir;
        dir = Quaternion.Euler(0, 0, _blastAngle / 2) * dir;
        Debug.DrawRay(pos, dir, Color.blue, 2.0f);
        float num = 10;
        Vector3 end = pos;
        for (int i = 0; i < num; i++)
        {
            Vector3 start = pos + dir;
            dir = Quaternion.Euler(0, 0, -_blastAngle / num) * dir;
            end = pos + dir;
            Debug.DrawRay(start, end - start, Color.blue, 2.0f);
        }
        Debug.Log(pos);
        pos = Warp2.transform.position + Dis;
        dir = Vector3.up * _blastDis;
        dir = Quaternion.Euler(0, 0, transform.localEulerAngles.z) * dir;
        dir = Quaternion.Euler(0, 0, -_blastAngle / 2) * dir;
        dir = Vector3.up * _blastDis;
        Debug.DrawRay(end, pos - end, Color.blue, 2.0f);
        // ============================ 
        Vector3 WarpDis = Warp2.transform.position - Warp1.transform.position;
        //- タグが花火のオブジェクトを全て取得
        GameObject[] Fireworks = GameObject.FindGameObjectsWithTag("Fireworks");
        //- レイの開始点のオブジェクト
        GameObject originObj = this.transform.GetChild(3).gameObject;
        //- 範囲検索用のベクトル開始地点
        Vector3 RangeStartPos = this.transform.position;
        RangeStartPos += WarpDis;
        //- レイの開始点
        Vector3 RayStartPos = new Vector3(originObj.transform.position.x, originObj.transform.position.y, originObj.transform.position.z);
        RayStartPos += WarpDis;


        //- 花火のオブジェクトを一つずつ実行
        foreach (var obj in Fireworks)
        {
            //- レイの目標点
            Vector3 targetPos = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
            //- クラッカーから花火へのベクトル(範囲用)
            Vector3 RangeDir = targetPos - (transform.position + WarpDis);
            //- クラッカーから花火へのベクトル(レイ用)
            Vector3 RayDir = targetPos - RayStartPos;
            //- 花火との距離を取得
            float dis = Vector3.Distance(RangeStartPos, targetPos);
            //- 花火との距離が射程内じゃなかったら処理しない
            if (dis > BlastDis) continue;

            //- 変数の準備
            float DisDelayRatio;
            float DelayTime;
            //- 「花火へのベクトル」と「クラッカーの向きベクトル」の角度を求める
            var angle = Vector3.Angle((transform.up).normalized, (RangeDir).normalized);
            if (/*angle != 0 && */(angle <= BlastAngle / 2))
            {
                DisDelayRatio = (dis) / (BlastDis * _particleSystem.main.startSpeed.constantMin / 25) / 2.8f;
                DelayTime = (10 / _particleSystem.main.startSpeed.constantMin / 25) + DisDelayRatio;
            }
            else
            {
                continue;
            }
            Debug.DrawRay(RayStartPos, RayDir, Color.green, 2.0f);

            // 自身から花火に向かうレイを作成
            Ray ray = new Ray(RayStartPos, RayDir);
            // 当たったオブジェクトを格納するための変数
            var HitList = new List<RaycastHit>();
            // レイが当たったオブジェクトをすべて順番に確認していく
            foreach (RaycastHit hit in Physics.RaycastAll(ray, BlastDis * 2))
            {
                //- 最初のオブジェクトなら無条件で格納
                if (HitList.Count == 0)
                {
                    HitList.Add(hit);
                    continue;
                }

                //- 格納フラグ
                bool bAdd = false;
                //- 格納変数と当たったオブジェクトの比較
                for (int i = 0; i < HitList.Count; i++)
                {
                    //- 格納フラグチェック
                    if (bAdd) break;
                    //- 距離が格納箇所データの距離より長ければリターン
                    if (HitList[i].distance < hit.distance) continue;
                    //- 仮のデータを一番最後に格納
                    HitList.Add(new RaycastHit());
                    //- 最後から格納場所までデータをずらす
                    for (int j = HitList.Count - 1; j > i; j--)
                    {
                        //- データを一つ移動
                        HitList[j] = HitList[j - 1];
                    }
                    //- 格納場所に格納
                    HitList[i] = hit;
                    bAdd = true;
                }

                //- 格納フラグが立っていなければ、一番距離が長いオブジェクトなので
                //- 配列の一番最後に格納する
                if (!bAdd) HitList.Add(hit);
            }

            //- 爆発フラグ
            bool bBlast = false;
            //- 距離が短いものから調べる
            for (int i = 0; i < HitList.Count; i++)
            {
                RaycastHit hit = HitList[i];

                //- 当たり判定のデバッグ表示
                if (Input.GetKey(KeyCode.Alpha1))
                {
                    float markdis = 0.1f;
                    Debug.DrawRay(RayStartPos, RayDir, Color.red, 3.0f);
                    Debug.DrawRay(hit.point, new Vector3(+markdis, +markdis, 0), Color.blue, 3.0f);
                    Debug.DrawRay(hit.point, new Vector3(+markdis, -markdis, 0), Color.blue, 3.0f);
                    Debug.DrawRay(hit.point, new Vector3(-markdis, +markdis, 0), Color.blue, 3.0f);
                    Debug.DrawRay(hit.point, new Vector3(-markdis, -markdis, 0), Color.blue, 3.0f);
                }
                if (hit.collider.gameObject.tag != "Warphole") continue; //- ステージオブジェクト以外なら次へ
                if (hit.distance > dis) continue;               //- 花火玉よりステージオブジェクトが奥にあれば次へ

                //- 当たった花火玉より手前にステージオブジェクトが存在する
                bBlast = true; //- フラグ変更
            }

            //- 遅延をかけて爆破
            if (bBlast) StartCoroutine(DelayDestroyCracker(obj, DelayTime, RangeStartPos));
        }
    }

    private void HardFire()
    {
        //- 無敵時間でない時に爆破が有効になった場合、処理する
        if (IsExploded && !_isInvinsible)
        {
            //- 爆発音の再生
            SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Explosion);
            //- 無敵フラグを設定
            _isInvinsible = true;
            //- 何回目の爆破かを更新     
            _blastCount++;
            if (_blastCount >= _blastNum)
            {
                //- 無敵時間のリセット
                _invTimeCount = 0;
                // 当たったオブジェクトのSphereColliderを有効にする
                this.transform.GetChild(0).gameObject.GetComponent<SphereCollider>().enabled = true;
                // 当たったオブジェクトのSphereColliderを有効にする
                DetonationCol.enabled = true;
                DetonationCol.EndDetonation(); //- 当たり判定のサイズを戻す
                // 指定した位置に生成
                GameObject fire = Instantiate(
                    _particleObject,                     // 生成(コピー)する対象
                    transform.position,           // 生成される位置
                    Quaternion.Euler(0.0f, 0.0f, 0.0f)  // 最初にどれだけ回転するか
                    );

                //- コントローラーの振動の設定
                vibration.SetVibration(30, 1.0f);

                // 爆発時に当たり判定を無効化
                GetComponent<SphereCollider>().isTrigger = true;
                GetComponent<MeshRenderer>().enabled = false;
            }
        }

        if (_isInvinsible)
        {
            //- まだ爆発してない
            if (_blastNum > _blastCount)
            {
                //- 無敵時間のカウント
                _invTimeCount += Time.deltaTime;
                //- 無敵時間終了時の処理
                if (_invTimeCount >= _firstInvTime)
                {
                    _isInvinsible = false;
                    _isExploded = false;
                    DetonationCol.EndDetonation(); //- 当たり判定のサイズを戻す
                    _invTimeCount = 0;
                }
            }
            else //- 爆発後
            {

                _afterTimeCount += Time.deltaTime;
                //- 当たり判定を消す処理
                if (_afterTimeCount >= _blastAfterTime)
                {
                    DetonationCol.EndDetonation(); //- 当たり判定の消滅
                    _invTimeCount = 0;
                }
            }
        }
    }

    private void DoubleFire()
    {
        //- 無敵時間でない時に爆破が有効になった場合、処理する
        if (!_isInvinsible && _isExploded)
        {
            //- 爆発音の再生
            SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Explosion);
            //- 爆発回数を更新
            _blastCount++;
            //- 無敵時間のリセット
            _invTimeCount = 0;
            //- 爆発中のフラグを設定
            _isInvinsible = true;
            // 爆発時の当たり判定を無効化
            _collisionObject.GetComponent<SphereCollider>().enabled = true;
            DetonationCol.enabled = true;

            if (_blastCount == 1)
            {
                //- 無敵時間の設定
                _MaxInvTime = _firstInvTime;
                //- １回目の花火を生成
                GameObject fire = Instantiate(
                    _particleObject,
                    transform.position,
                    Quaternion.Euler(0.0f, 0.0f, 0.0f));
                //- バリア破壊エフェクト生成
                SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.BarrierDes); // バリア破壊音
                GameObject barrier = Instantiate(
                _barrierParticleObj,
                new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z),
                Quaternion.Euler(0.0f, 0.0f, 0.0f)  
                );
                //- 不要になったオブジェクトを消去
                this.transform.GetChild(1).gameObject.SetActive(false);
                this.transform.GetChild(2).gameObject.SetActive(false);
                //- 花火のサイズ変更
                Vector3 Scale = fire.transform.localScale;
                Scale.x = 0.8f;
                Scale.y = 0.8f;
                Scale.z = 0.8f;
                fire.transform.localScale = Scale;
            }
            if (_blastCount == 2)
            {
                //- 自身のタグを切り替える
                gameObject.tag = "Untagged";
                //- 無敵時間の設定
                _MaxInvTime = _secondAfterTime;
                //- 不要になったオブジェクトを消去
                this.transform.GetChild(3).gameObject.SetActive(false);
                //- ２回目の花火を生成
                GameObject fire = Instantiate(
                    _multiBlast,
                    transform.position,
                    Quaternion.Euler(0.0f, 0.0f, 0.0f));
                //- 花火のサイズ変更
                Vector3 Scale = fire.transform.localScale;
                Scale.x = 1.3f;
                Scale.y = 1.3f;
                Scale.z = 1.3f;
                fire.transform.localScale = Scale;
            }
            //- コントローラーの振動の設定
            vibration.SetVibration(30, 1.0f);

            // 爆発時に当たり判定を無効化
            GetComponent<SphereCollider>().isTrigger = true;
        }
        if (_isInvinsible)
        {

            //- 無敵時間のカウント
            _invTimeCount += Time.deltaTime;
            //- 無敵時間終了時の処理
            if (_invTimeCount >= _MaxInvTime)
            {
                //- 爆発中のフラグを設定
                _isInvinsible = false;
                // 爆発時の当たり判定を無効化
                this.transform.GetChild(0).gameObject.GetComponent<SphereCollider>().enabled = false;
                GetComponent<SphereCollider>().isTrigger = false;
                DetonationCol.EndDetonation(); //- 当たり判定のサイズを戻す
                DetonationCol.enabled = false;
                //- 爆発判定を初期化
                _isExploded = false;
                _invTimeCount = 0;
            }
        }
    }

    private void ResurrectionBoxFire()
    {
        if (!_isOnce)
        { //- 爆発直後
            _isOnce = true;
            //- SpawnPlayerメソッドをdelayTime秒後に呼び出す
            StartCoroutine(SpawnPlayer(_delayTime));
        }
    }

    private void DragonflyFire()
    {
        //- 最初に一度だけ行う処理
        if (!bIsInit)
        {
            //- タグの変更(残り花火数のタグ検索を回避するため)
            this.tag = "Untagged";

            int DirNum = 16;
            float DirAngle = 360 / (float)DirNum;

            //- 引火したオブジェクトの座標
            Vector2 IgnPoint = HitInfo.objpoint;
            //- トンボ花火の座標
            Vector2 myPoint = transform.position;
            //- 移動方向ベクトルを生成
            Vector2 dis = myPoint - IgnPoint;
            float angle = Mathf.Atan2(dis.y, dis.x) * Mathf.Rad2Deg;
            angle += DirAngle / 2 ;
            SEManager.Instance.EffectSetPlaySE(SEManager.E_SoundEffect.TonboFire,0.3f); // トンボ花火の音
            //- 範囲外に出た角度を戻す
            if (angle < 0) angle += 360;
            //- 最終的な方向
            float moveAngle = 0;

            for (int i = 0; i < DirNum; i++)
            {
                float AngleMin = DirAngle * i;
                float AngleMax = DirAngle * (i + 1);
                if (AngleMin <= angle && angle < AngleMax)
                {
                    moveAngle = DirAngle * i + DirAngle / 2;
                    moveAngle -= DirAngle / 2;
                    moveAngle *= Mathf.Deg2Rad;
                    break;
                }
            }

            movedir.x = 1 * Mathf.Cos(moveAngle);
            movedir.y = 1 * Mathf.Sin(moveAngle);


            //- フラグの変更
            bIsInit = true;
            //- 失敗判定フラグ変更
            SceneChange scenechange = GameObject.Find("Main Camera").GetComponent<SceneChange>();
            scenechange.RequestStopMiss(true);

            //- エフェクト可視化
            _effectTonbo.SetActive(true);
        }

        //- トンボ花火の座標を取得
        Vector3 pos = transform.position;
        //- トンボ花火を移動させる
        pos.x += movedir.x * _tonboSpeed;
        pos.y += movedir.y * _tonboSpeed;
        //- トンボ花火の座標を適用
        transform.position = pos;

        //- トンボ花火の回転を取得
        Vector3 rot = transform.localEulerAngles;
        rot.z -= 45;
        transform.localEulerAngles = rot;
    }

    //- 遅れて起爆するクラッカーの関数
    private IEnumerator DelayDestroyCracker(GameObject obj, float delayTime,Vector3 pos)
    {
        //- delayTime秒待機する
        yield return new WaitForSeconds(delayTime);
        //- 既に花火玉が存在しなければ処理しない
        if (!obj) yield break;
        //- FireworksModuleの取得
        FireworksModule module = obj.gameObject.GetComponent<FireworksModule>();

        //- 起爆済みのトンボ花火だった場合、破壊フラグを変更
        if (module.Type == FireworksModule.FireworksType.Dragonfly && module.IsExploded == true) {
            obj.GetComponent<IgnitionCollision>().IsDestroy = true;
        }

        //- 花火タイプによって処理を分岐
        if (module.Type == FireworksModule.FireworksType.Boss)
            obj.GetComponent<FireworksModule>().IgnitionBoss(obj.gameObject);
        else
            obj.GetComponent<FireworksModule>().Ignition(pos);
    }

    //- オブジェクトのアクティブ判定を変更する関数
    private IEnumerator DelaySetActive(GameObject obj, bool bIsActive, float delayTime)
    {
        //- delayTime秒待機する
        yield return new WaitForSeconds(delayTime);
        //- アクティブ判定の変更
        obj.SetActive(bIsActive);
    }

    //- プレイヤーを再生成する関数
    private IEnumerator SpawnPlayer(float delayTime)
    {
        //- delayTime秒待機する
        yield return new WaitForSeconds(delayTime);

        //- 生成するプレイヤーの数
        int numPlayers = 1;

        //- 指定したプレイヤーの数分、生成する
        for (int i = 0; i < numPlayers; i++)
        {
            //- プレイヤーの生成位置
            Vector3 spawnPosition = new Vector3(
                transform.position.x, transform.position.y + 1.0f, transform.position.z);
            //- 生成時、顔の正面を向かせる為にY軸を150°回転させる
            Quaternion Angle = Quaternion.Euler(0.0f, 150.0f, 0.0f);
            //- 指定した位置にプレイヤーを生成する
            GameObject player = Instantiate(_playerPrefab, spawnPosition, Angle);

            //- 生成音の再生
            SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Generated);

            //=== プレイヤーを徐々に拡大し出現させるアニメーション ===
            player.transform.localScale = Vector3.zero; // スケールの初期状態
            player.transform.DOScale(Vector3.one, _animationTime)
                .SetEase(Ease.Linear);

            //- アニメーションの遅延
            yield return new WaitForSeconds(_animationDelayTime);
        }

        //- アニメーションの開始時刻を設定
        float startTime      = Time.time;
        //- 復活箱の初期スケールを設定
        Vector3 initialScale = transform.localScale;

        //- 消滅音の再生
        SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.Extinction);

        //=== 復活箱を徐々に消滅させるアニメーション ===
        transform.DOScale(Vector3.zero, _boxDisTime)
            .SetEase(Ease.Linear);

        //- アニメーションの遅延
        yield return new WaitForSeconds(_animationDelayTime);

        Destroy(gameObject); // オブジェクトを破棄
    }

    private void Boss2Fire()
    {
        if (bStartMovie) return; //- 演出が始まっているならリターン

        //- 最初に一度だけ行う処理
        if (!bIsInit)
        {
            //- バリアを非可視化
            transform.GetChild(1).gameObject.SetActive(false);
            //- バリア破壊エフェクト生成
            if (!_isOnce)
            {
                _isOnce = true;
                SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.BarrierDes); // バリア破壊音
                GameObject barrier = Instantiate(
                    _boss2barrierParticleObj,
                    new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z),
                    Quaternion.Euler(0.0f, 0.0f, 0.0f)
                    );
            }
        }

        //- 猶予時間内に引火したら実行する処理
        enemyNum = countEnemy.GetCurrentCountNum(); // 花火玉の残存数更新
        if (HitInfo.hitcount >= 2 && enemyNum <= 1)
        {
            //- マテリアルの取得
            Material material = _boss2Obj1.GetComponent<Renderer>().material;
            Material material2 = _boss2Obj2.GetComponent<Renderer>().material;
            //- 引火時のフェード処理
            material.DOColor(BossIgniteColor, FadeTime);
            material2.DOColor(BossIgniteColor, FadeTime);

            GameObject.Find("InGameSelect").GetComponent<EntryAnime>().OutMove();
            GameObject.Find("InGameReset").GetComponent<EntryAnime>().OutMove();
            GameObject.Find("InGameTips").GetComponent<EntryAnime>().OutMove();

            //- フラグ変更
            SceneChange scenechange = GameObject.Find("Main Camera").GetComponent<SceneChange>();
            scenechange.RequestStopClear(true);
            //- アニメーション処理
            transform.DOMoveY(-15, 1.5f).SetEase(Ease.OutSine).SetLink(gameObject);
            transform.DOMoveY(20, 0.7f).SetEase(Ease.OutSine).SetDelay(1.5f).SetLink(gameObject);
            DOTween.Sequence().SetDelay(1.5f).OnComplete(() =>
            { SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.BossBelt); });
            //- 演出用スクリプトの取得
            MovieManager movie = MovieObject.GetComponent<MovieManager>();
            //- 演出フラグ変更
            movie.SetMovieFlag(true);
            //- 演出開始
            DOVirtual.DelayedCall(2.1f, () => movie.StartVillageMovie(), false);
            //- 破壊処理
            Destroy(gameObject, 2.2f);

            //- 念の為、時間をリセット
            TimeCount = 0;
            //- 演出開始フラグを切り替え
            bStartMovie = true;
            return;
        }

        //- 時間経過
        TimeCount += Time.deltaTime;

        if (TimeCount < _synchroTime) return; //- 同時引火猶予内ならリターン


        //- 情報をリセットする
        _isExploded = false;
        bIsInit = false;
        _isOnce = false;
        TimeCount = 0;
        HitInfo.hitcount = 0;

        //- バリア可視化
        transform.GetChild(1).gameObject.SetActive(true);
    }
    
    private void Boss3Fire()
    {
        if (!_isOnce)
        {
            _isOnce = true;
            //- 引火数を増やす
            igniteCount++;
            //- リトライを防ぐため、タグを変更
            gameObject.tag = "Untagged";
            //- マテリアルの取得
            Material material = _boss3obj.GetComponent<Renderer>().material;
            //- 引火時のフェード処理
            material.DOColor(BossIgniteColor, FadeTime);
        }

        enemyNum = countEnemy.GetCurrentCountNum(); // 花火玉の残存数更新
        if (!bAnime && igniteCount >= 3 && enemyNum <= 0)
        {
            bAnime = true;
            GameObject.Find("InGameSelect").GetComponent<EntryAnime>().OutMove();
            GameObject.Find("InGameReset").GetComponent<EntryAnime>().OutMove();
            GameObject.Find("InGameTips").GetComponent<EntryAnime>().OutMove();
            //- フラグ変更
            SceneChange scenechange = GameObject.Find("Main Camera").GetComponent<SceneChange>();
            scenechange.RequestStopClear(true);
            //- アニメーション処理
            transform.DOMoveY(transform.position.y - 2.0f, 1.0f).SetEase(Ease.OutSine).SetDelay(1.0f).SetLink(gameObject);
            transform.DOMoveY(20, 0.7f).SetEase(Ease.OutSine).SetDelay(2.3f).SetLink(gameObject);
            DOTween.Sequence().SetDelay(1.5f).OnComplete(() =>
            { SEManager.Instance.SetPlaySE(SEManager.E_SoundEffect.BossBelt); }); // ヌシが打ち上がる時の音
            
            //- 演出用スクリプトの取得
            MovieManager movie = MovieObject.GetComponent<MovieManager>();
            //- 演出フラグ変更
            movie.SetMovieFlag(true);
            //- 演出開始
            DOVirtual.DelayedCall(3.1f, () => movie.StartVillageMovie(), false);
            //- 破壊処理
            Destroy(gameObject, 3.2f);
        }
    }

    private IEnumerator MakeDragonEffect(float delayTime, int maxEffect)
    {
        //- エフェクトを生成
        for (int i = 0; i < maxEffect; i++)
        {
            //- delayTime秒待機する
            yield return new WaitForSeconds(delayTime);
            SEManager.Instance.EffectSetPlaySE(SEManager.E_SoundEffect.DragonFire, 0.1f);// ドラゴンのエフェクト音再生
            //- エフェクト生成のために、座標を取得
            Vector3 pos = transform.position;
            pos.x -= 3.4f;
            pos.y += 3.5f;
            //- 指定した位置に生成
            GameObject fire = Instantiate(_particleObject, pos, Quaternion.Euler(-90.0f, 0.0f, 0.0f));
        }
    }

    private void PlayDragonMotion(GameObject gameObject)
    {
        //- Animationコンポーネントを取得してモーションを再生する
        if (gameObject.GetComponent<Animator>() != null)
        {
            Animator animator = gameObject.GetComponent<Animator>();
            animator.SetBool("Fire", true);
        }
    }

    private void Boss4Fire()
    {
        enemyNum = countEnemy.GetCurrentCountNum(); // 花火玉の残存数更新
        Debug.Log(enemyNum);
        if (!bAnime && enemyNum <= 1)
        {
            bAnime = true;

            //- モーションを切り替える
            _motionObject.GetComponent<DragonAnimation>().BreathAnime();

            //- MakeDragonEffectメソッドを呼び出す
            StartCoroutine(MakeDragonEffect(2.5f, 1));

            //- コントローラーの振動の設定
            vibration.SetVibration(30, 1.0f);

            //- 自身のタグを切り替える
            gameObject.tag = "Untagged";

            //- 色を変更
            Material mat = _materialObject.GetComponent<Renderer>().material;
            mat.DOColor(new Color(1.0f, 0.4f, 0.4f, 1.0f),1.0f);

            GameObject.Find("InGameSelect").GetComponent<EntryAnime>().OutMove();
            GameObject.Find("InGameReset").GetComponent<EntryAnime>().OutMove();
            GameObject.Find("InGameTips").GetComponent<EntryAnime>().OutMove();
            //- フラグ変更
            SceneChange scenechange = GameObject.Find("Main Camera").GetComponent<SceneChange>();
            scenechange.RequestStopClear(true);
            //- 演出用スクリプトの取得
            MovieManager movie = MovieObject.GetComponent<MovieManager>();
            //- 演出フラグ変更
            movie.SetMovieFlag(true);
            //- 演出開始
            DOVirtual.DelayedCall(3.0f, () => movie.StartVillageMovie(), false);
        }
    }
}