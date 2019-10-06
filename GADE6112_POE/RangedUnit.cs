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

    class RangedUnit : Unit
    {
        public int PosX
        {
            get { return base.posX; }
            set { base.posX = value; }
        }

        public int PosY
        {
            get { return base.posY; }
            set { base.posY = value; }
        }

        public int Health
        {
            get { return base.health; }
            set { base.health = value; }
        }

        public int MaxHealth
        {
            get { return base.maxHealth; }
        }

        public int Speed
        {
            get { return base.speed; }
        }

        public int Attack
        {
            get { return base.attack; }
        }

        public int AttackRange
        {
            get { return base.attackRange; }
        }

        public string Symbol
        {
            get { return base.symbol; }
        }

        public Faction FactionType
        {
            get { return base.factionType; }
        }

        public bool IsAttacking
        {
            get { return base.isAttacking; }
            set { base.isAttacking = value; }
        }

        private int speedCounter = 1;
        List<Unit> units = new List<Unit>();
        Random r = new Random();
        Unit closestUnit;

        //Constructor that sends all parameters to the unit constructor
        public RangedUnit(int x, int y, Faction faction, int hp, int sp, int att, int attRange, string sym, bool isAtt)
            : base(x, y, hp, sp, att, attRange, sym, faction, isAtt)
        {

        }

        //Changes the x and y position towards the closest enemy or to run away
        public override void Move()
        {
            //Moves towards closest enemey
            if (Health > MaxHealth * 0.25)
            {
                if (closestUnit is MeleeUnit)
                {
                    MeleeUnit M = (MeleeUnit)closestUnit;
                    if (M.PosX > posX && PosX < 19)
                    {
                        posX++;
                    }
                    else if (M.PosX < posX && posX > 0)
                    {
                        posX--;
                    }

                    if (M.PosY > posY && PosY < 19)
                    {
                        posY++;
                    }
                    else if (M.PosY < posY && posY > 0)
                    {
                        posY--;
                    }
                }
                else if (closestUnit is RangedUnit)
                {
                    RangedUnit R = (RangedUnit)closestUnit;
                    if (R.PosX > posX && PosX < 19)
                    {
                        posX++;
                    }
                    else if (R.PosX < posX && posX > 0)
                    {
                        posX--;
                    }

                    if (R.PosY > posY && PosY < 19)
                    {
                        posY++;
                    }
                    else if (R.PosY < posY && posY > 0)
                    {
                        posY--;
                    }
                }
            }
            else //Moves in random direction to run away
            {
                int direction = r.Next(0, 4);

                if(direction == 0 && PosX < 19)
                {
                    posX++;
                }
                else if(direction == 1 && posX > 0)
                {
                    posX--;
                }
                else if(direction == 2 && posY < 19)
                {
                    posY++;
                }
                else if(direction == 3 && posY > 0)
                {
                    posY--;
                }
            }

        }

        //Deals damage to closest unit if they are in attack range
        public override void Combat()
        {
            if (closestUnit is MeleeUnit)
            {
                MeleeUnit M = (MeleeUnit)closestUnit;
                M.Health -= Attack;
            }
            else if (closestUnit is RangedUnit)
            {
                RangedUnit R = (RangedUnit)closestUnit;
                R.Health -= Attack;
            }
        }

        //Checks to see if the closest enemy is in attack range and if they are calls combat or move if they aren't
        public override void CheckAttackRange(List<Unit> uni, Unit[,] unitMap)
        {
            units = uni;

            closestUnit = ClosestEnemy();
            int xDis, yDis;
            int distance = 1000;

            if (closestUnit is MeleeUnit)
            {
                MeleeUnit M = (MeleeUnit)closestUnit;
                xDis = Math.Abs((PosX - M.PosX) * (PosX - M.PosX));
                yDis = Math.Abs((PosY - M.PosY) * (PosY - M.PosY));

                distance = (int)Math.Round(Math.Sqrt(xDis + yDis), 0);
            }
            else if (closestUnit is RangedUnit)
            {
                RangedUnit R = (RangedUnit)closestUnit;
                xDis = Math.Abs((PosX - R.PosX) * (PosX - R.PosX));
                yDis = Math.Abs((PosY - R.PosY) * (PosY - R.PosY));

                distance = (int)Math.Round(Math.Sqrt(xDis + yDis), 0);
            }

            //Checks to see if they are below 25% health so they move rather than attacking
            if (Health > MaxHealth * 0.25)
            {
                if (distance <= AttackRange)
                {
                    Combat();
                }
                else
                {
                    Move();
                }
            }
            else
            {
                Move();
            }
            
        }

        //finds and returns the closest enemy
        public override Unit ClosestEnemy()
        {
            int xDis, yDis;
            double distance = 1000;
            double temp = 1000;
            Unit target = null;


            foreach (Unit u in units)
            {
                if (u is RangedUnit)
                {
                    RangedUnit b = (RangedUnit)u;

                    if (FactionType != b.FactionType)
                    {
                        xDis = Math.Abs((PosX - b.PosX) * (PosX - b.PosX));
                        yDis = Math.Abs((PosY - b.PosY) * (PosY - b.PosY));

                        distance = Math.Round(Math.Sqrt(xDis + yDis), 0);
                    }
                }
                else if (u is MeleeUnit)
                {
                    MeleeUnit b = (MeleeUnit)u;

                    if (FactionType != b.FactionType)
                    {
                        xDis = Math.Abs((PosX - b.PosX) * (PosX - b.PosX));
                        yDis = Math.Abs((PosY - b.PosY) * (PosY - b.PosY));

                        distance = Math.Round(Math.Sqrt(xDis + yDis), 0);
                    }
                }

                if (distance < temp)
                {
                    temp = distance;
                    target = u;
                }
            }

            return target;
        }

        //Checks and returns if the unit is below or at 0 health
        public override bool Death()
        {
            if (Health <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Returns the units information
        public override string ToString()
        {
            return "Archer X: " + PosX
                + " Y: " + PosY
                + "\nMax Health: " + MaxHealth
                + "\nHealth: " + Health
                + "\nSpeed: " + Speed
                + "\nAttack Damage " + Attack
                + "\nAttack Range: " + AttackRange
                + "\nFaction: " + FactionType
                + "\nAttacking: " + IsAttacking;
        }
    }
}
