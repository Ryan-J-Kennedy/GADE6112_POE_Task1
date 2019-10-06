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

    class MeleeUnit : Unit
    {
        public int PosX
        {
            get { return base.posX; }
            set { base.posX = value; }
        }

        public int PosY
        {
            get { return base.posY; }
            set { posY = value; }
        }

        public int Health
        {
            get { return base.health; }
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

        Unit closestUnit = null;

        //Constructor that sends all parameters to the unit constructor
        public MeleeUnit(int x, int y, Faction faction, int hp, int sp, int att, int attRange, string sym, bool isAtt)
            : base(x, y, hp, sp, att, attRange, sym, faction, isAtt)
        {

        }

        //Changes the x and y position towards the closest enemy or to run away
        public override void Move()
        {
            //Moves towards closest enemey
            if (Health > MaxHealth * 0.25)
            {
                if (closestUnit.posX > posX && PosX < 19)
                {
                    posX++;
                }
                else if (closestUnit.posX < posX && posX > 0)
                {
                    posX--;
                }

                if (closestUnit.posY > posY && PosY < 19)
                {
                    posY++;
                }
                else if (closestUnit.posY < posY && posY > 0)
                {
                    posY--;
                }
            }
            else //Moves in random direction to run away
            {
                int direction = r.Next(0, 4);

                if (direction == 0 && PosX < 19)
                {
                    posX++;
                }
                else if (direction == 1 && posX > 0)
                {
                    posX--;
                }
                else if (direction == 2 && posY < 19)
                {
                    posY++;
                }
                else if (direction == 3 && posY > 0)
                {
                    posY--;
                }
            }

        }

        //Deals damage to closest unit if they are in attack range
        public override void Combat()
        {
            foreach (Unit u in units)
            {
                if (closestUnit.posX == u.posX && closestUnit.posY == u.posY)
                {
                    u.health = u.health - Attack;
                    IsAttacking = true;
                    break;
                }
                else
                {
                    IsAttacking = false;
                }
            }
        }

        //Checks to see if they are below 25% health so they move rather than attacking
        public override void CheckAttackRange(List<Unit> uni, Unit[,] unitMap)
        {
            units = uni;

            closestUnit = ClosestEnemy();
            int xDis, yDis;
            int distance;

            xDis = Math.Abs((PosX - closestUnit.posX) * (PosX - closestUnit.posX));
            yDis = Math.Abs((PosY - closestUnit.posY) * (PosY - closestUnit.posY));

            distance = (int)Math.Round(Math.Sqrt(xDis + yDis), 0);

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
            double distance;
            double temp = 1000;
            Unit target = null;


            foreach (Unit u in units)
            {
                if (FactionType != u.factionType)
                {
                    xDis = Math.Abs((PosX - u.posX) * (PosX - u.posX));
                    yDis = Math.Abs((PosY - u.posY) * (PosY - u.posY));

                    distance = Math.Round(Math.Sqrt(xDis + yDis), 0);

                    if (distance < temp)
                    {
                        temp = distance;
                        target = u;
                    }
                }
            }

            return target;
        }

        //Checks and returns if the unit is below or at 0 health
        public override bool Death()
        {
            if(Health <= 0)
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
            return "Knight X: " + PosX
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
