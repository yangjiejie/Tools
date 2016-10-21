using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

//用来在技能编辑器中显示中文字段名称
class EditorNameAttribute : Attribute
{
    string _name;

    public EditorNameAttribute(string name)
    {
        this._name = name;
    }

    public string EditorName
    {
        get { return this._name; }
        set { this._name = value; }
    }
}

//用来在编辑器中显示中文字段描述
class EditorDescriptionAttribute : Attribute
{
    string _name;

    public EditorDescriptionAttribute(string name)
    {
        this._name = name;
    }

    public string EditorDescription
    {
        get { return this._name; }
        set { this._name = value; }
    }
}



public enum SkillFrameAction
{
    None,
    ATTACK,     //攻击
    PLAY_FX,    //播放特效
    PLAY_SOUND, //播放声音
    DRIFT,      //位移
    SHAKE,      //相机震动
    SHOOT,       //发射弹道
    PLAY_ANIM,  //播放动画
    MOVE,   //怪物登场
    GHOST,  //残影
    START,  //起手
    FLAW,    //破绽
    PA_BODY,     //霸体时间
    NO_HIT,   //不可被攻击
    SUMMON,    //召唤(陷阱或者怪物，友军）
    LOOP_FRAME,       //循环
    CAN_MOVE,               //可移动范围
    CAN_ROTATE,           //可旋转范围
    SHOW,
    VARIANT,
    JUMP_FRAME,                 //条件跳转
    DRIFT_TARGET,               //漂移目标
    TOGGLE_WEAPON,               //切换武器
    BUFF,                        //添加或移除buff
    SHOW_RANGE,                       //显示技能范围
    CHARGE,                     //蓄力
    PLAY_FX_LINK_TARGET,        //链接目标的特效
    MONSTER_COMBINATION,        //合体
    ATTRACTION,                    //吸取
    SCREEN_LIGHT,				//关灯杀......
    FRAME_STATE,                //帧状态数组
    QTE,
    INVISIBLE,					//不可见
    [EditorName("军师/通用")]
    SHOW_COUNSELLOR,            //显示军师图片
    BLOCK,                      //格挡状态
    [EditorName("军师/田丰")]
    RECOVE_SHIELD,              //护盾回复
    LIGHT,
    [EditorName("军师/祢衡")]
    RECOVER_HP_COST_MP,         //消耗mp回复hp
    [EditorName("BOSS")]
    DRIFT_TARGET_DELAY,
    [EditorName("军师/蒋琬")]
    ATTACK_HP_COST_MP,
    [EditorName("军师/法正")]
    FAZHENG_DMG_HP_COST_MP,
    [EditorName("军师/通用")]
    LOST_HP,
    [EditorName("军师/李儒")]
    SUMMON_ATTACK_WARDS              //召唤攻击结界
}


public class config_action_summon_attack_words
{
    [EditorName("召唤区域")]
    public Quad[] box;

    [EditorName("陷阱攻击频率"), EditorDescription("如果这个值为0，则以伤害时间为准")]
    public float trap_atk_period;

    [EditorName("陷阱特效")]
    public config_action_play_fx fx = new config_action_play_fx();

    [EditorName("陷阱生命周期"), EditorDescription("单位：秒")]
    public float life_time;

    [EditorName("陷阱伤害"), EditorDescription("真实伤害")]
    public int dmg;
}

public class config_action_lost_hp
{
    [EditorName("损失当前生命值比率")]
    public float lost_hp_rate;
}

public class config_action_fz_dmg_hp_cost_mp
{
    [EditorName("伤害能量系数")]
    public float mp_factor;
    [EditorName("战斗力系数")]
    public float atk_power_factor;
}

public class config_action_attack_hp_cost_mp
{
    [EditorName("MP消耗")]
    public float cost_mp_rate;
    [EditorName("伤害系数"), EditorDescription("亿损失生命值的系数")]
    public float x;
}

public class config_action_drift_target_delay
{
    [EditorName("瞬移帧数")]
    public int drift_frame;
    public Quad[] box;
    [EditorName("目标类型")]
    public TargetType target_type;

    [EditorName("预兆半径")]
    public float show_range_r;
    [EditorName("预兆底图颜色")]
    public Color show_range_color_bg = new Color(1, 1, 1, 1);
    [EditorName("预兆扩散颜色")]
    public Color show_range_color_fg = new Color(1, 1, 1, 1);
}

