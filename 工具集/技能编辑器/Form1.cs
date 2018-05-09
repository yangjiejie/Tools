using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;


namespace 技能编辑器
{
    public partial class Form1 : Form
    {
        public static string table_path = "";

        public config_skill _selected_skill;

        string history_file_path = Application.StartupPath + "/history.cfg";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            panel_Root.Enabled = false;
            //事件通知绑定
            frame1.onSelectFrame = OnSelectFrame;

            //自动打开上次文件
            if (File.Exists(history_file_path))
            {
                OpenFile(File.ReadAllText(history_file_path));
            }
                
        }

        //private void panel1_Paint(object sender, PaintEventArgs e)
        //{


        //}

        private void listBox_skills_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBoxItem_skill item = listBox_skills.SelectedItem as ListBoxItem_skill;
            if(item!=null)
            {
                _selected_skill = item.skill;
                frame1.UpdateSkill(item.skill);
                DrawProperty(item.skill);
                
            }
            else
            {
                _selected_skill = null;
                frame1.UpdateSkill(null);
                propertyGrid1.SelectedObject = null;
            }
            boxView1.listener = (null);
        }

        void DrawProperty(object obj)
        {
            if(obj!=null)
            {
                XProps xps = new XProps(obj);
                propertyGrid1.SelectedObject = xps;
            }
            else
            {
                propertyGrid1.SelectedObject = null;
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            XProps xps = propertyGrid1.SelectedObject as XProps;
            if(xps!=null && xps._obj is config_skill)
            {
                frame1.UpdateSkill(_selected_skill);
            }
        }

        void RefreshListBox_skill()
        {
            listBox_skills.Items.Clear();
            listBox_skills.BeginUpdate();
            foreach (var s in Data.skills)
            {
                listBox_skills.Items.Add(new ListBoxItem_skill(s.Value));
            }
            listBox_skills.EndUpdate();

            listBox_skills.SelectedIndex = 0;
        }
        
        //void Pant()
        //{
        //    var categorysinfo = propertyGrid1.SelectedObject.GetType().GetField("categorys", BindingFlags.NonPublic | BindingFlags.Instance);
        //    if (categorysinfo != null)
        //    {
        //        var categorys = categorysinfo.GetValue(propertyGrid1.SelectedObject) as List<String>;
        //        propertyGrid1.CollapseAllGridItems();
        //        GridItemCollection currentPropEntries = propertyGrid1.GetType().GetField("currentPropEntries", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(propertyGrid1) as GridItemCollection;
        //        var newarray = currentPropEntries.Cast<GridItem>().OrderBy((t) => categorys.IndexOf(t.Label)).ToArray();
        //        currentPropEntries.GetType().GetField("entries", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(currentPropEntries, newarray);
        //        propertyGrid1.ExpandAllGridItems();
        //        propertyGrid1.PropertySort = (PropertySort)propertyGrid1.Tag;
        //    }
        //    //propertyGrid1.Paint -= new PaintEventHandler(propertyGrid1);

        //}


        private void OnSelectFrame(int frame)
        {
            if(frame == -1)
            {
                DrawProperty(null);
                boxView1.listener = (null);
                return;
            }

            config_skill_frame selected_frame = _selected_skill.frames[frame];
            switch (selected_frame.action)
            {
                case SkillFrameAction.ATTACK:
                    if (selected_frame.attack == null)
                        selected_frame.attack = new config_action_attack();

                    DrawProperty(selected_frame.attack);
                    break;
                case SkillFrameAction.DRIFT:
                    if (selected_frame.drift == null)
                        selected_frame.drift = new config_action_drift();
                    DrawProperty(selected_frame.drift);
                    break;
                case SkillFrameAction.FLAW:
                    DrawProperty(null);
                    break;
                case SkillFrameAction.GHOST:
                    if (selected_frame.ghost == null)
                        selected_frame.ghost = new config_action_ghost();
                    DrawProperty(selected_frame.ghost);
                    break;
                case SkillFrameAction.MOVE:
                    if (selected_frame.move == null)
                        selected_frame.move = new config_action_move();
                    DrawProperty(selected_frame.move);
                    break;
                case SkillFrameAction.None:
                    DrawProperty(null);
                    break;
                case SkillFrameAction.PLAY_ANIM:
                    if (selected_frame.play_anim == null)
                        selected_frame.play_anim = new config_action_play_anim();
                    DrawProperty(selected_frame.play_anim);
                    break;
                case SkillFrameAction.PLAY_FX:
                    if (selected_frame.play_fx == null)
                        selected_frame.play_fx = new config_action_play_fx();
                    DrawProperty(selected_frame.play_fx);
                    break;
                case SkillFrameAction.PLAY_SOUND:
                    if (selected_frame.play_sound == null)
                        selected_frame.play_sound = new config_action_play_sound();
                    DrawProperty(selected_frame.play_sound);
                    break;
                case SkillFrameAction.SHAKE:
                    if (selected_frame.shake == null)
                        selected_frame.shake = new config_action_shake();
                    DrawProperty(selected_frame.shake);
                    break;
                case SkillFrameAction.SHOOT:
                    if (selected_frame.shoot == null)
                        selected_frame.shoot = new config_action_shoot();
                    DrawProperty(selected_frame.shoot);
                    break;
                case SkillFrameAction.START:
                    DrawProperty(null);
                    break;
                case SkillFrameAction.SUMMON:
                    if (selected_frame.summon == null)
                        selected_frame.summon = new config_action_summon();
                    DrawProperty(selected_frame.summon);
                    break;
                case SkillFrameAction.LOOP_FRAME:
                    if (selected_frame.loop == null)
                        selected_frame.loop = new config_action_loop();
                    DrawProperty(selected_frame.loop);
                    break;
                case SkillFrameAction.CAN_MOVE:
                    if (selected_frame.can_move == null)
                        selected_frame.can_move = new config_action_can_move();
                    DrawProperty(selected_frame.can_move);
                    break;
                case SkillFrameAction.CAN_ROTATE:
                    if (selected_frame.can_rotate == null)
                        selected_frame.can_rotate = new config_action_can_rotate();
                    DrawProperty(selected_frame.can_rotate);
                    break;
                case SkillFrameAction.VARIANT:
                    if (selected_frame.variant == null)
                        selected_frame.variant = new config_action_variant();
                    DrawProperty(selected_frame.variant);
                    break;
                case SkillFrameAction.JUMP_FRAME:
                    if (selected_frame.jump == null)
                        selected_frame.jump = new config_action_jump();
                    DrawProperty(selected_frame.jump);
                    break;
                case SkillFrameAction.DRIFT_TARGET:
                    if (selected_frame.drift_target == null)
                        selected_frame.drift_target = new config_action_drift_target();
                    DrawProperty(selected_frame.drift_target);
                    break;
                case SkillFrameAction.BUFF:
                    if (selected_frame.buff == null)
                        selected_frame.buff = new config_action_buff();
                    DrawProperty(selected_frame.buff);
                    break;
                case SkillFrameAction.SHOW_RANGE:
                    if (selected_frame.show_range == null)
                        selected_frame.show_range = new config_action_show_range();
                    DrawProperty(selected_frame.show_range);
                    break;
                case SkillFrameAction.CHARGE:
                    if (selected_frame.charge == null)
                        selected_frame.charge = new config_action_charge();
                    DrawProperty(selected_frame.charge);
                    break;
                case SkillFrameAction.PLAY_FX_LINK_TARGET:
                    if (selected_frame.fx_link_target == null)
                        selected_frame.fx_link_target = new config_action_play_fx_link_target();
                    DrawProperty(selected_frame.fx_link_target);
                    break;
                case SkillFrameAction.MONSTER_COMBINATION:
                    if (selected_frame.monster_com == null)
                        selected_frame.monster_com = new config_action_monster_com();
                    DrawProperty(selected_frame.monster_com);
                    break;
                case SkillFrameAction.ATTRACTION:
                    if (selected_frame.attract == null)
                        selected_frame.attract = new config_action_attract();
                    DrawProperty(selected_frame.attract);
                    break;
                case SkillFrameAction.SCREEN_LIGHT:
                    if (selected_frame.screen_light == null)
                        selected_frame.screen_light = new config_action_screen_light();
                    DrawProperty(selected_frame.screen_light);
                    break;
                case SkillFrameAction.FRAME_STATE:
                    if (selected_frame.frame_state == null)
                        selected_frame.frame_state = new config_action_frame_state();
                    DrawProperty(selected_frame.frame_state);
                    break;
                case SkillFrameAction.QTE:
                    if (selected_frame.qte == null)
                        selected_frame.qte = new config_action_qte();
                    DrawProperty(selected_frame.qte);
                    break;
                case SkillFrameAction.SHOW_COUNSELLOR:
                    if (selected_frame.show_counsellor == null)
                        selected_frame.show_counsellor = new config_action_show_counsellor();
                    DrawProperty(selected_frame.show_counsellor);
                    break;
                case SkillFrameAction.BLOCK:
                    if (selected_frame.block == null)
                        selected_frame.block = new config_action_block();
                    DrawProperty(selected_frame.block);
                    break;
                case SkillFrameAction.RECOVE_SHIELD:
                    if (selected_frame.recover_shiled == null)
                        selected_frame.recover_shiled = new config_action_recover_shield();
                    DrawProperty(selected_frame.recover_shiled);
                    break;
                case SkillFrameAction.LIGHT:
                    if (selected_frame.light == null)
                        selected_frame.light = new config_action_light();
                    DrawProperty(selected_frame.light);
                    break;
                case SkillFrameAction.RECOVER_HP_COST_MP:
                    if (selected_frame.recover_hp_cost_mp == null)
                        selected_frame.recover_hp_cost_mp = new config_action_recover_hp_cost_mp();
                    DrawProperty(selected_frame.recover_hp_cost_mp);
                    break;
                case SkillFrameAction.DRIFT_TARGET_DELAY:
                    if (selected_frame.drift_target_delay == null)
                        selected_frame.drift_target_delay = new config_action_drift_target_delay();
                    DrawProperty(selected_frame.drift_target_delay);
                    break;
                case SkillFrameAction.ATTACK_HP_COST_MP:
                    if (selected_frame.attack_hp_cost_mp == null)
                        selected_frame.attack_hp_cost_mp = new config_action_attack_hp_cost_mp();
                    DrawProperty(selected_frame.attack_hp_cost_mp);
                    break;
                case SkillFrameAction.FAZHENG_DMG_HP_COST_MP:
                    if (selected_frame.fz_dmg_hp_cost_mp == null)
                        selected_frame.fz_dmg_hp_cost_mp = new config_action_fz_dmg_hp_cost_mp();
                    DrawProperty(selected_frame.fz_dmg_hp_cost_mp);
                    break;
                case SkillFrameAction.LOST_HP:
                    if (selected_frame.lost_hp == null)
                        selected_frame.lost_hp = new config_action_lost_hp();
                    DrawProperty(selected_frame.lost_hp);
                    break;
                case SkillFrameAction.SUMMON_ATTACK_WARDS:
                    if (selected_frame.summon_attack_words == null)
                        selected_frame.summon_attack_words = new config_action_summon_attack_words();
                    DrawProperty(selected_frame.summon_attack_words);
                    break;
                default:
                    DrawProperty(null);
                    break;
            }



            //攻击框
            if(selected_frame.action == SkillFrameAction.ATTACK)
            {
                //刷新攻击框
                boxView1.listener = new FieldBoxEvent(selected_frame.attack);
            }
            //召唤区域
            else if (selected_frame.action == SkillFrameAction.SUMMON)
            {
                //刷新攻击框
                boxView1.listener = new FieldBoxEvent(selected_frame.summon);
            }
            else if(selected_frame.action == SkillFrameAction.JUMP_FRAME)
            {
                //刷新攻击框
                boxView1.listener = new FieldBoxEvent(selected_frame.jump);
            }
            else if(selected_frame.action == SkillFrameAction.DRIFT_TARGET)
            {
                boxView1.listener = new FieldBoxEvent(selected_frame.drift_target);
            }
            else if(selected_frame.action == SkillFrameAction.BUFF)
            {
                boxView1.listener = new FieldBoxEvent(selected_frame.buff);
            }
            else if(selected_frame.action == SkillFrameAction.SHOW_RANGE)
            {
                boxView1.listener = new FieldBoxEvent(selected_frame.show_range);
            }
            else if(selected_frame.action == SkillFrameAction.PLAY_FX_LINK_TARGET)
            {
                boxView1.listener = new FieldBoxEvent(selected_frame.fx_link_target);
            }
            else if(selected_frame.action == SkillFrameAction.SHOOT)
            {
                boxView1.listener = new FieldBoxEvent(selected_frame.shoot);
            }
            else if(selected_frame.action == SkillFrameAction.MONSTER_COMBINATION)
            {
                boxView1.listener = new FieldBoxEvent(selected_frame.monster_com);
            }
            else if(selected_frame.action == SkillFrameAction.ATTRACTION)
            {
                boxView1.listener = new FieldBoxEvent(selected_frame.attract);
            }
            else if(selected_frame.action == SkillFrameAction.DRIFT_TARGET_DELAY)
            {
                boxView1.listener = new FieldBoxEvent(selected_frame.drift_target_delay);
            }
                else if(selected_frame.action == SkillFrameAction.SUMMON_ATTACK_WARDS)
            {
                boxView1.listener = new FieldBoxEvent(selected_frame.summon_attack_words);
            }
            else
            {
                boxView1.listener = (null);
            }


            splitContainer1.Panel2.Enabled = boxView1.listener != null;
            
        }

        
        class FieldBoxEvent:IBoxViewEvent
        {
            FieldInfo _fi_box;
            object _obj;
            public FieldBoxEvent(object obj)
            {
                _obj = obj;
                _fi_box = _obj.GetType().GetField("box");
            }
            public void onAddBox(BoxView bv)
            {
                _fi_box.SetValue(_obj, bv.box);
            }
            public void onBoxChanged(BoxView bv, Quad q)
            {

            }
            public void onRemoveBox(BoxView bv)
            {
                _fi_box.SetValue(_obj, bv.box);
            }

            public Quad[] GetBoxList()
            {
                return ( Quad[])_fi_box.GetValue(_obj);
            }
        }

        private void tsmi_OpenFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.SelectedPath = Application.StartupPath;

            DialogResult dr = folderDlg.ShowDialog();
            if(dr == System.Windows.Forms.DialogResult.OK)
            {
                OpenFile(folderDlg.SelectedPath);
            }
        }

        void OpenFile(string path)
        {
            try
            {
                table_path = path;
                Data.Load();
                panel_Root.Enabled = true;
                RefreshListBox_skill();
            }
            catch (Exception except)
            {
                Console.WriteLine(except.StackTrace);
            }
        }


        void WriteFileUtf8WithoutBom(string fileName,string txt)
        {
            var utf8WithoutBom = new System.Text.UTF8Encoding(false);
            using (var sink = new StreamWriter(fileName, false, utf8WithoutBom))
            {
                sink.Write(txt);
            }
        }

        private void tsmi_Save_Click(object sender, EventArgs e)
        {
            {
                List<config_skill_frame> frames = new List<config_skill_frame>();
                //收集所有关键帧数据
                foreach(var d in Data.skills)
                {
                    if(d.Value.frames!=null)
                    {
                        frames.AddRange(d.Value.frames);
                        foreach(var f in d.Value.frames)
                        {
                            f.skill_id = d.Value.id;
                        }
                    }
                }
                fastJSON.JSON.Parameters.UseExtensions = false;
                WriteFileUtf8WithoutBom(table_path + "/Skill/skill_frame.csv",Core.CsvWriter.WriteCsvTable(frames, Encoding.UTF8));
            }

            {
                List<config_skill> skills = new List<config_skill>();
                foreach(var d in Data.skills)
                {
                    skills.Add(d.Value);
                }

                WriteFileUtf8WithoutBom(table_path + "/Skill/skill.csv", Core.CsvWriter.WriteCsvTable(skills, Encoding.UTF8));
            }



            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!string.IsNullOrEmpty(table_path))
                File.WriteAllText(history_file_path, table_path);
        }

        private void lbskill_tsmi_Refresh_Click(object sender, EventArgs e)
        {
            RefreshListBox_skill();
        }

        private void lbskill_tsmi_Add_Click(object sender, EventArgs e)
        {
            Form_NewSkill skillDlg = new Form_NewSkill();
            if(skillDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Data.skills.ContainsKey(skillDlg.id))
                {
                    MessageBox.Show("重复的ID");
                    return;
                }
                    

                config_skill skill = new config_skill();
                skill.id = skillDlg.id;
                skill.name = skillDlg.name;
                Data.skills.Add(skill.id, skill);
                listBox_skills.Items.Add(new ListBoxItem_skill(skill));
            }
        }

        private void lbskill_tsmi_Delete_Click(object sender, EventArgs e)
        {
            if(listBox_skills.SelectedIndex!=-1)
            {
                Data.skills.Remove(_selected_skill.id);
                RefreshListBox_skill();
            }
        }

        private void trackBar_BoxViewSize_Scroll(object sender, EventArgs e)
        {
            float value = trackBar_BoxViewSize.Value-5;

            value = value * 0.1f;

            value += 1.0f;

            label_BoxViewSize.Text = string.Format("{0}%", value * 100);

            boxView1.scale = value;
            boxView1.Refresh();
        }

        private void checkBox_Align_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown_align.Enabled = checkBox_Align.Checked;

            boxView1.snap_gird = checkBox_Align.Checked;
            boxView1.snap_size = (float)numericUpDown_align.Value;
        }

        private void numericUpDown_align_ValueChanged(object sender, EventArgs e)
        {
            boxView1.snap_size = (float)numericUpDown_align.Value;
        }

        private void tsmi_frame_copy_Click(object sender, EventArgs e)
        {
            if(frame1.selected_frame!=null)
            {
                config_skill_frame new_frame = fastJSON.JSON.ToObject<config_skill_frame>(fastJSON.JSON.ToJSON(frame1.selected_frame));
                List<config_skill_frame> newList = new List<config_skill_frame>(_selected_skill.frames);
                newList.Add(new_frame);
                _selected_skill.frames = newList.ToArray();
                frame1.UpdateSkill(_selected_skill);
            }
        }

        private void tsmi_frame_delete_Click(object sender, EventArgs e)
        {
            if (frame1.selected_frame != null)
            {
                List<config_skill_frame> newList = new List<config_skill_frame>(_selected_skill.frames);
                newList.Remove(frame1.selected_frame);
                _selected_skill.frames = newList.ToArray();
                frame1.UpdateSkill(_selected_skill);
            }
        }

        private void tsmi_frame_Add_Click(object sender, EventArgs e)
        {
            config_skill_frame new_frame = new config_skill_frame();
            List<config_skill_frame> newList = new List<config_skill_frame>(_selected_skill.frames);
            newList.Add(new_frame);
            _selected_skill.frames = newList.ToArray();
            frame1.UpdateSkill(_selected_skill);
        }

        private void tsmi_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnResetView_Click(object sender, EventArgs e)
        {
            boxView1.center_offset = new Point(0, 0);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("确认退出吗？","警告！",MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void tsmi_frame_addframecount_Click(object sender, EventArgs e)
        {
            frame1.InsertFrameCount();
        }

        private void tsmi_frame_appendframecount_Click(object sender, EventArgs e)
        {
            frame1.AppendFrameCount();
        }

        private void tsmi_frame_deleteframecount_Click(object sender, EventArgs e)
        {
            frame1.DeleteFrameCount();
        }

        private void propertyGrid1_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            if (e.NewSelection.Value is config_action_attack)
            {
                //刷新攻击框
                boxView1.listener = new FieldBoxEvent(e.NewSelection.Value);
            }
            else if(e.NewSelection.Value is config_action_summon)
            {
                boxView1.listener = new FieldBoxEvent(e.NewSelection.Value);
            }
            else if(e.NewSelection.Value is config_projectile)
            {
                boxView1.listener = new FieldBoxEvent(e.NewSelection.Value);
            }
            else if(e.NewSelection.Value is config_action_attract)
            {
                boxView1.listener = new FieldBoxEvent(e.NewSelection.Value);
            }
            splitContainer1.Panel2.Enabled = boxView1.listener != null;
        }

        //private void propertyGrid1_Click(object sender, EventArgs e)
        //{
        //    if (propertyGrid1.SelectedGridItem!=null && propertyGrid1.SelectedGridItem.Value is config_action_attack)
        //    {
        //        //刷新攻击框
        //        boxView1.listener = new AttackEvent(propertyGrid1.SelectedGridItem.Value as config_action_attack);
        //    }
        //}


    }


    class ListBoxItem_skill
    {
        public config_skill skill;
        public ListBoxItem_skill(config_skill s)
        {
            skill = s;
        }
        public override string ToString()
        {
            return skill.id + ":" + skill.name;
        }
    }

}


