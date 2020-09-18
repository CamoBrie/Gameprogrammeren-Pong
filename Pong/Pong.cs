using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.GameClasses;
using System;
using System.Net.Mime;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Pong : Game
    {
        //Monogame sprites
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Object variables
        Player redPlayer;
        Player bluePlayer;
        Ball ball;

        //variables for the lives rectangles
        Texture2D single_pixel;


        //variables for speed
        private double paddle_speed;
        private double ball_defaultspeed;
        private float bounce_increase;
        private float bounce_speed;

        //variables for gameStates
        private bool player_turn;

        //random class
        readonly static Random rng = new Random();

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

            //create a new ball(starting position, starting speed, image, colortint)
            ball = new Ball(new Vector2(450,300), new Vector2(3.0f, 3.0f), Content.Load<Texture2D>("ball"), Color.White);

            redPlayer = new Player(3, Color.IndianRed, Content.Load<Texture2D>("red_player"), new Vector2(0, 300),"West");
            bluePlayer = new Player(3, Color.CornflowerBlue, Content.Load<Texture2D>("blue_player"), new Vector2(884, 300), "East");

            //set texturecolor
            single_pixel = new Texture2D(GraphicsDevice, 1, 1);
            single_pixel.SetData(new[] { Color.White });

            //set default variables
            paddle_speed = 10.0;
            bounce_increase = 1.005f;
            bounce_speed = 1;

            //set gameStates
            //true = blue | false = red
            player_turn = true;

            ball_defaultspeed = 7.0;
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
            if (ball.getPosition().X < -20)
            {
                redPlayer.setLives(redPlayer.getLives() - 1);
                resetBall();

            }
            
            if (ball.getPosition().X > 920)
            {
                bluePlayer.setLives(bluePlayer.getLives() - 1);
                resetBall();

            }

            //Exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            //RED PLAYER UP
            if(Keyboard.GetState().IsKeyDown(Keys.W) && redPlayer.getPosition().Y > 0)
            {
                redPlayer.setPosition(new Vector2(0, (float) (redPlayer.getPosition().Y - paddle_speed)));
            }
            //RED PLAYER DOWN
            if (Keyboard.GetState().IsKeyDown(Keys.S) && redPlayer.getPosition().Y < 600 - redPlayer.Height)
            {
                redPlayer.setPosition(new Vector2(0, (float)(redPlayer.getPosition().Y + paddle_speed)));
            }
            //BLUE PLAYER UP
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && bluePlayer.getPosition().Y > 0)
            {
                bluePlayer.setPosition(new Vector2(880, (float)(bluePlayer.getPosition().Y - paddle_speed)));
            }
            //BLUE PLAYER DOWN
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && bluePlayer.getPosition().Y < 600 - bluePlayer.Height)
            {
                bluePlayer.setPosition(new Vector2(880, (float)(bluePlayer.getPosition().Y + paddle_speed)));
            }

            //change ball position
            ball.setPosition(Vector2.Add(ball.getPosition(), Vector2.Multiply(ball.getSpeed(), (float)ball_defaultspeed * bounce_speed)));

            //check boundaries of ball position
            if(ball.getPosition().Y >= 600 - ball.Height || ball.getPosition().Y <= 0)
            {
                ball.invertY();
            }

            if(player_turn == false && ball.getPosition().X < redPlayer.Width && ball.getPosition().Y > redPlayer.getPosition().Y - ball.Height && ball.getPosition().Y < redPlayer.getPosition().Y + redPlayer.Height)
            {
                ball.invertX();

                //double change = ((redPlayer.getPosition().Y + redPlayer.Height / 2) - (ball.getPosition().Y + ball.Height / 2));
                //ball.setSpeed(new Vector2(0, ball.getSpeed().Y - (float) (change * 0.1)));

                

                //switch player turn
                player_turn = !player_turn;

                //speed up the ball on bounce
                bounce_speed *= bounce_increase;

                //ball.setSpeed(new Vector2(ball.getSpeed().X * bounce_increase, ball.getSpeed().Y * bounce_increase));

            }
      
            if (player_turn == true && ball.getPosition().X > 900 - bluePlayer.Width - ball.Width && ball.getPosition().X < 900 && ball.getPosition().Y > bluePlayer.getPosition().Y - ball.Height && ball.getPosition().Y < bluePlayer.getPosition().Y + bluePlayer.Height)
            {
                //ball.setSpeed(new Vector2(0, ball.getSpeed().X * -1));
                ball.invertX();

                //double change = ((bluePlayer.getPosition().Y + bluePlayer.Height / 2) - (ball.getPosition().Y + ball.Height / 2));
                //ball.setSpeed(new Vector2(0, ball.getSpeed().Y - (float)(change * 0.1)));

                //ball.getSpeed().X *= (float)(-1 + 1/(Math.Abs(change)/23));
                //switch player turn
                player_turn = !player_turn;

                //speed up the ball on bounce
                bounce_speed *= bounce_increase;
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

            spriteBatch.Draw(ball.getImage(), new Rectangle((int) ball.getPosition().X, (int) ball.getPosition().Y, ball.Width, ball.Height), ball.GetColor());
            spriteBatch.Draw(redPlayer.getImage(), new Rectangle((int) redPlayer.getPosition().X, (int) redPlayer.getPosition().Y, redPlayer.Width, redPlayer.Height), redPlayer.getColor());
            spriteBatch.Draw(bluePlayer.getImage(), new Rectangle((int) bluePlayer.getPosition().X, (int) bluePlayer.getPosition().Y, bluePlayer.Width, bluePlayer.Height), bluePlayer.getColor());

            //drawing red player's lives
            for(int i = 0; i < redPlayer.getLives(); i++)
            {
                spriteBatch.Draw(single_pixel, new Rectangle(4, (int) redPlayer.getPosition().Y + 31 + i * 12, 8, 8), redPlayer.getColor());
            }

            //drawing blue player's lives
            for (int i = 0; i < bluePlayer.getLives(); i++)
            {
                spriteBatch.Draw(single_pixel, new Rectangle(900 - bluePlayer.Width + 4, (int) bluePlayer.getPosition().Y + 31 + i * 12, 8, 8), bluePlayer.getColor());
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void resetBall()
        {
            ball.setPosition(new Vector2(450, 300));
            ball.setSpeed(new Vector2(3.0f, 3.0f));
            new Vector2(450, 300);
        }
    }
}