public class config_action_recover_hp_cost_mp
{
    [EditorName("mp消耗回复hp的比率")]
    public float hp_recover_factor;
}

public class config_action_recover_shield
{
    [EditorName("能量回复百分比")]
    public float factor;        //回复百分比
    [EditorName("能量回复增量")]
    public int plus;           //回复量
}

public class config_action_block
{
    [EditorName("格挡角度")]
    public float angle;
}

public class config_action_show_counsellor
{
    [EditorName("军师资源名称")]
    public string name;
    [EditorName("淡入时间")]
    public float fade_in = 0.2f;
    [EditorName("淡出时间")]
    public float fade_out = 0.2f;
    [EditorName("顿帧数")]
    public int hit_stop_frame;
}

public class config_action_frame_state
{
    public enum StateType
    {
        START,  //起手
        FLAW,    //破绽
        PA_BODY,     //霸体时间
        NO_HIT,   //不可被攻击
        SKILL_REAR, //技能收尾
    }

    [EditorName("状态")]
    public StateType state;
}

public class config_action_qte
{
    [EditorName("阶段2起始帧")]
    public int step2_frame;
}


public class config_action_screen_light
{
    [EditorName("屏幕底色")]
    public Color light_color = new Color(1, 1, 1, 0.7f);
    [EditorName("是否其他玩家可见，1可")]
    public int showToOthers = 1;
    [EditorName("淡入帧数")]
    public int fadeInFrame = 5;
    [EditorName("淡出帧数")]
    public int fadeOutFrame = 3;
    [EditorName("显示在最上层")]
    public bool showAtTop = false;
}

public class config_action_attract
{
    [EditorName("吸引框")]
    public Quad[] box;
    [EditorName("吸引速度")]
    public float speed;
    [EditorName("吸引目标偏移"), EditorDescription("相对于施法者")]
    public Vector3 offset = new Vector3();
}


public class config_action_charge
{
    [EditorName("阶段1")]
    public int frame1;
    [EditorName("阶段2")]
    public int frame2;

    [EditorName("跳转1")]
    public int next_step_frame1;
    [EditorName("跳转2")]
    public int next_step_frame2;
    [EditorName("跳转3")]
    public int next_step_frame3;
}

public class config_action_show_range
{
    public enum RangeType
    {
        Circle,
        Rectangle
    }
    [EditorName("范围类型")]
    public RangeType range_type;
    public Quad[] box;
    [EditorName("底图颜色")]
    public Color bg_color = new Color(1, 0, 0, 0.5f);
    [EditorName("扩散颜色")]
    public Color fg_color = new Color(1, 0, 0, 0.2f);
}


public class BuffData
{
    public int id;
    [EditorName("几率")]
    public float rate = 1;
    [EditorName("增加")]
    public bool add = true;
}

public class config_action_buff
{
    public Quad[] box;
    [EditorName("目标类型")]
    public TargetType target_type;
    [EditorName("BUFF增加")]
    public BuffData[] buffs;
}

public class config_action_drift_target
{
    public Quad[] box;
    [EditorName("目标类型")]
    public TargetType target_type;
    [EditorName("目标偏移")]
    public Vector3 target_pos = new Vector3();
    [EditorName("自身偏移")]
    public Vector3 self_pos = new Vector3();
    [EditorName("无视碰撞")]
    public bool NO_HIT;
    [EditorName("延时瞬移")]
    public bool delay;
}

public class config_action_jump
{
    public enum Condition
    {
        None,   //无条件跳转
        HIT_ENEMY,
    }
    [EditorName("条件")]
    public Condition condition;
    [EditorName("判定框")]
    public Quad[] box;
    [EditorName("真"), EditorDescription("true 条件为真则跳转，为假则不跳转, false 条件为真则不跳转，为假则跳转")]
    public bool true_type = true;
    [EditorName("目标类型")]
    public TargetType target_type;

}

public class config_action_can_move
{
    [EditorName("移动速度修正")]
    public float move_speed_correct;
}

public class config_action_can_rotate
{
    [EditorName("旋转角速度修正")]
    public float angle_speed_correct;
}

