using System.Collections.Generic;
using System.Xml;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections;
using UnityEngine;

using 技能编辑器;

public class config_skill
{
    public int id;
    [EditorName("升级技能")]
    public int next_skill_id;
    [EditorName("名称")]
    public string name;
    [EditorName("图标")]
    public string icon;
    [EditorName("描述")]
    public string description;
    [EditorName("预CD")]
    public float pre_cd;

    [EditorName("打断等级"), EditorDescription("技能打断等级。高等级会打断低等级")]
    public int interrupt_level;//技能打断等级。高等级会打断低等级

    public enum SkillType
    {
        Attack_Skill,
        Special_Skill,
        Defence_Skill
    }
    [EditorName("技能类型"), EditorDescription("普攻，特技，防御")]
    public SkillType type;

    public float cd;
    public float common_cd;

    [EditorName("技能结束开始cd")]
    public bool start_cd_at_skill_end;

    [EditorName("可连续使用次数")]
    public int coolMax = 1;

    [EditorName("消耗MP")]
    public int costMp;
    [EditorName("消耗武器值")]
    public int cost_energy;

    [EditorName("帧数")]
    public int frame_count;

    /// <summary>
    /// 收尾动作名称
    /// </summary>
    [EditorName("收尾动作")]
    public string actionEndName;

    ///// <summary>
    ///// 武器技能形态
    ///// </summary>
    //[EditorName("武器类型")]
    //public int weaponType;

    //自动朝向敌人
    [EditorName("朝向敌人")]
    public bool face_enemy;

    [EditorName("主动取消")]
    public bool trigger_cancel;

    [EditorName("无视硬直")]
    public bool defy_hit_recover;

    [EditorName("施法距离")]
    public float range=2.0f;
    /// <summary>
    /// 动画速度因子，默认为1
    /// </summary>
    [EditorName("动画速度")]
    public float anim_speed_factor = 1.0f;

    [NonSerialized, EditorName("关键帧")]
    public config_skill_frame[] frames = new config_skill_frame[0];

    [EditorName("允许背击")]
    public bool allow_hit_back = true;
    [EditorName("允许前面攻击")]
    public bool allow_hit_front = true;

    [EditorName("特化脚本")]
    public string script = "Effect";

    [EditorName("类型伤害调整")]
    public int skill_type_dam = 1000;
}


public class Data
{
    public static Dictionary<int, config_skill> skills;
    

    public static config_skill GetSkill(int id)
    {
        // if (id <= 0 || id > skills.Count)
        //     return null;

        //return skills[id-1];

        config_skill data = null;
        if (skills.TryGetValue(id, out data))
            return data;
        return null;
    }
    
    public static string GetTablePath()
    {
        string outPath = Form1.table_path;
        return outPath;
    }

    //todo:加载所有数据
    public static void Load()
    {
        LoadSkill();
        //LoadCharacter();
        //LoadMonster();
        //LoadShake();
        //players = ReadCSVTable<config_player>(GetTablePath() + "/Player/player.csv");
        //LoadWeather();
        //LoadSound();
        //LoadCamera();
        //buffs = ReadCSVTable<config_buff>(GetTablePath() + "/Skill/buff.csv");
    }

    static List<T> ReadCSVTable<T>(string path, int data_start_line = 3) where T : new()
    {
        List<T> table = new List<T>();

        string text = File.ReadAllText(path, System.Text.Encoding.UTF8);

        List<List<string>> records = new List<List<string>>();

        using (CsvReader reader = new CsvReader(Encoding.UTF8, text))
        {
            while (reader.ReadNextRecord())
                records.Add(reader.Fields);
        }

        //        //各平台换行符
        //#if UNITY_IOS
        //        string[] lines = text.Split('\r');
        //#elif UNITY_STANDALONE_WIN
        //        string[] lines = text.Split('\r');
        //#elif UNITY_STANDALONE_OSX
        //        string[] lines = text.Split('\r');
        //#elif UNITY_ANDROID
        //        string[] lines = text.Split('\n');
        //#else
        //#error "UnSurported Platform"
        //#endif
        //第一行是标题，第二行是说明，第三行是类型
        List<string> titles = records[0];

        for (int i = data_start_line; i < records.Count; i++)
        {
            List<string> cells = records[i];

            if (cells.Count != titles.Count) //无效的单元
            {
                Debug.LogWarning(path + ":Invalid Table Line " + i);
                continue;
            }


            T t = new T();

            for (int j = 0; j < titles.Count; j++)
            {
                FieldInfo fi = t.GetType().GetField(titles[j]);
                if (fi != null)
                {
                    if (!string.IsNullOrEmpty(cells[j]))
                    {
                        try
                        {
                            if (fi.FieldType.IsPrimitive || fi.FieldType == typeof(string) || fi.FieldType.IsEnum)
                            {
                                object v = System.ComponentModel.TypeDescriptor.GetConverter(fi.FieldType).ConvertFrom(cells[j]);
                                fi.SetValue(t, v);
                            }
                            else
                            {
                                object v = fastJSON.JSON.ToObject(cells[j], fi.FieldType);
                                fi.SetValue(t, v);
                            }

                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e.Message);
                            Debug.LogError(cells[j] + "parse error:" + e.StackTrace);
                        }

                    }
                    else
                    {

                    }
                }
            }
            table.Add(t);
        }
        return table;
    }

    static void LoadSkill()
    {
        skills = new Dictionary<int, config_skill>();
        List<config_skill> skillList = ReadCSVTable<config_skill>(GetTablePath() + "/Skill/skill.csv", 1);
        foreach (var s in skillList)
        {
            skills[s.id] = s;
        }
        List<config_skill_frame> frames = ReadCSVTable<config_skill_frame>(GetTablePath() + "/Skill/skill_frame.csv", 1);
        List<config_skill_frame> skillframe = new List<config_skill_frame>();
        for (int i = 0; i < frames.Count; i++)
        {
            skillframe.Add(frames[i]);
            if (i < frames.Count - 1)
            {
                if (frames[i + 1].skill_id != frames[i].skill_id)
                {
                    skillframe.Sort((a, b) => { return a.time < b.time ? -1 : 1; });
                    Debug.Log("id:" + frames[i].skill_id);
                    GetSkill(frames[i].skill_id).frames = skillframe.ToArray();
                    skillframe.Clear();
                }
            }
            else
            {
                skillframe.Sort((a, b) => { return a.time < b.time ? -1 : 1; });
                GetSkill(frames[i].skill_id).frames = skillframe.ToArray();
            }

        }
    }

}