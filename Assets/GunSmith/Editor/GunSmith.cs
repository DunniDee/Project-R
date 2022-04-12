using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using gs_Types;

public class GunSmith : EditorWindow
{
    //UI

    Texture2D ImageSectionTexture;
    Texture2D BlankSectionTexture;
    Rect BodySection;
    Rect HeaderSection;
    Rect ImageSection;

    Rect TriggerSection;
    Rect GunBodySection;
    Rect MagSection;
    Rect BarrelSection;
    Rect StockSection;
    Rect MiscSection;

    Rect TriggerSectionStat;
    Rect GunBodySectionStat;
    Rect MagSectionStat;
    Rect BarrelSectionStat;
    Rect StockSectionStat;
    Rect MiscSectionStat;

    Rect ButtonSection;

    static gs_GunData GunData;
    public static gs_GunData GunInfo {get {return GunData; } }

    [MenuItem("Window/Gun Smith")]
    static void OpenWindow()
    {
        GunSmith window = (GunSmith)GetWindow(typeof(GunSmith));
        //window.minSize= new Vector2(250,800);
        window.Show();
    }
    private void OnEnable() 
    {
        InitTextures();
        InitData();
    }

    public void InitData()
    {
        GunData = (gs_GunData)ScriptableObject.CreateInstance(typeof(gs_GunData));

        GenerateGunName();

        GunData.ShootSound = Resources.Load<AudioClip>("GunSmith/gs_DefaultShot");
        GunData.ReloadSound = Resources.Load<AudioClip>("GunSmith/gs_DefaultReload");
    }

    private void InitTextures()
    {
        ImageSectionTexture = Resources.Load<Texture2D>("Icons/Editor_GunSmith_Graphic");
        BlankSectionTexture = Resources.Load<Texture2D>("Icons/Editor_Gunsmith_Background");
    }

    private void OnGUI() 
    { 
        if(GunData == null)
        {
            InitData();
        }

        SetImageTexture();
        DrawLayouts();

        //Drawing Panels
        DrawHeader();
        DrawTrigger();
        DrawGunBody();
        DrawMag();
        DrawBarrel();
        DrawStock();
        DrawMisc();

        AssetDatabase.SaveAssets();

        //fixes minor bug where some atributes would show due to persistent data
        if (GunData.ShotType == gs_ShotType.Hitscan && GunData.ChargeType == gs_ChargeType.Force)
        {
            Debug.LogError("Hitscan shot type can not be assigned a force");
            GunData.ChargeType = gs_ChargeType.Damage;
        }
    }