public class config_action_loop
{
    [EditorName("消耗MP")]
    public int cost_mp;
    [EditorName("循环最大次数")]
    public int times;
}

public class config_action_summon
{
    public enum Obj_Type
    {
        Trap,
        Monster
    }
    [EditorName("召唤类型")]
    public Obj_Type obj_type;


    public enum SUMMON_TYPE
    {
        ALL_ENEMY,
        RANDOM_ENEMY,
        RANDOM_POS,
        NEAREST_ONE,
        ALL_PARTY,
        RANDOM_PARTY,
        SELF
    }

    [EditorName("召唤方式")]
    public SUMMON_TYPE summon_type;
    [EditorName("召唤数量")]
    public int number = 1;
    [EditorName("召唤区域")]
    public Quad[] box;
    [EditorName("怪物ID")]
    public int obj_id;
    [EditorName("陷阱伤害触发间隔")]
    public float[] damage_times;
    [EditorName("陷阱伤害参数")]
    public config_action_attack atk;
    [EditorName("陷阱特效")]
    public config_action_play_fx fx;
    [EditorName("陷阱默认位置"), EditorDescription("NEAREST_ONE 专用，如果范围内没有目标，选择此偏移为陷阱目标")]
    public Vector2 defaultPos = new Vector2();
    [EditorName("陷阱攻击特效")]
    public config_action_play_fx trap_fx_attack;
    [EditorName("陷阱预兆半径")]
    public float show_range_r;
    [EditorName("陷阱预兆底图颜色")]
    public Color show_range_color_bg = new Color(1, 1, 1, 1);
    [EditorName("陷阱预兆扩散颜色")]
    public Color show_range_color_fg = new Color(1, 1, 1, 1);
    [EditorName("陷阱预兆帧数")]
    public int show_range_frame_count;
    [EditorName("陷阱攻击震动")]
    public int attack_shake_id;
    [EditorName("陷阱吸引")]
    public config_action_attract[] attract;
    [EditorName("陷阱攻击频率"), EditorDescription("如果这个值为0，则以伤害时间为准")]
    public float trap_atk_period;
    [EditorName("陷阱跟随")]
    public bool trap_follow_target;

    public config_action_summon()
    {
        atk = new config_action_attack();
        fx = new config_action_play_fx();
        trap_fx_attack = new config_action_play_fx();
    }
}


public class config_action_ghost
{
    public float interval;  //周期
    public float existTime; //存在时间
}

public class config_projectile
{
    [EditorName("挂载点"), EditorDescription("弹道初始位置")]
    public string mountPoint;
    [EditorName("发射点"), EditorDescription("相对于挂载点的弹道发射点，如果没有，则相对于玩家坐标，X为左右，Y为上下，Z为前后")]
    public Vector3 offset;
    [EditorName("速度方向"), EditorDescription("相对于玩家方向的弹道飞行速度，X为左右，Y为上下，Z为前后")]
    public Vector3 velocity;
    [EditorName("重力加速度")]
    public Vector3 g = new Vector3();

    [EditorName("飞行特效")]
    public string fx_projectile;

    [EditorName("生命周期"), EditorDescription("生命周期结束后爆炸")]
    public float life_time;
    [EditorName("最大连击数"), EditorDescription("最大连击数结束后爆炸")]
    public int max_hit_count;
    [EditorName("落地爆炸")]
    public bool explode_on_ground;
    [EditorName("目标刷新间隔帧"), EditorDescription("目标可以被再次攻击的间隔帧数，0表示不可被再次攻击")]
    public int hit_refresh_frame;

    public enum TrackType
    {
        Line,
        Parabola,
        Curve
    }

    [EditorName("弹道框")]
    public Quad[] box;
    [EditorName("目标类型")]
    public TargetType target_type;

    public class Event
    {
        public enum EventType
        {
            Hit,
            Explode
        }
        [EditorName("触发时机")]
        public EventType type;
        [EditorName("特效")]
        public config_action_play_fx fx = new config_action_play_fx();
        [EditorName("声效")]
        public int sound;
        [EditorName("攻击参数")]
        public config_action_attack atk = new config_action_attack();
        [EditorName("召唤参数")]
        public config_action_summon[] summons;
    }

    [EditorName("事件")]
    public Event[] events;


