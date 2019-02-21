using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrabbleLibrary
{
    public class Rack : IRack
    {

        public Rack(Bag bag, string playerName)
        {
            this.bag = bag;
            this.playerName = playerName;
            addLetterValues();
        }

        // Member variables
        public List<char> rackLetters = new List<char>();
        public int totalPoints = 0;
        public string playerName;
        public Bag bag;

        // Letter values
        Dictionary<string, int> letterValues = new Dictionary<string, int>();

        // Set up the point values for each letter 
        public void addLetterValues()
        {
            if (letterValues.Count == 0)
            {
                letterValues.Add("a", 1);
                letterValues.Add("e", 1);
                letterValues.Add("i", 1);
                letterValues.Add("l", 1);
                letterValues.Add("n", 1);
                letterValues.Add("o", 1);
                letterValues.Add("r", 1);
                letterValues.Add("s", 1);
                letterValues.Add("t", 1);
                letterValues.Add("u", 1);
                letterValues.Add("d", 2);
                letterValues.Add("g", 2);
                letterValues.Add("b", 3);
                letterValues.Add("c", 3);
                letterValues.Add("m", 3);
                letterValues.Add("p", 3);
                letterValues.Add("f", 4);
                letterValues.Add("h", 4);
                letterValues.Add("v", 4);
                letterValues.Add("w", 4);
                letterValues.Add("y", 4);
                letterValues.Add("k", 5);
                letterValues.Add("j", 8);
                letterValues.Add("x", 8);
                letterValues.Add("q", 10);
                letterValues.Add("z", 10);
            }
        }

        // Return the player's total score
        public int TotalPoints
        {
            get
            {
                return totalPoints;
            }
        }


        // Only tests a word for points, PlayWord actually removes the tiles from the rack 
        public int GetPoints(string Candidate)
        {

            // Create a copy of the rack to use when testing the word
            List<char> copyOfRack = new List<char>();
            foreach (char letter in rackLetters)
            {
                copyOfRack.Add(letter);
            }

            // Check if the word is made up of the letters in the rack
            foreach (char letter in Candidate)
            {
                if (copyOfRack.Contains(letter))
                {
                    int index = copyOfRack.IndexOf(letter);
                    copyOfRack.RemoveAt(index);
                }
                else return 0;
            }

            // Check if the word is a real word
            if(!bag.spellChecker.CheckSpelling(Candidate))
                return 0;

            // Calculate the score of the word 
            int wordScore = 0;

            foreach (char letter in Candidate)
            {
                string letterString = letter.ToString();
                wordScore += letterValues[letterString];
            }

            return wordScore;
        }

        // Player the candidate word, which removes the used letters from the player's rack and updates their total score
        public string PlayWord(string Candidate)
        {
            // If the word is valid, this code removes the used letters from the rack 
            int wordScore = GetPoints(Candidate);
            if (wordScore > 0)
            {
                foreach (char letter in Candidate)
                {
                    int index = rackLetters.IndexOf(letter);
                    rackLetters.RemoveAt(index);
                }

                // Add word points to player's total
                totalPoints += wordScore;
            }
            return Candidate;
        }

        // Fill up the rack with more tiles if it has less than 7 
        public string TopUp()
        {
            // randomize the order of letters in the bag and pull out how many you need 
            Random rng = new Random();
            if (bag.TilesRemaining > 0)
            {
                bag.TilesInBag = bag.TilesInBag.OrderBy(number => rng.Next()).ToList();
                int tilesNeeded = 7 - rackLetters.Count();

                for (int i = 0; i < tilesNeeded; i++)
                {
                    rackLetters.Add(bag.TilesInBag[i]);
                    bag.TilesInBag.Remove(bag.TilesInBag[i]);
                }

                string tiles = "";

                foreach (char tile in rackLetters)
                {
                    tiles += tile;
                }

                // return the letters in the rack as a string
                return tiles;
            }
            else
            {
                bag.spellChecker.Quit();
                return "No tiles left.";
            }
        }

        // Return the letters in the rack as a string
        public override string ToString()
        {
            string tiles = "";

            foreach (char tile in rackLetters)
            {
                tiles += tile;
            }

            return tiles;
        }
    }
}
