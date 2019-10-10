using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace GADE6112_POE
{
    //Ryan Kennedy
    //19013266

    public partial class Form1 : Form
    {
        Button[,] buttons = new Button[20,20]; 

        static int unitNum = 8;

        public int round = 1;

        Map m = new Map(unitNum);

        public Form1()
        {
            InitializeComponent();
        }

        //Runs when the form loads to set up the map
        private void Form1_Load(object sender, EventArgs e)
        {
            m.GenerateBattlefeild();
            Placebuttons();
        }

        //Places the buttons on the form and puts the units in the buttons 
        public void Placebuttons()
        {
            gbMap.Controls.Clear();

            Size btnSize = new Size(30, 30);

            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    Button btn = new Button();

                    btn.Size = btnSize;
                    btn.Location = new Point(x * 30, y * 30);

                    if (m.map[x, y] == "R")
                    {
                        if (m.unitMap[x, y] is RangedUnit)
                        {
                            RangedUnit M = (RangedUnit)m.unitMap[x, y];

                            btn.Text = M.Symbol;
                            btn.Name = M.ToString();
                            btn.Click += MyButtonClick;

                            if (M.FactionType == Faction.Dire)
                            {
                                btn.BackColor = Color.Red;
                            }
                            else
                            {
                                btn.BackColor = Color.Green;
                            }

                            gbMap.Controls.Add(btn);
                        }
                    }
                    else if (m.map[x, y] == "M")
                    {
                        if (m.unitMap[x, y] is MeleeUnit)
                        {
                            MeleeUnit M = (MeleeUnit)m.unitMap[x, y];

                            btn.Text = M.Symbol;
                            btn.Name = M.ToString();
                            btn.Click += MyButtonClick;

                            if (M.FactionType == Faction.Dire)
                            {
                                btn.BackColor = Color.Red;
                            }
                            else
                            {
                                btn.BackColor = Color.Green;
                            }

                            gbMap.Controls.Add(btn);
                        }
                    }
                }
            }
        }

        //Starts the timer when the button is clicked
        private void btnStart_Click(object sender, EventArgs e)
        {
            GameTick.Enabled = true;
        }

        //Pauses the timer when the button is clicked
        private void btnPause_Click(object sender, EventArgs e)
        {
            GameTick.Enabled = false;
        }

        //Timer to run every second
        private void GameTick_Tick(object sender, EventArgs e)
        {
            GameLogic();
            

            lblRound.Text = "Round: " + round; 
        }

        //Runs all the logic behind the game
        public void GameLogic()
        {
            int dire = 0;
            int radiant = 0;

            foreach (Unit u in m.units)
            {
                if (u is MeleeUnit)
                {
                    MeleeUnit M = (MeleeUnit)u;

                    if (M.FactionType == Faction.Dire)
                    {
                        dire++;
                    }
                    else
                    {
                        radiant++;
                    }
                }
                else if (u is RangedUnit)
                {
                    RangedUnit M = (RangedUnit)u;

                    if (M.FactionType == Faction.Dire)
                    {
                        dire++;
                    }
                    else
                    {
                        radiant++;
                    }
                }
            }


            if (dire > 0 && radiant > 0)//Checks to see if both teams are still alive
            {
                foreach (Unit u in m.units)
                {
                    u.CheckAttackRange(m.units, m.unitMap);
                }
                
                round++;
                m.PlaceUnits();
                Placebuttons();
            }
            else
            {
                m.PlaceUnits();
                Placebuttons();
                GameTick.Enabled = false;

                if (dire > radiant)
                {
                    MessageBox.Show("Dire Wins in " + round + " rounds");
                }
                else
                {
                    MessageBox.Show("Radiant Wins in " + round + " rounds");
                }
            }

            //Checks to see who has died and needs to be deleted
            for (int i = 0; i < m.rangedUnits.Count; i++)
            {
                if (m.rangedUnits[i].Death())
                {
                    m.map[m.rangedUnits[i].PosX, m.rangedUnits[i].PosY] = "";
                    m.rangedUnits.RemoveAt(i);
                    
                }
            }

            for (int i = 0; i < m.meleeUnits.Count; i++)
            {
                if (m.meleeUnits[i].Death())
                {
                    m.map[m.meleeUnits[i].PosX, m.meleeUnits[i].PosY] = "";
                    m.meleeUnits.RemoveAt(i);

                }
            }

            for (int i = 0; i < m.units.Count; i++)
            {
                if (m.units[i].Death())
                {
                    if (m.units[i] is MeleeUnit)
                    {
                        MeleeUnit M = (MeleeUnit)m.units[i];
                        m.map[M.PosX, M.PosY] = "";
                        m.units.RemoveAt(i);
                    }
                    else if (m.units[i] is RangedUnit)
                    {
                        RangedUnit R = (RangedUnit)m.units[i];
                        m.map[R.PosX, R.PosY] = "";
                        m.units.RemoveAt(i);
                    }
                }
            }
        }

        //The on click event of the buttons with the units
        public void MyButtonClick(object sender, EventArgs e)
        {
            Button btn = ((Button)sender);

            foreach (Unit u in m.units)
            {
                if(btn.Name == u.ToString())
                {
                    txtOutput.Text = u.ToString();
                }
            }
        }
    }
}
