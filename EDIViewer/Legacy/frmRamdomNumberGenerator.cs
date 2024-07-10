using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using DCSGlobal.BusinessRules.CoreLibraryII;

namespace Manual_test_app
{
    public partial class frmRamdomNumberGenerator : Form
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        
        public frmRamdomNumberGenerator()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string rnd = string.Empty;
           List<string> _items = new List<string>(); // <-- Add this
            const int totalRolls = 25000;
            int[] results = new int[Convert.ToInt32(textBox1.Text )];

            // Roll the dice 25000 times and display
            // the results to the console.
            for (int x = 0; x < totalRolls; x++)
            {
                byte roll = RollDice((byte)results.Length);
                results[roll - 1]++;
            }
            for (int i = 0; i < results.Length; ++i)
            {

                rnd = Convert.ToString((double)results[i] / (double)totalRolls);
                _items.Add(rnd);
                  rnd = Convert.ToString((results[i]));
                  _items.Add(rnd);
            }

            listBox1.DataSource = _items;
            rngCsp.Dispose();
            
        }


        // This method simulates a roll of the dice. The input parameter is the
        // number of sides of the dice.

        public static byte RollDice(byte numberSides)
        {
            if (numberSides <= 0)
                throw new ArgumentOutOfRangeException("numberSides");

            // Create a byte array to hold the random value.
            byte[] randomNumber = new byte[1];
            do
            {
                // Fill the array with a random value.
                rngCsp.GetBytes(randomNumber);
            }
            while (!IsFairRoll(randomNumber[0], numberSides));
            // Return the random number mod the number
            // of sides.  The possible values are zero-
            // based, so we add one.
            return (byte)((randomNumber[0] % numberSides) + 1);
        }



        private static bool IsFairRoll(byte roll, byte numSides)
        {
            // There are MaxValue / numSides full sets of numbers that can come up
            // in a single byte.  For instance, if we have a 6 sided die, there are
            // 42 full sets of 1-6 that come up.  The 43rd set is incomplete.
            int fullSetsOfValues = Byte.MaxValue / numSides;

            // If the roll is within this range of fair values, then we let it continue.
            // In the 6 sided die case, a roll between 0 and 251 is allowed.  (We use
            // < rather than <= since the = portion allows through an extra 0 value).
            // 252 through 255 would provide an extra 0, 1, 2, 3 so they are not fair
            // to use.
            return roll < numSides * fullSetsOfValues;
        }

        private void button2_Click(object sender, EventArgs e)
        {


            HighEntropyRamdonNumberGenerator rnd = new HighEntropyRamdonNumberGenerator();
            rnd.Mutiplyer = Convert.ToInt32(textBox1.Text);
            listBox1.Items.Add(rnd.Next());
    
           

        }

        private  void ShowRandomNumbers(int seed)
        {     
            string srnd = string.Empty;
                List<string> _items = new List<string>(); // <-- Add this
            Random rnd = new Random(seed);
            for (int ctr = 0; ctr <= 20; ctr++)
            {


                srnd = Convert.ToString(Math.Truncate(rnd.NextDouble() * 1000) * Convert.ToInt32(textBox2.Text));
                _items.Add(srnd);
              
            }
            rnd = null;
              listBox1.DataSource = _items;
        }


        private void getrnum()
        {

            Random rnd = new Random(Convert.ToInt32(textBox1.Text));

            int rf = 0;
            int rt = 0;

            rf = Convert.ToInt32(textBox2.Text);
            rf = Convert.ToInt32(textBox3.Text);

            listBox1.Items.Add((rf + Convert.ToInt32(rnd.Next()) * (rt - rf + 1)));

        }

        private void button3_Click(object sender, EventArgs e)
        {
            getrnum();
        }

    }
}
