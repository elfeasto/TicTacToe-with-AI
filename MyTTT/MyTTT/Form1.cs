using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyTTT
{
    public enum Result { XWin, OWin, Draw, NotOver };



    public partial class Form1 : Form
    {
        bool gameOver;
        bool XTurn;
        bool againstCPU = true;
        bool cpuIsX = false;


        const int numButtonsAcross = 3;
        const int numButtonsVertical = 3;
        int buttonWidth = 60;
        int buttonHeight = 60;
        float fontRatio = 0.7f;
        Button[,] buttons = new Button[numButtonsVertical, numButtonsAcross];

        AI gameAI = new AI();

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // sets up an array of buttons
            for (int i = 0; i < numButtonsVertical; i++)
            {
                for (int j = 0; j < numButtonsAcross; j++)
                {
                    Button myButton = new Button();
                    buttons[i, j] = myButton;
                    myButton.Size = new Size(buttonWidth, buttonHeight);
                    myButton.Location = new Point(buttonWidth * j, buttonHeight * i);
                    myButton.Font = new Font("Arial", buttonWidth * fontRatio);
                    myButton.TextAlign = ContentAlignment.MiddleCenter;
                    myButton.Click += new EventHandler(ButtonClick);
                    this.Controls.Add(myButton);
                }
            }
            // autosize after buttons added
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            StartGame();

        }

        private void StartGame()
        {

            foreach (Button b in buttons)
            {
                b.Text = "";
            }

            XTurn = true;
            TurnLabel.Text = "Turn: X";
            gameOver = false;

            // cpu move if needed
            if (againstCPU && cpuIsX)
            {
                CPUTakeTurn();
            }
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            PlayOneRound(b);
        }

        ///takes a player turn on selected button 
        ///then a cpu turn if against cpu
        private void PlayOneRound(Button selectedButton)
        {
            PlayerTakeTurn(selectedButton);
            if (againstCPU && !gameOver)
            {
                CPUTakeTurn();
            }             
        }


        private void PlayerTakeTurn(Button selectedButton)
        {
            if (gameOver)
            {
                return;
            }
            // mark button if was empty
            if (selectedButton.Text != "")
            {
                MessageBox.Show("That tile is already marked, try another tile please");
                return;
            }

            //change button text
            string buttonText;
            if (XTurn)
            {
                buttonText = "X";
            }
            else
            {
                buttonText = "O";
            }
            selectedButton.Text = buttonText;

            //check for result
            Result turnResult;
            char[,] ButtonsAsChars = ButtonsToCharArray();
            turnResult = FindResult.CheckResult(ButtonsAsChars, 3); //check for 3 marks in a row

            if (turnResult != Result.NotOver)
            {
                MessageBox.Show(turnResult.ToString());
                gameOver = true;
            }
            
            // switch whose turn it is
            XTurn = !XTurn;
            if (gameOver)
                TurnLabel.Text = "";
            else // game not over
            {
                if (XTurn)
                    TurnLabel.Text = "Turn: X";
                else
                    TurnLabel.Text = "Turn: O";
            }
        }

        private void CPUTakeTurn()
        {

            int[] nextAIcoords;
            Button selectedButton;
            Char[,] buttonsAsChars = ButtonsToCharArray();

            nextAIcoords = gameAI.GetAIMove(buttonsAsChars, XTurn);
            selectedButton = buttons[nextAIcoords[0], nextAIcoords[1]]; //finds the AI move as a button 
           
            //change button text
            string buttonText;
            if (XTurn)
            {
                buttonText = "X";
            }
            else
            {
                buttonText = "O";
            }
            selectedButton.Text = buttonText;

            //check for result
            Result turnResult;
            char[,] ButtonsAsChars = ButtonsToCharArray();
            turnResult = FindResult.CheckResult(ButtonsAsChars, 3); //check for 3 marks in a row

            if (turnResult != Result.NotOver)
            {
                MessageBox.Show(turnResult.ToString());
                gameOver = true;
            }

            // switch whose turn it is
            XTurn = !XTurn;
            if (gameOver)
                TurnLabel.Text = "";
            else // game not over
            {
                if (XTurn)
                    TurnLabel.Text = "Turn: X";
                else
                    TurnLabel.Text = "Turn: O";
            }
        }

        private char[,] ButtonsToCharArray()
        {
            char[,] board = new char[numButtonsAcross, numButtonsVertical];

            for (int i = 0; i < numButtonsAcross; i++)
            {
                for (int j = 0; j < numButtonsVertical; j++)
                {
                    if (buttons[i, j].Text == "")
                    {
                        board[i, j] = ' ';
                    }
                    else
                    {
                        board[i, j] = (char)buttons[i, j].Text[0];
                    }
                }
            }
            return board;
        }

        private void ResetGame(object sender, EventArgs e)
        {
            StartGame();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(gameAI.numCalculations.ToString()); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            gameAI.testOne();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CPUTakeTurn();
        }
    }
}

