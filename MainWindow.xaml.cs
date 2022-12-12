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
        // Variables
        bool goLeft, goRight = false;                          // Movement
        
        List<Rectangle> itemsToRemove = new List<Rectangle>(); // Garbage collector
        ImageBrush playerSkin = new ImageBrush();              // Player skin image

        int enemies = 0;                                       // Enemy counter
        int totalEnemies;                                      // Total enemies
        int enemySpeed = 10;                                   // Enemy speed

        int bulletTimer;                                       // Bullet timer
        int bulletTimerLimit = 90;                             // Bullet timer limit

        // Timer
        DispatcherTimer dispatcherTimer = new DispatcherTimer(); 

        public MainWindow()
        {
            InitializeComponent();

            // Configuring the Timer and Starting it.
            dispatcherTimer.Tick += gameEngine;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20);
            dispatcherTimer.Start();

            // PlayerSkin
            playerSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/player.png"));
            PlayerRec.Fill = playerSkin;

            // Making the enemies (spawns a random amount of enemies)
            Random rng = new Random();
            SpawnEnemies(rng.Next(30, 100));
        }

        /**
         * @brief
         * Handles the movement of the player when AD keys are pressed.
         * @param sender The object that sent the event.
         * @param e The event arguments.
         * return void 
         */
        private void IsKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D)
                goRight = true;

            if (e.Key == Key.A)
                goLeft = true;
        }

        /**
         * @brief
         * Stops the movement of the player when AD keys are released.
         * @param sender The object that sent the event.
         * @param e The event arguments.
         * @return void
         */
        private void IsKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D)
                goRight = false;

            if (e.Key == Key.A)
                goLeft = false;

            MakeBullet(sender, e);
        }

        /**
         * @brief
         * Makes the player's bullet.
         * @param sender The object that sent the event.
         * @param e The event arguments.
         * @return void
         */
        private void MakeBullet(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && bulletTimer > bulletTimerLimit)
            {
                Rectangle newBullet = new Rectangle
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Red
                };

                // Placing the bullet where the player is
                Canvas.SetTop(newBullet, Canvas.GetTop(PlayerRec) - newBullet.Height);
                Canvas.SetLeft(newBullet, Canvas.GetLeft(PlayerRec) + PlayerRec.Width / 2);

                // Adding the bullet to the canvas
                GameCanvas.Children.Add(newBullet);
            }
        }

        /**
         * @brief
         * Makes the bullets that fires back at the player. 
         * @param x double The x position of the bullet.
         * @param y double The y position of the bullet.
         * @return void
         */
        private void SpawnEnemyBullets(double x, double y)
        {
            Rectangle newEnemyBullet = new Rectangle
            {
                Tag = "enemyBullet",
                Height = 40,
                Width = 15, 
                Fill = Brushes.Yellow, 
                Stroke = Brushes.Black, 
                StrokeThickness = 5
            };

            // Placing the bullets
            Canvas.SetTop(newEnemyBullet, y);
            Canvas.SetLeft(newEnemyBullet, x);

            // Adding the bullets to the canvas
            GameCanvas.Children.Add(newEnemyBullet);
        }

        /**
         * @brief
         * Creates and spawns the enemies on the canvas.
         * @param limit int The amount of enemies to spawn.
         */
        private void SpawnEnemies(int limit)
        {
            int enemiesLeft = 0;
            totalEnemies = limit;

            // Spawning the enemies
            for (int i = 0; i < limit; i++)
            {
                // New enemy skin
                ImageBrush enemySkin = new ImageBrush();

                // Enemy Rectangle
                Rectangle newEnemy = new Rectangle
                {
                    Tag = "enemy",
                    Height = 45,
                    Width = 45,
                    Fill = enemySkin,
                };

                // Setting the enemy location
                Canvas.SetTop(newEnemy, 10);
                Canvas.SetLeft(newEnemy, enemiesLeft);

                // Adding the enemy to the canvas
                GameCanvas.Children.Add(newEnemy);

                // Changing the enemiesLeft value
                enemiesLeft -= 60;

                // Adding 1 to the images counter
                enemies++;

                if (enemies > 8)
                    enemies = 1;

                // Setting the enemy skin
                switch (enemies)
                {
                    case 1:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/enemy1.png"));
                        break;
                        
                    case 2:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/enemy2.png"));
                        break;
                        
                    case 3:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/enemy3.png"));
                        break;
                        
                    case 4:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/enemy4.png"));
                        break;

                    case 5:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/enemy5.png"));
                        break;

                    case 6:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/enemy6.png"));
                        break;

                    case 7:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/enemy7.png"));
                        break;

                    case 8:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/enemy8.png"));
                        break;
                }
            }
        }

        private void gameEngine(object sender, EventArgs e)
        { }
    }
}