namespace UnityEngine
{
    public class Debug
    {
        public static void Log(string msg)
        {
            Console.WriteLine(msg);
        }
        public static void LogError(string msg)
        {
            Console.WriteLine(msg);
        }
        public static void LogWarning(string msg)
        {
            Console.WriteLine(msg);
        }
    }


    public class Vector2
    {
        public float x;
        public float y;
        public Vector2()
        {

        }
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static float Dot(Vector2 a, Vector2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }
        public static Vector2 operator /(Vector2 a, float s)
        {
            return new Vector2(a.x / s, a.y / s);
        }

        public float magnitude
        {
            get
            {
                return (float)Math.Sqrt(x * x + y * y);
            }
        }

        public Vector2 normalized
        {
            get
            {
                return new Vector2(x / magnitude, y / magnitude);
            }
        }
    }

    public class Matrix4x4
    {
        public Vector3 MultiplyPoint(Vector3 point)
        {
            return new Vector3(0, 0, 0);
        }
    }

    public class Vector3
    {
        public float x;
        public float y;
        public float z;
        public Vector3()
        {

        }
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public class Color
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public Color() { }
        //
        // 摘要: 
        //     Constructs a new Color with given r,g,b components and sets /a/ to 1.
        //
        // 参数: 
        //   r:
        //     Red component.
        //
        //   g:
        //     Green component.
        //
        //   b:
        //     Blue component.
        public Color(float r, float g, float b) { this.r = r; this.g = g; this.b = b; this.a = 1.0f; }
        //
        // 摘要: 
        //     Constructs a new Color with given r,g,b,a components.
        //
        // 参数: 
        //   r:
        //     Red component.
        //
        //   g:
        //     Green component.
        //
        //   b:
        //     Blue component.
        //
        //   a:
        //     Alpha component.
        public Color(float r, float g, float b, float a) { this.r = r; this.g = g; this.b = b; this.a = a; }
    }
}
