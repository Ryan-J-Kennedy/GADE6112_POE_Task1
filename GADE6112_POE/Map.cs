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
    class Map
    {
        //Ryan Kennedy
        //19013266

        public string[,] map = new string[20, 20];

        public List<Button> buttons = new List<Button>();

        Random rd = new Random();

        public List<Unit> units = new List<Unit>();
        public List<Unit> rangedUnits = new List<Unit>();
        public List<Unit> meleeUnits = new List<Unit>();

        public Unit[,] unitMap = new Unit[20, 20];


        int unitNum;


        //Constructor called with the number of units as a parameter
        public Map(int unitN = 0)
        {
            unitNum = unitN;
        }

        //Creates the unit objects and randomises thier x and y positions
        public void GenerateBattlefeild()
        {

            for (int i = 0; i < unitNum; i++)
            {
                RangedUnit archer = new RangedUnit(0, 0, Faction.Dire, 30, 1, 3, 3, "{|", false);
                rangedUnits.Add(archer);

                MeleeUnit knight = new MeleeUnit(0, 0, Faction.Dire, 40, 1, 5, 1, "/", false);
                meleeUnits.Add(knight);
            }

            for (int i = 0; i < unitNum; i++)
            {
                RangedUnit archer = new RangedUnit(0, 0, Faction.Radient, 30, 1, 3, 3, "{|", false);
                rangedUnits.Add(archer);

                MeleeUnit knight = new MeleeUnit(0, 0, Faction.Radient, 40, 1, 5, 1, "/", false);
                meleeUnits.Add(knight);
            }


            //Randomises unit object's x and u position
            foreach (Unit u in rangedUnits)
            {
                for (int i = 0; i < rangedUnits.Count; i++)
                {
                    int xPos = rd.Next(0, 20);
                    int yPos = rd.Next(0, 20);

                    while (xPos == rangedUnits[i].posX && yPos == rangedUnits[i].posY && xPos == meleeUnits[i].posX && yPos == meleeUnits[i].posY)
                    {
                        xPos = rd.Next(0, 20);
                        yPos = rd.Next(0, 20);
                    }

                    u.posX = xPos;
                    u.posY = yPos;
                    unitMap[u.posY, u.posX] = (Unit)u;
                }

                units.Add(u);
            }

            foreach (Unit u in meleeUnits)
            {
                for (int i = 0; i < meleeUnits.Count; i++)
                {
                    int xPos = rd.Next(0, 20);
                    int yPos = rd.Next(0, 20);

                    while (xPos == meleeUnits[i].posX && yPos == meleeUnits[i].posY && xPos == rangedUnits[i].posX && yPos == rangedUnits[i].posY)
                    {
                        xPos = rd.Next(0, 20);
                        yPos = rd.Next(0, 20);
                    }

                    u.posX = xPos;
                    u.posY = yPos;
                }

                unitMap[u.posY, u.posX] = (Unit)u;
                units.Add(u);
            }

            PlaceUnits();
        }

        //Places the units on a string representation of the 20x20 map
        public void PlaceUnits()
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    map[i, j] = "";
                }
            }

            foreach (Unit u in units)
            {
                unitMap[u.posY, u.posX] = u;
            }

            foreach (Unit u in rangedUnits)
            {
                map[u.posY, u.posX] = "R";
            }

            foreach (Unit u in meleeUnits)
            {
                map[u.posY, u.posX] = "M";
            }
        }
    }
}

