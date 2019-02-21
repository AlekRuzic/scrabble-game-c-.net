using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ScrabbleLibrary;

namespace ScrabbleConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize a new bag 
            IBag bag = new Bag();

            // Initialize list to hold player (rack) objects that contain the players' scores and the contents of their racks
            List<IRack> players = new List<IRack>();

            Console.WriteLine(bag.About);
            Console.WriteLine();

            Console.WriteLine("Bag initialized with the following 98 tiles...");
            Console.WriteLine(bag.ToString());
            Console.WriteLine();

            string numPlayers = "";

            // Check to make sure a valid input was entered for number of players
            bool validPlayerNumber = false;
            while (!validPlayerNumber)
            {
                Console.WriteLine("Enter the number of players (1-8): ");

                numPlayers = Console.ReadLine();

                if (Regex.IsMatch(numPlayers, @"^\d+$") && (int.Parse(numPlayers) < 9) && (int.Parse(numPlayers) > 0))
                {
                    validPlayerNumber = true;
                }
                else
                {
                    Console.WriteLine("Invalid number of players");
                }
            }

            for (int i = 0; i < int.Parse(numPlayers); i++)
            {
                players.Add(bag.NewRack());
                players[i].TopUp();
            }

            Console.WriteLine();
            Console.WriteLine("Racks for {0} players were populated.", players.Count());
            Console.WriteLine("Bag now contains the follows {0} tiles.", bag.TilesRemaining);
            Console.WriteLine(bag.ToString());
            Console.WriteLine();

            while (bag.TilesRemaining > 0)
            {
                for (int i=0; i<int.Parse(numPlayers); i++)
                {

                    Console.WriteLine();
                    Console.WriteLine("--------------------------------------------------------------");
                    Console.WriteLine("                          Player {0}                          ", i + 1);
                    Console.WriteLine("--------------------------------------------------------------");
                    Console.WriteLine("Your rack contains [" + players[i].ToString() + "]");
                    Console.WriteLine("Test a word for its points value? (y/n)");
                    string response = Console.ReadLine();
                    switch (response)
                    {
                        // If the player wants to test a word for how many points it's worth 
                        case "y":
                            Console.WriteLine("Enter a word using the letters [" + players[i].ToString() + "]");
                            string candidate = Console.ReadLine();
                            int wordScore = players[i].GetPoints(candidate);

                            // If the word score is 0, the word is not valid
                            if (wordScore == 0)
                                Console.WriteLine("The word [{0}] is not a real word, or is not made up of only the letters in [{1}]", candidate, players[i].ToString());
                            else
                            {
                                Console.WriteLine("The word [{0}] is worth {1} points.", candidate, wordScore);
                                Console.WriteLine("Do you want to play the word [{0}]?", candidate);

                                string playWordResponse = Console.ReadLine();
                                switch (playWordResponse)
                                {
                                    case "y":
                                        players[i].PlayWord(candidate);
                                        Console.WriteLine("          -----------------------------");
                                        Console.WriteLine("          Word Played: {0}", candidate);
                                        Console.WriteLine("          Total Points: {0}", players[i].TotalPoints);
                                            // Top up the player's rack if there are tiles in the bag
                                            try
                                            {
                                                players[i].TopUp();
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex);
                                            }
                                        Console.WriteLine("          Rack now contains: [{0}]", players[i].ToString());
                                        Console.WriteLine("          -----------------------------");
                                        break;
                                    case "n":
                                        break;
                                }
                            }
                            break;

                        // If the player doesn't want to test a word, it moves on to the next player's turn
                        case "n":
                            break;
                    }

                    Console.WriteLine("The bag has the following " + bag.TilesRemaining + " tiles remaining");
                }
            }
            
            Console.WriteLine("All tiles have been used. Game over!");
            Console.WriteLine("Press enter to quit.");
            Console.ReadLine();

        }

    }
}