    [EditorName("预兆半径")]
    public float show_range_r;
    [EditorName("预兆背景色")]
    public Color show_range_color_bg = new Color(1, 1, 1, 1);
    [EditorName("预兆前景色")]
    public Color show_range_color_fg = new Color(1, 1, 1, 1);


    public config_projectile()
    {
        offset = new Vector3();
        velocity = new Vector3();
    }
}


public class Quad
{
    //public Vector2 dir = Vector2.up;
    public Vector2 a;
    public Vector2 b;
    public Vector2 c;
    public Vector2 d;
    public Quad()
    {
        a = new Vector2();
        b = new Vector2();
        c = new Vector2();
        d = new Vector2();
    }

    public bool PointIn(Vector2 pt)
    {
        float r1 = Vector2.Dot(b - a, pt - a);
        float r2 = Vector2.Dot(c - b, pt - b);
        float r3 = Vector2.Dot(d - c, pt - c);
        float r4 = Vector2.Dot(a - d, pt - d);
        return r1 > 0 && r2 > 0 && r3 > 0 && r4 > 0 || r1 < 0 && r2 < 0 && r3 < 0 && r4 < 0;
    }

    bool Contain(Quad box)
    {
        return PointIn(box.a) || PointIn(box.b) || PointIn(box.c) || PointIn(box.d);
    }

    bool IntersectOnLine(Vector2 a, Vector2 b, Quad box)
    {
        float kMin = 0;
        float kMax = 0;
        KMaxMin(a, b, out kMin, out kMax);

        float kMin1 = 0;
        float kMax1 = 0;
        box.KMaxMin(a, b, out kMin1, out kMax1);

        return kMax > kMin1 && kMin < kMax1 || kMax1 > kMin && kMin1 < kMax;
    }

    public bool Overlap(Quad box)
    {
        //return Contain(box) || box.Contain(this);
        return IntersectOnLine(a, b, box) && IntersectOnLine(b, c, box) && IntersectOnLine(box.a, box.b, box) && IntersectOnLine(box.a, box.b, box);
    }


    public Quad Transform(Matrix4x4 mat)
    {
        Quad world = new Quad();
        Vector3 a3 = mat.MultiplyPoint(new Vector3(a.x, 0, a.y));
        world.a = new Vector2(a3.x, a3.z);
        Vector3 b3 = mat.MultiplyPoint(new Vector3(b.x, 0, b.y));
        world.b = new Vector2(b3.x, b3.z);
        Vector3 c3 = mat.MultiplyPoint(new Vector3(c.x, 0, c.y));
        world.c = new Vector2(c3.x, c3.z);
        Vector3 d3 = mat.MultiplyPoint(new Vector3(d.x, 0, d.y));
        world.d = new Vector2(d3.x, d3.z);
        //Vector3 dir3 = mat.MultiplyVector(new Vector3(dir.x, 0, dir.y));
        //world.dir = new Vector2(dir3.x, dir3.z);

        return world;
    }

    void KMaxMin(Vector2 P1, Vector2 P2, out float min, out float max)
    {
        float kMin = 0;
        float kMax = 0;

        float k1 = KPoint(P1, P2, a);
        float k2 = KPoint(P1, P2, b);
        float k3 = KPoint(P1, P2, c);
        float k4 = KPoint(P1, P2, d);

        kMin = Math.Min(Math.Min(Math.Min(k1, k2), k3), k4);
        kMax = Math.Max(Math.Max(Math.Max(k1, k2), k3), k4);
        min = kMin;
        max = kMax;
    }
    float KPoint(Vector2 P1, Vector2 P2, Vector2 P3)
    {
        float length = (P2 - P1).magnitude;
        float k = Vector2.Dot((P3 - P1), (P2 - P1)) / (length * length);
        return k;
    }


    //默认第一条边的方向为矩形方向
    public Vector2 dir
    {
        get
        {
            return (b - a).normalized;
        }
    }

    public Vector3 dir3d
    {
        get
        {
            return new Vector3(dir.x, 0, dir.y);
        }
    }

    public float width
    {
        get { return (b - a).magnitude; }
    }
    public float height
    {
        get { return (c - b).magnitude; }
    }
    public Vector2 center
    {
        get { return a + (c - a) / 2; }
    }
    public Vector3 center3d
    {
        get
        {
            return new Vector3(center.x, 0, center.y);
        }
    }
}

