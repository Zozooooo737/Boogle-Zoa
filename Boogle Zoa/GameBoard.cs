﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boogle_Zoa
{
    internal class GameBoard
    {
        private Dice[] dices;
        private char[] visibleLetters;
        private const int size = 16;
        private const int side = sqrt(size);

        public GameBoard(Dice[] d)
        {
            dices = d;
            for(int i = 0;i<dices.Length;i++)
            {
                dices[i].visibleLetter = visibleLetters[i];
            }
        }
        
        public string toString()
        {
            string description = "";

            int n = 0;

            for (int i = 0; i < side; i++)
            {
                for (int j = 0; j < side; j++)
                {
                    description += visibleLetters[n] + " ";
                    n++;
                }
                description += "\n";
            }
            return description;
        }

        public bool GameBoard_Test(string mot)
        {
            bool adja = false;
            char[,] Board = new int[side, side];
            int n = 0;
            for (int i = 0; i < side; i++)
            {
                for (int j = 0; j < side; j++)
                {
                    Board[i,j] = visibleLetters[n];
                    n++;
                }
            }


        }

        public bool Adjacent(string mot, char lettre)
    }
}
