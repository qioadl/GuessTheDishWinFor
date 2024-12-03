using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GuessTheDishWinForms
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> dishImages = null!;
        private Dictionary<string, string> dishHints = null!;
        private List<string> dishNames = null!;
        private int currentLevel;
        private string correctDish = string.Empty;
        private int attempts;

        private PictureBox dishPictureBox = null!;
        private TextBox answerTextBox = null!;
        private Button checkButton = null!;
        private Label attemptsLabel = null!;
        private Label resultLabel = null!;
        private Label questionLabel = null!;
        private Label hintLabel = null!;

        public Form1()
        {
            InitializeComponent();
            InitGame();
        }

        private void InitGame()
        {
            // Укажите путь к папке с изображениями
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

            // Инициализация изображений и подсказок
            dishImages = new Dictionary<string, string>
            {
                { "борщ", Path.Combine(imagePath, "borscht.jpg") },
                { "паста", Path.Combine(imagePath, "pasta.jpg") },
                { "суші", Path.Combine(imagePath, "sushi.jpg") },
                { "піца", Path.Combine(imagePath, "pizza.jpg") },
                { "вареники", Path.Combine(imagePath, "vareniki.jpg") },
                { "салат", Path.Combine(imagePath, "salad.jpg") },
                { "бургер", Path.Combine(imagePath, "burger.jpg") }
            };

            dishHints = new Dictionary<string, string>
            {
                { "борщ", "Українська страва з буряка, капусти та м'яса." },
                { "паста", "Італійська страва з макаронів і соусу." },
                { "суші", "Японська страва з рису, риби і водоростей." },
                { "піца", "Італійська випічка з тіста, соусу та сиру." },
                { "вареники", "Українська страва з тіста з начинкою." },
                { "салат", "Страва зі свіжих овочів або фруктів." },
                { "бургер", "Американська страва з булки, м'яса і соусу." }
            };

            dishNames = new List<string>(dishImages.Keys);
            currentLevel = 0;
            attempts = 3;

            this.Text = "Вгадай страву";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Вопрос
            questionLabel = new Label
            {
                Text = "Що зображено на рисунку?",
                Location = new Point(20, 10),
                Size = new Size(400, 30),
                Font = new Font("Arial", 14, FontStyle.Bold)
            };
            this.Controls.Add(questionLabel);

            // Лейбл для подсказки
            hintLabel = new Label
            {
                Location = new Point(20, 40),
                Size = new Size(450, 50),
                Font = new Font("Arial", 10, FontStyle.Italic),
                ForeColor = Color.DarkGray
            };
            this.Controls.Add(hintLabel);

            // PictureBox для отображения блюда
            dishPictureBox = new PictureBox
            {
                Location = new Point(20, 100),
                Size = new Size(300, 200),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(dishPictureBox);

            // Текстовое поле для ответа
            answerTextBox = new TextBox
            {
                Location = new Point(20, 320),
                Size = new Size(200, 30)
            };
            this.Controls.Add(answerTextBox);

            // Кнопка проверки
            checkButton = new Button
            {
                Text = "Перевірити",
                Location = new Point(230, 320),
                Size = new Size(100, 30)
            };
            checkButton.Click += CheckAnswer;
            this.Controls.Add(checkButton);

            // Метка попыток
            attemptsLabel = new Label
            {
                Text = $"Залишилось спроб: {attempts}",
                Location = new Point(20, 360),
                Size = new Size(200, 30)
            };
            this.Controls.Add(attemptsLabel);

            // Метка результата
            resultLabel = new Label
            {
                Location = new Point(20, 400),
                Size = new Size(400, 30),
                ForeColor = Color.Blue
            };
            this.Controls.Add(resultLabel);

            StartLevel();
        }

        private void StartLevel()
        {
            if (currentLevel < dishNames.Count)
            {
                correctDish = dishNames[currentLevel];
                attempts = 3;
                attemptsLabel.Text = $"Залишилось спроб: {attempts}";
                resultLabel.Text = "";

                hintLabel.Text = $"Підказка: {dishHints[correctDish]}";
                dishPictureBox.Image = Image.FromFile(dishImages[correctDish]);
            }
            else
            {
                resultLabel.Text = "Вітаємо! Ви пройшли всі рівні!";
                resultLabel.ForeColor = Color.Green;
                checkButton.Enabled = false;
            }
        }

        private void CheckAnswer(object? sender, EventArgs e)
        {
            string userAnswer = answerTextBox.Text.Trim().ToLower();

            if (userAnswer == correctDish.ToLower())
            {
                resultLabel.Text = "Правильно! Переходьте до наступного рівня.";
                resultLabel.ForeColor = Color.Green;
                currentLevel++;
                StartLevel();
            }
            else
            {
                attempts--;
                if (attempts > 0)
                {
                    resultLabel.Text = "Неправильно. Спробуйте ще раз.";
                    attemptsLabel.Text = $"Залишилось спроб: {attempts}";
                }
                else
                {
                    resultLabel.Text = $"Ви не вгадали. Правильна відповідь: {correctDish}. Переходьте далі.";
                    resultLabel.ForeColor = Color.Red;
                    currentLevel++;
                    StartLevel();
                }
            }

            answerTextBox.Clear();
        }
    }
}
