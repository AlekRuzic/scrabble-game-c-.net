/*
 * Program:     ScrabbleConsoleClient.exe
 * Module:      Program.cs
 * Author:      Alek Ruzic
 * Date:        Feb 12, 2019
 * Description: A console client that uses the ScrabbeLibrary assembly to play the game of scrabble in the CLI
 *              
 * Status:      Complete
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrabbleLibrary
{
    // Interfaces
    public interface IRack
    {

        int TotalPoints { get; }

        int GetPoints(string Candidate);
        string PlayWord(string Candidate);
        string TopUp();
        string ToString();
    }

    public interface IBag
    {
        string About { get; }
        int TilesRemaining { get; }

        IRack NewRack();
        string ToString();
    }
    // End of Interfaces 


    public class Bag : IBag
    {
        // Member variables
        public List<char> TilesInBag = new List<char>();
        private int numberOfPlayers = 0;
        public Microsoft.Office.Interop.Word.Application spellChecker = new Microsoft.Office.Interop.Word.Application();


    // Constructor

    public Bag()
        {
            // Add letters that only occur once
            TilesInBag.Add('j');
            TilesInBag.Add('k');
            TilesInBag.Add('q');
            TilesInBag.Add('x');
            TilesInBag.Add('z');

            // Add letters that occur twice 
            for (int i=0; i<2; i++)
            {
                TilesInBag.Add('b');
                TilesInBag.Add('c');
                TilesInBag.Add('f');
                TilesInBag.Add('h');
                TilesInBag.Add('m');
                TilesInBag.Add('p');
                TilesInBag.Add('v');
                TilesInBag.Add('w');
                TilesInBag.Add('y');
            }

            // Add letters that occur three times 
            for (int i = 0; i < 3; i++)
            {
                TilesInBag.Add('g');
            }

            // Add letters that occur four times
            for (int i = 0; i < 4; i++)
            {
                TilesInBag.Add('d');
                TilesInBag.Add('l');
                TilesInBag.Add('s');
                TilesInBag.Add('u');
            }

            // Add letters that occur six times
            for (int i = 0; i < 6; i++)
            {
                TilesInBag.Add('n');
                TilesInBag.Add('r');
                TilesInBag.Add('t');
            }

            // Add letters that occur eight times
            for (int i = 0; i < 8; i++)
            {
                TilesInBag.Add('o');
            }

            // Add letters that occur nine times
            for (int i = 0; i < 9; i++)
            {
                TilesInBag.Add('a');
                TilesInBag.Add('i');
            }

            // Add letters that occur twelve times
            for (int i = 0; i < 12; i++)
            {
                TilesInBag.Add('e');
            }

            // Order Alphabetically
            TilesInBag.Sort();
        }


        public string About
        {
            get
            {
                return "Client for Scrabble Library - Alek Ruzic 2019";
            }
        }


        public int TilesRemaining
        {
            get
            {
                return TilesInBag.Count();
            }
        }

        public IRack NewRack()
        {
            numberOfPlayers++;

            // Do i even need player name?
            string playerName = "rack" + numberOfPlayers.ToString();
            IRack rack = new Rack(this, playerName);
            return rack;
        }


        public override string ToString()
        {
            string tiles = "";
            var dictionary = new Dictionary<char, int>();

            foreach (var letter in TilesInBag)
            {

                var key = char.ToLower(letter);
                if (dictionary.ContainsKey(key))
                    dictionary[key]++;
                else
                    dictionary.Add(key, 1);
            }

            foreach (var pair in dictionary.OrderBy(p => p.Key))
            {
                tiles += pair.Key + "(" + pair.Value + ") ";
            }

            return tiles;
        }

    }
}