public enum TargetType
{
    Alive_Enemy,
    Alive_Party,
    Dead_Party,
    Alive_All,
    Self
}



public class config_action_attack
{
    [EditorName("目标类型")]
    public TargetType target_type;
    [EditorName("攻击强度")]
    public int atk_power;   //攻击强度
    [EditorName("声效")]
    public int hit_sound_id;    //声效ID
    [EditorName("伤害类型")]
    public int dmg_type;        //伤害类型0物理，1魔法
    [EditorName("硬直持续时间")]
    public float hit_recover_time;  //硬直持续时间
    [EditorName("击退时间")]
    public float hit_back_time;     //击退时间
    [EditorName("击退速度")]
    public float hit_back_speed;    //击退速度
    [EditorName("顿帧数")]
    public int hit_stop_time;     //顿帧时间
    [EditorName("受击特效")]
    public config_action_play_fx fx_hit = new config_action_play_fx();
    [EditorName("扇形框")]
    public float[] attack_fan;//[X左右偏移,Y前后偏移,扇形角度,扇形半径]
    [EditorName("攻击框")]
    public Quad[] box;
    [EditorName("破盾值")]
    public int dmg_shield;
    [EditorName("计算背击")]
    public bool cal_hit_back = true;
    [EditorName("破坏场景物件")]
    public bool hit_scene_prop = false;
    [EditorName("段伤害系数，千分比")]
    public int dam_section = 1000;
    [EditorName("BUFF增加")]
    public BuffData[] buffs;

    public enum HitEffect
    {
        HitBack,
        HitFly,
        None
    }
    [EditorName("击中效果_地面")]
    public HitEffect ground_hit_effect = HitEffect.HitBack;
    [EditorName("击中效果_空中")]
    public HitEffect fly_hit_effect = HitEffect.HitFly;
    [EditorName("击中效果_死亡")]
    public HitEffect dead_hit_effect = HitEffect.HitFly;

    [EditorName("击飞ID")]
    public int[] hit_fly_ids = new int[3] { 1, 2, 3 };

    [EditorName("秒杀概率")]
    public float kill_rate;
    [EditorName("秒杀特效")]
    public config_action_play_fx kill_fx = new config_action_play_fx() { lifetime = 1, follow = false, fx = "seckillbcross", mountPoint = "Heart_Point00" };

    [EditorName("斩杀生命比率")]
    public float execute_rate;
    [EditorName("斩杀伤害值上限")]
    public int execute_max;


    public enum HitBackDir
    {
        AsOwnerDir, //与攻击者方向一致
        AsOwner,    //与攻击者连线方向一致
        AsBoxCenter //与攻击框中心点连线方向一致
    }

    [EditorName("击退方向"), EditorDescription("AsOwnerDir:与攻击者方向一致\r\nAsOwner:与攻击者连线方向一致\r\nAsBoxCenter:与攻击框中心点连线方向一致")]
    public HitBackDir hit_back_dir = HitBackDir.AsOwnerDir;
}

//击飞参数
public class HitFly_Phase
{
    public int id;
    [EditorName("速度")]
    public Vector3 velocity = new Vector3();
    [EditorName("重力加速度")]
    public float g;
    [EditorName("时间")]
    public float time;
}

public class config_action_play_fx
{
    [EditorName("特效名")]
    public string fx;
    [EditorName("是否跟随")]
    public bool follow = true;
    [EditorName("生命周期")]
    public float lifetime = 3.0f;
    [EditorName("挂载点")]
    public string mountPoint;
    [EditorName("偏移")]
    public Vector3 offset = new Vector3();
    [EditorName("打断删除")]
    public bool interrupt_delete = true;
    [EditorName("结束删除")]
    public bool end_delete;
    [EditorName("挂载到相机")]
    public bool mountCamera;
    [EditorName("相机朝向")]
    public bool faceCamera;
}

public class config_action_play_fx_link_target
{
    [EditorName("特效名")]
    public string fx;
    [EditorName("特效tiling速度")]
    public Vector2 tiling_speed = new Vector2();
    [EditorName("特效offset速度")]
    public Vector2 offset_speed = new Vector2();
    [EditorName("自身挂载点")]
    public string self_mountPoint;
    [EditorName("自身偏移")]
    public Vector3 self_offset = new Vector3();
    [EditorName("目标挂载点")]
    public string target_mountPoint;
    [EditorName("目标偏移")]
    public Vector3 target_offset = new Vector3();
    public Quad[] box;
    [EditorName("目标类型")]
    public TargetType target_type;

