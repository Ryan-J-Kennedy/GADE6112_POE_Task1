using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GADE6112_POE
{
    //Ryan Kennedy
    //19013266

    enum Faction
    {
        Dire,
        Radient
    }

    abstract class Unit
    {
        protected int posX, posY;
        protected int health;
        protected int maxHealth;
        protected int speed;
        protected int attack, attackRange;
        protected string symbol;
        protected Faction factionType;
        protected bool isAttacking;

        public Unit(int x, int y, int hp, int sp, int att, int attRange, string sym, Faction faction, bool isAtt)
        {
            posX = x;
            posY = y;
            health = hp;
            speed = sp;
            attack = att;
            attackRange = attRange;
            symbol = sym;
            factionType = faction;
            isAttacking = isAtt;

            maxHealth = hp;
        }

        public abstract void Move();

        public abstract void Combat();

        public abstract void  CheckAttackRange(List<Unit> uni, Unit[,] unitMap);

        public abstract Unit ClosestEnemy();

        public abstract bool Death();

        public override abstract string ToString();
    }
}
