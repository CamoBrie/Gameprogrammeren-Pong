using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Net.Mime;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Pong : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //variables for the game sprites
        Texture2D ball;
        Texture2D red_player;
        Texture2D blue_player;

        //variables for the lives pixels
        Texture2D single_pixel;

        //variables for the positions
        private double red_position;
        private double blue_position;
        private Vector2 ball_position;

        //variables for speed
        private double paddle_speed;
        private double ball_defaultspeed;
        private float bounce_increase;

        //variables for angles
        private Vector2 ball_speed;

        //random class
        static Random rng = new Random();
        private double divider;

        //variable for player lives
        private int red_lives;
        private int blue_lives;

        public Pong()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Change window size to a larger size.
            graphics.PreferredBackBufferWidth = 900;  
            graphics.PreferredBackBufferHeight = 600;   
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ball = Content.Load<Texture2D>("ball");
            red_player = Content.Load<Texture2D>("red_player");
            blue_player = Content.Load<Texture2D>("blue_player");

            //set texturecolor
            single_pixel = new Texture2D(GraphicsDevice, 1, 1);
            single_pixel.SetData(new[] { Color.White });

            //set default variables
            red_position = 300.0 - red_player.Height/2;
            blue_position = 300.0 - blue_player.Height / 2;
            red_lives = 3;
            blue_lives = 3;
            paddle_speed = 10.0;
            bounce_increase = 1.01f;

            ball_defaultspeed = 7.0;
            resetBall();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //reset ball and remove a life when ball passes paddle
            if (ball_position.X < -100)
            {
                red_lives--;
                resetBall();
            }
            
            if (ball_position.X > 1000)
            {
                blue_lives--;
                resetBall();
            }

            //Exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            //RED PLAYER UP
            if(Keyboard.GetState().IsKeyDown(Keys.W) && red_position > 0)
            {
                red_position -= paddle_speed;
            }
            //RED PLAYER DOWN
            if (Keyboard.GetState().IsKeyDown(Keys.S) && red_position < 600 - red_player.Height)
            {
                red_position += paddle_speed;
            }
            //BLUE PLAYER UP
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && blue_position > 0)
            {
                blue_position -= paddle_speed;
            }
            //BLUE PLAYER DOWN
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && blue_position < 600 - blue_player.Height)
            {
                blue_position += paddle_speed;
            }

            //change ball position
            ball_position = Vector2.Add(ball_position, ball_speed);

            //check boundaries of ball position
            if(ball_position.Y >= 600 - ball.Height || ball_position.Y <= 0)
            {
                ball_speed.Y *= -1;
            }

            if(ball_position.X < red_player.Width && ball_position.Y > red_position && ball_position.Y < red_position + red_player.Height)
            {   
                //TODO: change the ball's trajectory based on where it hits the paddle
                ball_speed.X *= -1;

                //speed up the ball on bounce
                ball_speed.X *= bounce_increase;
                ball_speed.Y *= bounce_increase;
            }
      
            if (ball_position.X > 900 - blue_player.Width - ball.Width && ball_position.X < 900 && ball_position.Y > blue_position && ball_position.Y < blue_position + blue_player.Height)
            {
                //TODO: change the ball's trajectory based on where it hits the paddle
                ball_speed.X *= -1;

                //speed up the ball on bounce
                ball_speed.X *= bounce_increase;
                ball_speed.Y *= bounce_increase;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // change background color accordingly with new pngs
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(ball, new Rectangle((int) ball_position.X, (int) ball_position.Y, ball.Width, ball.Height), Color.White);
            spriteBatch.Draw(red_player, new Rectangle(0, (int) red_position, red_player.Width, red_player.Height), Color.IndianRed);
            spriteBatch.Draw(blue_player, new Rectangle(900 - blue_player.Width, (int) blue_position, blue_player.Width, blue_player.Height), Color.CornflowerBlue);

            //drawing red player's lives
            for(int i = 0; i < red_lives; i++)
            {
                spriteBatch.Draw(single_pixel, new Rectangle(4, (int)red_position + 31 + i * 12, 8, 8), Color.IndianRed);
            }

            //drawing blue player's lives
            for (int i = 0; i < blue_lives; i++)
            {
                spriteBatch.Draw(single_pixel, new Rectangle(900 - blue_player.Width + 4, (int)blue_position + 31 + i * 12, 8, 8), Color.CornflowerBlue);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void resetBall()
        {
            divider = rng.NextDouble();
            ball_speed = new Vector2((float)(ball_defaultspeed * divider + ball_defaultspeed * 0.25), (float)(ball_defaultspeed * (1 - divider) + ball_defaultspeed * 0.25));
            ball_position = new Vector2(450 - ball.Width / 2, 300 - ball.Height / 2);

            if (rng.Next(0, 2) == 1)
            {
                ball_speed.X *= -1;
            }
            if (rng.Next(0, 2) == 1)
            {
                ball_speed.Y *= -1;
            }
        }
    }
}
