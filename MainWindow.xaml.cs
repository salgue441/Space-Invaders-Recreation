using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SpaceInvaders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        // Movement variables
        private bool GoLeft = false, GoRight = false;
        private int PlayerSpeed = 10;
        private int PlayerLives = 4;

        // Score variable
        private int Score = 0;

        // Enemy variables
        private int EnemyImages = 0;                    // Holds the number of enemy images
        private int TotalEnemies;                       // Saves the total number of enemies
        private int EnemySpeed = 5;                     // Base speed of the enemies
        private int EnemyBulletTimer;                   // Enemy bullet timer
        private int EnemyBulletTimerLimit = 90;         // Limit and Frequency for enemie's bullet

        // Bullet variables
        private bool BulletFired = false;               // Checks if the bullet is fired
        private int BulletSpeed = 10;                   // Speed of the bullet

        // Garbage Collector
        private List<Rectangle> ItemsToRemove = new List<Rectangle>();

        // Dispatcher timer class
        private DispatcherTimer GameTimer = new DispatcherTimer();

        // Player Skin
        private ImageBrush PlayerSkin = new ImageBrush();

        public MainWindow()
        {
            InitializeComponent();

            // Set the game timer
            GameTimer.Tick += GameEngine;
            GameTimer.Interval = TimeSpan.FromMilliseconds(20);
            GameTimer.Start();

            // Player Skin
            PlayerSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/player.png"));
            PlayerRec.Fill = PlayerSkin;

            // Randomizing the enemy generation
            Random rng = new Random();
            SpawnEnemies(rng.Next(150, 1000));

            GameCanvas.Focus();
        }

        /**
         * @brief 
         * Handles the movement of the player when the A & D keys are pressed. 
         * @param sender The object that sent the event.
         * @param e The event arguments.
         * @return void 
         */
        private void IsKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
                GoLeft = true;

            if (e.Key == Key.D)
                GoRight = true;
        }

        /**
         * @brief
         * Handles the movement of the player when the A & D keys are released.
         * @param sender The object that sent the event.
         * @param e The event arguments.
         * @return void
         */
        private void IsKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
                GoLeft = false;

            if (e.Key == Key.D)
                GoRight = false;

            // Gives the player the ability to shoot
            if (e.Key == Key.Space)
                MakeBullet();
        }

        /**
         * @brief
         * Handles the generation of Player's bullet. 
         * @param none
         * @return void
         */
        private void MakeBullet()
        {
            // Clearing the bullet from the canvas
            ItemsToRemove.Clear();

            // Creates a new bullet
            Rectangle NewBullet = new Rectangle
            {
                Tag = "Bullet",
                Height = 20,
                Width = 5,
                Fill = Brushes.White,
                Stroke = Brushes.Red
            };

            // Sets the bullet's position
            Canvas.SetTop(NewBullet, Canvas.GetTop(PlayerRec) - NewBullet.Height);
            Canvas.SetLeft(NewBullet, Canvas.GetLeft(PlayerRec) + PlayerRec.Width / 2);

            // Adds the bullet to the canvas
            GameCanvas.Children.Add(NewBullet);   
        }

        /**
         * @brief
         * Handles the spawning of the enemies. The number of enemies spawned is determined
         * by the parameter.
         * @param EnemyLimit The number of enemies to be spawned.
         * @return void
         */
        private void SpawnEnemies(int EnemyLimit)
        {
            int EnemiesLeft = 0;
            TotalEnemies = EnemyLimit;

            for (int i = 0; i < EnemyLimit; i++)
            {
                // New skin for the enemy rectangle
                ImageBrush EnemySkin = new ImageBrush();

                // Making a new Enemy Rectangle
                Rectangle NewEnemy = new Rectangle
                {
                    Tag = "Enemy",
                    Height = 45,
                    Width = 45,
                    Fill = EnemySkin,
                };

                // Setting the enemy's position
                Canvas.SetTop(NewEnemy, 25);           // Top location
                Canvas.SetLeft(NewEnemy, EnemiesLeft); // Left location

                // Adding the enemy to the canvas
                GameCanvas.Children.Add(NewEnemy);

                // Incrementing the enemy's left position
                EnemiesLeft -= 60;

                // Incrementing the total number of enemies
                EnemyImages++;

                // Randomizing the skin selection
                if (EnemyImages > 8)
                {
                    Random rng_images = new Random();
                    EnemyImages = rng_images.Next(1, 8);
                    Console.WriteLine(rng_images);
                }

                RandomizeEnemySkin(EnemyImages, EnemySkin);
            }
        }

        /**
         * @brief
         * Randomizes the skin selection for the enemies.
         * @param EnemyImages The number of enemies that have been spawned.
         * @param EnemySkin The skin of the enemy.
         * @return void
         */
        private void RandomizeEnemySkin(int EnemyImages, ImageBrush EnemySkin)
        {   
            switch (EnemyImages)
            {
                case 1:
                    EnemySkin.ImageSource 
                        = new BitmapImage(new Uri("pack://application:,,,/Images/invader1.gif"));
                    break;

                case 2:
                    EnemySkin.ImageSource
                        = new BitmapImage(new Uri("pack://application:,,,/Images/invader2.gif"));
                    break;

                case 3:
                    EnemySkin.ImageSource 
                        = new BitmapImage(new Uri("pack://application:,,,/Images/invader3.gif"));
                    break;

                case 4:
                    EnemySkin.ImageSource 
                        = new BitmapImage(new Uri("pack://application:,,,/Images/invader4.gif"));
                    break;

                case 5:
                    EnemySkin.ImageSource 
                        = new BitmapImage(new Uri("pack://application:,,,/Images/invader5.gif"));
                    break;

                case 6:
                    EnemySkin.ImageSource 
                        = new BitmapImage(new Uri("pack://application:,,,/Images/invader6.gif"));
                    break;

                case 7:
                    EnemySkin.ImageSource 
                        = new BitmapImage(new Uri("pack://application:,,,/Images/invader7.gif"));
                    break;

                case 8:
                    EnemySkin.ImageSource 
                        = new BitmapImage(new Uri("pack://application:,,,/Images/invader8.gif"));
                    break;
            }
        }

        /**
         * @brief 
         * Spawns enemy bullets. 
         * @param x X position of the Bullet.
         * @param y Y position of the Bullet.
         * @return void
         */
        private void SpawnEnemyBullets(double x, double y)
        {
            Rectangle newEnemyBullet = new Rectangle
            {
                Tag = "EnemyBullet",
                Height = 40,
                Width = 15,
                Fill = Brushes.Yellow,
                Stroke = Brushes.Black,
                StrokeThickness = 5
            };

            // Setting the bullet's position
            Canvas.SetTop(newEnemyBullet, y);
            Canvas.SetLeft(newEnemyBullet, x);

            // Adding the bullet to the canvas
            GameCanvas.Children.Add(newEnemyBullet);
        }

        /**
         * @brief
         * Checks if the Rectangle is a player's bullet. 
         * @param Bullet Rectangle to be checked.
         * @return true if the Rectangle is a bullet, false otherwise.
         */
        private bool IsBullet(Rectangle Bullet)
        {
            if (Bullet is Rectangle && (string)Bullet.Tag == "Bullet")
                return true;

            return false;
        }

        /**
         * @brief
         * Checks if the Rectangle is an Enemy. 
         * @param Enemy Rectangle to be checked. 
         * @return true if the Rectangle is an Enemy, false otherwise.
         */
        public bool IsEnemy(Rectangle Enemy)
        {
            if (Enemy is Rectangle && (string)Enemy.Tag == "Enemy")
                return true;

            return false;
        }

        /**
         * @brief
         * Checks if the Rectangle is an Enemy Bullet. 
         * @param EnemyBullet Rectangle to be checked
         * @return true if the Rectangle is an Enemy Bullet, false otherwise.
         */
        public bool IsEnemyBullet(Rectangle EnemyBullet)
        {
            if (EnemyBullet is Rectangle && (string)EnemyBullet.Tag == "EnemyBullet")
                return true;

            return false;
        }

        /**
         * @brief
         * Movemet Script for the player. 
         * @param none
         * @return void
         */
        private void PlayerMovement()
        {
            if (GoLeft && Canvas.GetLeft(PlayerRec) > 0)
                Canvas.SetLeft(PlayerRec, Canvas.GetLeft(PlayerRec) - PlayerSpeed);

            else if (GoRight && Canvas.GetLeft(PlayerRec) + 90 < Application.Current.MainWindow.Width)
                Canvas.SetLeft(PlayerRec, Canvas.GetLeft(PlayerRec) + PlayerSpeed);
        }

        /**
         * @brief
         * Handles the Player's Bullet Movement. 
         * @param none
         * @return void
         */
        private void BulletMovement()
        {
            // Looping through all the bullets
            foreach (var Bullet in GameCanvas.Children.OfType<Rectangle>())
            {
                if (IsBullet(Bullet))
                {
                    // Moving the bullet up
                    Canvas.SetTop(Bullet, Canvas.GetTop(Bullet) - 20);

                    // Checking if the bullet is out of the canvas
                    if (Canvas.GetTop(Bullet) < 10)
                        ItemsToRemove.Add(Bullet);
                }
            }
        }

        /**
         * @brief
         * Determines the enemy's speed based on the amount left. 
         * @param none
         * @return void
         */
        public void EnemySpeedHandler()
        {
            if (TotalEnemies < 100 || TotalEnemies < 50)
                EnemySpeed = 15;

            else if (TotalEnemies < 40)
                EnemySpeed = 18;

            else if (TotalEnemies < 30)
                EnemySpeed = 21;

            else if (TotalEnemies < 20)
                EnemySpeed = 24;

            else if (TotalEnemies < 10)
                EnemySpeed = 27;

            else if (TotalEnemies < 5)
                EnemySpeed = 30;
        }

        /**
         * @brief
         * Handles the Enemy movement. 
         * @param none
         * @return void
         */
        public void EnemyMovement()
        {
            EnemySpeedHandler();

            // Looping through all the enemies
            foreach (var Enemy in GameCanvas.Children.OfType<Rectangle>())
            {          
                if (IsEnemy(Enemy))
                {
                    // Moving the enemy left
                    Canvas.SetLeft(Enemy, Canvas.GetLeft(Enemy) + EnemySpeed);

                    if (Canvas.GetLeft(Enemy) > 1205)
                    {
                        Canvas.SetLeft(Enemy, -80);
                        Canvas.SetTop(Enemy, Canvas.GetTop(Enemy) + (Enemy.Height + 10));
                    }

                }
            }
        }
        
        /**
         * @brief
         * Handles the Enemy Bullet Generation and Movement.
         * @param none
         * @return void
         */
        public void EnemyBulletMovement()
        {
            EnemyBulletTimer -= 3;

            if (EnemyBulletTimer < 0)
            {
                SpawnEnemyBullets((Canvas.GetLeft(PlayerRec) + 20), 10);
                EnemyBulletTimer = EnemyBulletTimerLimit;
            }

            foreach (var EnemyBullet in GameCanvas.Children.OfType<Rectangle>())
            {
                if (IsEnemyBullet(EnemyBullet))
                {
                    // Moving the bullets down
                    Canvas.SetTop(EnemyBullet, Canvas.GetTop(EnemyBullet) + 10);

                    if (Canvas.GetTop(EnemyBullet) > 720)
                        ItemsToRemove.Add(EnemyBullet);
                }
            }
        }

        /**
         * @brief
         * Handles the collision between the Rectangles in the game.
         * @param none
         * @return void
         */
        public void CollissionDetection()
        {
            Rect PlayerHitBox = new Rect(Canvas.GetLeft(PlayerRec), Canvas.GetTop(PlayerRec),
                PlayerRec.Width, PlayerRec.Height);

            // Player Bullet Collision
            foreach (var i in GameCanvas.Children.OfType<Rectangle>())
            {
                if (IsBullet(i))
                {
                    Rect BulletHitBox = new Rect(Canvas.GetLeft(i), Canvas.GetTop(i),
                        i.Width, i.Height);

                    foreach (var j in GameCanvas.Children.OfType<Rectangle>())
                    {
                        if (IsEnemy(j))
                        {
                            Rect EnemyHitBox = new Rect(Canvas.GetLeft(j), Canvas.GetTop(j),
                                j.Width, j.Height);

                            if (BulletHitBox.IntersectsWith(EnemyHitBox))
                            {
                                ItemsToRemove.Add(i);
                                ItemsToRemove.Add(j);

                                TotalEnemies -= 1;
                                Score += 1;
                            }
                        }
                    }
                }

                if (IsEnemy(i))
                {
                    Rect EnemyHitBox = new Rect(Canvas.GetLeft(i), Canvas.GetTop(i),
                        i.Width, i.Height);

                    if (PlayerHitBox.IntersectsWith(EnemyHitBox))
                        LooseGameOver();
                }

                if (IsEnemyBullet(i))
                {
                    Rect EnemyBulletHitBox = new Rect(Canvas.GetLeft(i), Canvas.GetTop(i),
                        i.Width, i.Height);

                    if (PlayerHitBox.IntersectsWith(EnemyBulletHitBox))
                    {
                        // Decrease player lives
                        PlayerLives -= 1;

                        // Remove the bullet
                        ItemsToRemove.Add(i);

                        // Check if the player is dead
                        if (PlayerLives == 0)
                            LooseGameOver();
                    }
                }
            }
        }
        
        /**
         * @brief
         * Shows the player a loose message and stops the game. 
         * @param none
         * @return void
         */
        private void LooseGameOver()
        {
            GameTimer.Stop();
            MessageBox.Show("Game Over! You loose");
        }

        /**
         * @brief
         * Shows the player a winning message and stops the game. 
         * @param none
         * @return void
         */
        private void WinGameOver()
        {
            EnemiesLeft.Content = "Invaders left: 0";
            
            GameTimer.Stop();
            MessageBox.Show("Game Over! You Win");
        }

        /**
         * @brief
         * Checks for every rectangle that has been added to the itemsToRemove list
         * @param none
         * @return void
         */
        private void GargabeCollection()
        {
            // Garbage collection loop
            foreach (Rectangle GarbageItems in ItemsToRemove)
                GameCanvas.Children.Remove(GarbageItems);
        }


        /**
         * @brief
         * Game's logic. Calls the respective methods for enemy movement, bullet movement,
         * enemy's bullet movement, player movement, and collision detection. This function
         * is called every 20 milliseconds.
         * @param sender The object that called the method.
         * @param e The event that called the method.
         * @return void
         */
        private void GameEngine(object sender, EventArgs e)
        {
            EnemiesLeft.Content = "Invaders left:  " + TotalEnemies;
            PlayersLives.Content = "Lives: " + PlayerLives;
            ScoreLabel.Content = "Score: " + Score;

            PlayerMovement();
            BulletMovement();
            EnemyMovement();
            EnemyBulletMovement();
            CollissionDetection();
            GargabeCollection();

            if (TotalEnemies == 0)
                WinGameOver();
        }
    }
}