    [EditorName("伤害触发帧")]
    public int[] damage_frame;

    [EditorName("攻击参数")]
    public config_action_attack atk = new config_action_attack();
}

public class config_action_play_sound
{
    public int sound_id;
}

public class config_action_drift
{
    [EditorName("方向")]
    public int dir; //0前移，1后移
    [EditorName("位移速度")]
    public float speed; //位移速度
}

public class config_action_shake
{
    public int shake_id;
}
public class config_action_shoot
{
    [EditorName("弹道参数")]
    public config_projectile[] projectiles;

    [EditorName("目标区域(可选)")]
    public Quad[] box;

    public enum TARGET_TYPE
    {
        ALL_ENEMY,
        RANDOM_ENEMY,
        RANDOM_POS,
        NEAREST_ONE
    }
    [EditorName("目标类型")]
    public TARGET_TYPE target_pos_type;

    [EditorName("追踪目标")]
    public bool follow_target;
    [EditorName("目标矫正旋转速度")]
    public float follow_target_angle_speed;
    [EditorName("目标矫正旋转加速度")]
    public float follow_target_acc_angle_speed;
    [EditorName("延时追踪"), EditorDescription("单位：千分秒")]
    public int follow_target_delay_time;
}

public class config_action_play_anim
{
    [EditorName("名称")]
    public string name;
    [EditorName("循环")]
    public int loop; //是否循环
    [EditorName("速度")]
    public float speed = 1.0f;
}
public class config_action_move
{
    public Vector3 from = new Vector3();
    public Vector3 to = new Vector3();
    public float acc;
    public float time;
}

public class config_action_variant
{
    [EditorName("变身目标id")]
    public int monsterId;
}

public class config_action_monster_com
{
    public class condition
    {
        [EditorName("合体怪物ID列表")]
        public int[] mons;
        [EditorName("合体目标怪物id")]
        public int target_monster_id;
    }
    [EditorName("条件")]
    public condition[] conds;
    [EditorName("区域")]
    public Quad[] box;
}

public class config_action_light
{
    [EditorName("相对坐标")]
    public Vector3 offset = new Vector3();
    [EditorName("颜色")]
    public Color lightColor = new Color();
    [EditorName("范围")]
    public int range;
    [EditorName("强度")]
    public int intensity;
    [EditorName("持续时间")]
    public float duration;
}

public class config_skill_frame
{
    public int skill_id;
    [EditorName("触发时间")]
    public float time;
    [EditorName("触发时长")]
    public float time_length;  //持续时间
    [EditorName("动作")]
    public SkillFrameAction action;

    public int frame;
    public int frame_length;

    public config_action_attack attack;
    public config_action_move move;
    public config_action_play_fx play_fx;
    public config_action_play_sound play_sound;
    public config_action_drift drift;
    public config_action_shake shake;
    public config_action_shoot shoot;
    public config_action_play_anim play_anim;
    public config_action_ghost ghost;
    public config_action_summon summon;
    public config_action_loop loop;
    public config_action_can_move can_move;
    public config_action_can_rotate can_rotate;
    public config_action_variant variant;
    public config_action_jump jump;
    public config_action_drift_target drift_target;
    public config_action_buff buff;
    public config_action_show_range show_range;
    public config_action_charge charge;
    public config_action_play_fx_link_target fx_link_target;
    public config_action_monster_com monster_com;
    public config_action_attract attract;
    public config_action_screen_light screen_light;
    public config_action_frame_state frame_state;
    public config_action_qte qte;
    public config_action_show_counsellor show_counsellor;
    public config_action_block block;
    public config_action_recover_shield recover_shiled;
    public config_action_light light;
    public config_action_recover_hp_cost_mp recover_hp_cost_mp;
    public config_action_drift_target_delay drift_target_delay;
    public config_action_attack_hp_cost_mp attack_hp_cost_mp;
    public config_action_fz_dmg_hp_cost_mp fz_dmg_hp_cost_mp;
    public config_action_lost_hp lost_hp;
    public config_action_summon_attack_words summon_attack_words;
}