    private void CreateGunData()
    {
        Debug.Log("Creating " + GunData.Name + " Data");

        string DataPath = "Assets/GunSmith/ScriptableObjects/";
        DataPath += GunData.Name + ".asset";

        AssetDatabase.CreateAsset(GunSmith.GunData, DataPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    private void CreatePrefab()
    {
        Debug.Log("Creating " + GunData.Name + " Prefab");

        string PrefabPath = "Assets/GunSmith/Prefabs/";
        PrefabPath += GunData.Name + ".prefab";

        AssetDatabase.CopyAsset("Assets/Resources/GunSmith/gs_BaseGun.prefab", PrefabPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        GameObject GunPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(PrefabPath, typeof(GameObject)); 
        if (!GunPrefab.GetComponent<gs_GunController>())
        {
            GunPrefab.AddComponent<gs_GunController>();
        }

        if (!GunPrefab.GetComponent<AudioSource>())
        {
            GunPrefab.AddComponent<AudioSource>();
        }

        GunPrefab.GetComponent<gs_GunController>().m_GunData = GunData;
    }

    private void SetImageTexture()
    {
        if(GUIUtility.GUIToScreenRect(ImageSection).Contains(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)))
        {
            ImageSectionTexture = Resources.Load<Texture2D>("Icons/Editor_GunSmith_Graphic");
        }
        if(GUIUtility.GUIToScreenRect(TriggerSection).Contains(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)))
        {
            ImageSectionTexture = Resources.Load<Texture2D>("Icons/Editor_GunSmithGraphic_Trigger");
        }
        if(GUIUtility.GUIToScreenRect(GunBodySection).Contains(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)))
        {
            ImageSectionTexture = Resources.Load<Texture2D>("Icons/Editor_GunSmithGraphic_Body");
        }
        if(GUIUtility.GUIToScreenRect(MagSection).Contains(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)))
        {
            ImageSectionTexture = Resources.Load<Texture2D>("Icons/Editor_GunSmithGraphic_Magazine");
        }
        if(GUIUtility.GUIToScreenRect(BarrelSection).Contains(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)))
        {
            ImageSectionTexture = Resources.Load<Texture2D>("Icons/Editor_GunSmithGraphic_Barrel");
        }
        if(GUIUtility.GUIToScreenRect(StockSection).Contains(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)))
        {
            ImageSectionTexture = Resources.Load<Texture2D>("Icons/Editor_GunSmithGraphic_Stock");
        }
        if(GUIUtility.GUIToScreenRect(MiscSection).Contains(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)))
        {
            ImageSectionTexture = Resources.Load<Texture2D>("Icons/Editor_GunSmithGraphic_Misc");
        }
    }

    private void DrawLayouts()
    {
        HeaderSection.x = 0;
        HeaderSection.y = 0; 
        HeaderSection.width = Screen.width / 2;
        HeaderSection.height = Screen.height / 8;

        GUI.DrawTexture(HeaderSection, BlankSectionTexture);

        ImageSection.x = Screen.width / 2;
        ImageSection.y = 0; 

        ImageSection.width = Screen.width / 2;
        ImageSection.height = Screen.height / 8;

        GUI.DrawTexture(ImageSection, ImageSectionTexture);

        BodySection.x = 0;
        BodySection.y = Screen.height / 8; 
        BodySection.width = Screen.width;
        BodySection.height = Screen.height - Screen.height / 8;

        GUI.DrawTexture(BodySection, BlankSectionTexture);

        // Trigger Sections
        TriggerSection.x = 0;
        TriggerSection.y = (Screen.height / 8); 
        TriggerSection.width = Screen.width;
        TriggerSection.height = Screen.height / 6;

        TriggerSectionStat.x = Screen.width / 3;
        TriggerSectionStat.y = (Screen.height / 8); 
        TriggerSectionStat.width = (Screen.width / 3) * 2;
        TriggerSectionStat.height = Screen.height / 6;

        // Gun Body Sections
        GunBodySection.x = 0;
        GunBodySection.y = (Screen.height / 8) * 2; 
        GunBodySection.width = Screen.width;
        GunBodySection.height = Screen.height / 6;

        GunBodySectionStat.x = Screen.width / 3;
        GunBodySectionStat.y = (Screen.height / 8) * 2; 
        GunBodySectionStat.width = (Screen.width / 3) * 2;
        GunBodySectionStat.height = Screen.height / 6;

        // Gun Body Sections
        MagSection.x = 0;
        MagSection.y = (Screen.height / 8) * 3; 
        MagSection.width = Screen.width;
        MagSection.height = Screen.height / 6;

        MagSectionStat.x = Screen.width / 3;
        MagSectionStat.y = (Screen.height / 8) * 3; 
        MagSectionStat.width = (Screen.width / 3) * 2;
        MagSectionStat.height = Screen.height / 6;

        // Barrel Sections
        BarrelSection.x = 0;
        BarrelSection.y = (Screen.height / 8) * 4; 
        BarrelSection.width = Screen.width;
        BarrelSection.height = Screen.height / 6;

        BarrelSectionStat.x = Screen.width / 3;
        BarrelSectionStat.y = (Screen.height / 8) * 4; 
        BarrelSectionStat.width = (Screen.width / 3) * 2;
        BarrelSectionStat.height = Screen.height / 6;

        // Stock Sections
        StockSection.x = 0;
        StockSection.y = (Screen.height / 8) * 5; 
        StockSection.width = Screen.width;
        StockSection.height = Screen.height / 6;

        StockSectionStat.x = Screen.width / 3;
        StockSectionStat.y = (Screen.height / 8) * 5; 
        StockSectionStat.width = (Screen.width / 3) * 2;
        StockSectionStat.height = Screen.height / 6;

        // Misc Sections
        MiscSection.x = 0;
        MiscSection.y = (Screen.height / 8) * 6; 
        MiscSection.width = Screen.width;
        MiscSection.height = Screen.height / 6;
        
        MiscSectionStat.x = Screen.width / 3;
        MiscSectionStat.y = (Screen.height / 8) * 6; 
        MiscSectionStat.width = (Screen.width / 3) * 2;
        MiscSectionStat.height = Screen.height / 6;

        ButtonSection.x = 0;
        ButtonSection.y = (Screen.height / 8) * 7; 
        ButtonSection.width = Screen.width;
        ButtonSection.height = Screen.height / 6;
    }

    private void DrawHeader()
    {
        GUILayout.BeginArea(HeaderSection);

        GUILayout.Label("Gun Designer");

        if(GUILayout.Button("New Gun", GUILayout.Height(20)))
        {
            InitData();
        }

        if(GUILayout.Button("Save Stats", GUILayout.Height(20)))
        {
            EditorUtility.SetDirty(GunData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        GUILayout.EndArea();
    }

    private void DrawTrigger()
    {
        //Trigger Section Text
        GUILayout.BeginArea(TriggerSection);
        GUILayout.Label("Damage");
        GUILayout.Label("Trigger Type");

        if (GunData.TriggerType == gs_TriggerType.Charge)
        {
            GUILayout.Label("Charge Time");
            GUILayout.Label("Charge Type");

            if (GunData.ChargeType == gs_ChargeType.Damage)
            {
                GUILayout.Label("Max Damage");
            }

            if (GunData.ChargeType == gs_ChargeType.Accuracy)
            {
                GUILayout.Label("Min Spread");
            }

            if (GunData.ChargeType == gs_ChargeType.ShotCount)
            {
                GUILayout.Label("Max Shot Count");
            }

            if (GunData.ChargeType == gs_ChargeType.Force)
            {
                GUILayout.Label("Max Force");
            }

        }

        if (GunData.TriggerType == gs_TriggerType.Burst ||
            GunData.TriggerType == gs_TriggerType.Charge &&  GunData.ChargeType == gs_ChargeType.Burst)
        {
            GUILayout.Label("Burst Count");
            GUILayout.Label("Burst Delay");
        }

        if (GunData.TriggerType == gs_TriggerType.FullAutoRampUp)
        {
            GUILayout.Label("Ramp Up Time");

            GUILayout.Label("Max FireRate");
        }
        GUILayout.EndArea(); // end Trigger section


        // Trigger Section Stats
        GUILayout.BeginArea(TriggerSectionStat); // trigger section Stats

        GunData.Damage = (float)EditorGUILayout.FloatField(GunData.Damage);

        GunData.TriggerType = (gs_TriggerType)EditorGUILayout.EnumPopup(GunData.TriggerType);

        if (GunData.TriggerType == gs_TriggerType.Charge)
        {
            GunData.ChargeTime = (float)EditorGUILayout.FloatField(GunData.ChargeTime);
            GunData.ChargeType = (gs_ChargeType)EditorGUILayout.EnumPopup(GunData.ChargeType);

            if (GunData.ChargeType == gs_ChargeType.Damage)
            {
                GunData.MaxDamage = (float)EditorGUILayout.FloatField(GunData.MaxDamage);
            }

            if (GunData.ChargeType == gs_ChargeType.Accuracy)
            {
                GunData.MinSpreadAngle = (float)EditorGUILayout.FloatField(GunData.MinSpreadAngle);
            }

            if (GunData.ChargeType == gs_ChargeType.ShotCount)
            {
                GunData.MaxShotCount = (int)EditorGUILayout.FloatField(GunData.MaxShotCount);
            }

            if (GunData.ChargeType == gs_ChargeType.Force)
            {
                GunData.MaxProjectileForce = (float)EditorGUILayout.FloatField(GunData.MaxProjectileForce);
            }
        }

        if (GunData.TriggerType == gs_TriggerType.Burst ||
            GunData.TriggerType == gs_TriggerType.Charge &&  GunData.ChargeType == gs_ChargeType.Burst)
        {
            GunData.BurstCount = (float)EditorGUILayout.FloatField(GunData.BurstCount);
            GunData.BurstDelay = (float)EditorGUILayout.FloatField(GunData.BurstDelay);
        }

        if (GunData.TriggerType == gs_TriggerType.FullAutoRampUp)
        {
            GunData.RampUpTime = (float)EditorGUILayout.FloatField(GunData.RampUpTime);
            GunData.MaxFireRate = (float)EditorGUILayout.FloatField(GunData.MaxFireRate);
        }
        GUILayout.EndArea(); // end Trigger section Stats
    }

    private void DrawGunBody()
    {
        // Gun Body Section
        GUILayout.BeginArea(GunBodySection); // gun body section
        GUILayout.Label("Fire Rate");

        GUILayout.Label("Shot Type");

        if (GunData.ShotType == gs_ShotType.Projectile) // is a projectile weapon
        {
            GUILayout.Label("Projectile");
            GUILayout.Label("Force");
            GUILayout.Label("Lifetime");
            GUILayout.Label("Apply Gravity");
        }
        GUILayout.EndArea(); // end gun body section


        // Gun Body Section Stats
        GUILayout.BeginArea(GunBodySectionStat); // gun body section Stats
        GunData.FireRate = (float)EditorGUILayout.FloatField(GunData.FireRate);
        GunData.ShotType = (gs_ShotType)EditorGUILayout.EnumPopup(GunData.ShotType);

        if (GunData.ShotType == gs_ShotType.Projectile) // is a projectile weapon
        {
            GunData.Projectile = (GameObject)EditorGUILayout.ObjectField(GunData.Projectile, typeof(GameObject), false);
            GunData.ProjectileForce = (float)EditorGUILayout.FloatField(GunData.ProjectileForce);
            GunData.ProjectileLifetime = (float)EditorGUILayout.FloatField(GunData.ProjectileLifetime);
            GunData.ApplyGravity = (bool)EditorGUILayout.Toggle(GunData.ApplyGravity);
        }
        GUILayout.EndArea(); // end gun body section stats
    }

    private void DrawMag()
    {
        // Mag Section Text
        GUILayout.BeginArea(MagSection); //Mag section
        GUILayout.Label("Reload Time");

        GUILayout.Label("Ammo Count");
        GUILayout.EndArea(); //Mag Section Ends


        // Mag Section Stats
        GUILayout.BeginArea(MagSectionStat); //Mag section Stat        
        GunData.ReloadTime = (float)EditorGUILayout.FloatField(GunData.ReloadTime);

        GunData.AmmoCount = (float)EditorGUILayout.FloatField(GunData.AmmoCount);
        GUILayout.EndArea(); //Mag Section Ends Stat
    }

    private void DrawBarrel()
    {
        // Barrel Section Text
        GUILayout.BeginArea(BarrelSection); // Barrel Section Starts
        GUILayout.Label("Spread Angle");

        GUILayout.Label("Shot Count");
        GUILayout.EndArea(); // Barrel Section Ends


        // Barrel Section Stats
        GUILayout.BeginArea(BarrelSectionStat); // Barrel Section Stat Starts
        GunData.SpreadAngle = (float)EditorGUILayout.FloatField(GunData.SpreadAngle);

        GunData.ShotCount = (int)EditorGUILayout.FloatField(GunData.ShotCount);
        GUILayout.EndArea(); // Barrel Section Stat Ends
    }

    private void DrawStock()
    {
        // Stock Section Text
        GUILayout.BeginArea(StockSection);// Stock Section starts
        GUILayout.Label("Recoil");
        GUILayout.EndArea();// ends stock section 


        // Stock Section Text
        GUILayout.BeginArea(StockSectionStat);// Stock Section starts Stat
        GunData.Recoil = (float)EditorGUILayout.FloatField(GunData.Recoil);
        GUILayout.EndArea(); // ends stock section stats
    }

    private void DrawMisc()
    {
        // Misc Section Text
        GUILayout.BeginArea(MiscSection); // misc section starts
        GUILayout.Label("Gun Name");
        GUILayout.Label("Shoot Sound");
        GUILayout.Label("Reload Sound");
        GUILayout.Label("Gun Data");
        GUILayout.EndArea(); // misc section ends


        // Misc Section Stats
        GUILayout.BeginArea(MiscSectionStat);  // misc section starts stats
        GunData.Name = (string)EditorGUILayout.TextField(GunData.Name);
        GUILayout.Space(5);
        GunData.ShootSound = (AudioClip)EditorGUILayout.ObjectField(GunData.ShootSound, typeof(AudioClip), false);
        GunData.ReloadSound = (AudioClip)EditorGUILayout.ObjectField(GunData.ReloadSound, typeof(AudioClip), false);
        GunData = (gs_GunData)EditorGUILayout.ObjectField(GunData, typeof(gs_GunData), false);
        GUILayout.EndArea();  // misc section ends stats

        GUILayout.BeginArea(ButtonSection);  // misc section starts stats

        if(GUILayout.Button("Create Gun Data", GUILayout.Height(20)))
        {
            CreateGunData();
        }
        
        if(GUILayout.Button("Create Prefab", GUILayout.Height(20)))
        {
            CreatePrefab();
        }
        GUILayout.EndArea();  // misc section ends stats
    }

    private void GenerateGunName()
    {
        GunData.Name = "";

        int Index = Random.Range(0,10);
        switch (Index)
        {
            case 0:
                GunData.Name  += "Forcefull ";
            break;

            case 1:
                GunData.Name  += "Loaded ";
            break;

            case 2:
                GunData.Name  += "L + ";
            break;

            case 3:
                GunData.Name  += "Feeble ";
            break;

            case 4:
                GunData.Name  += "Mega ";
            break;

            case 5:
                GunData.Name  += "Small ";
            break;

            case 6:
                GunData.Name  += "Old ";
            break;

            case 7:
                GunData.Name  += "New ";
            break;

            case 8:
                GunData.Name  += "Grumbo ";
            break;

            case 9:
                GunData.Name  += "Schtimby ";
            break;

            default:
            break;
        }

        Index = Random.Range(0,10);
        switch (Index)
        {
            case 0:
                GunData.Name  += "Pibbler ";
            break;

            case 1:
                GunData.Name  += "Shooter ";
            break;

            case 2:
                GunData.Name  += "Cannon ";
            break;

            case 3:
                GunData.Name  += "+ Ratio ";
            break;

            case 4:
                GunData.Name  += "Gat ";
            break;

            case 5:
                GunData.Name  += "Big Iron ";
            break;

            case 6:
                GunData.Name  += "Rifle ";
            break;

            case 7:
                GunData.Name  += "Blaster ";
            break;

            case 8:
                GunData.Name  += "Zapper ";
            break;

            case 9:
                GunData.Name  += "Schtimby ";
            break;

            default:
            break;
        }

        Index = Random.Range(0,10);
        switch (Index)
        {
            case 0:
                GunData.Name  += "the Accursed";
            break;

            case 1:
                GunData.Name  += "of Death";
            break;

            case 2:
                GunData.Name  += "my Beloved";
            break;

            case 3:
                GunData.Name  += "of the Maidenless";
            break;

            case 4:
                GunData.Name  += "of the Racist";
            break;

            case 5:
                GunData.Name  += "of Herresy";
            break;

            case 6:
                GunData.Name  += "of Justice";
            break;

            case 7:
                GunData.Name  += "the Judge";
            break;

            case 8:
                GunData.Name  += "of Minor Importance";
            break;

            case 9:
                GunData.Name  += "Schtimby";
            break;

            default:
            break;
        }
    }
}
