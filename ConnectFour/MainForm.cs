using ConnectFour.Properties;
using ConnectFourEngine;
using System;
using System.Windows.Forms;

namespace ConnectFour
{
    public partial class MainForm : Form
    {
        private Engine _engine;
        private PictureBox[,] _pictureBoxes;

        public MainForm()
        {
            InitializeComponent();
            _engine = new Engine();
            _pictureBoxes = new PictureBox[,] { { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7 }, { pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14 }, { pictureBox15, pictureBox16, pictureBox17, pictureBox18, pictureBox19, pictureBox20, pictureBox21 }, { pictureBox22, pictureBox23, pictureBox24, pictureBox25, pictureBox26, pictureBox27, pictureBox28 }, { pictureBox29, pictureBox30, pictureBox31, pictureBox32, pictureBox33, pictureBox34, pictureBox35 }, { pictureBox36, pictureBox37, pictureBox38, pictureBox39, pictureBox40, pictureBox41, pictureBox42 } };
        }

        private void GameRound(object sender, EventArgs e)
        {
            var pictureBoxNumber = int.Parse((sender as PictureBox).Name.Substring(10));

            try
            {
                _engine.Play((pictureBoxNumber - 1) % 7);
                _engine.AIMove();
            }
            catch
            {
            }

            var board = _engine.Print();

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    switch (board[i][j])
                    {
                        case '0':
                            _pictureBoxes[i, j].Image = Resources.Empty;
                            break;
                        case '1':
                            _pictureBoxes[i, j].Image = Resources.Red;
                            break;
                        case '2':
                            _pictureBoxes[i, j].Image = Resources.Blue;
                            break;
                    }
                }
            }

            if (_engine.Winner() != null)
            {
                MessageBox.Show(_engine.Winner().Value ? "You Win!" : "You Lose...");
            }
        }
    }
}