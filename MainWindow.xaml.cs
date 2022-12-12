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
        { }

        private void SpawnEnemies(int limit)
        { }

        private void gameEngine(object sender, EventArgs e)
        { }
    }
}
