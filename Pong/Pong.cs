
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.GameClasses;
using System;
using System.Net.Mime;
using System.Runtime.InteropServices.ComTypes;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Pong : Game
    {
        //Monogame sprites
        readonly GraphicsDeviceManager graphics;
        private SpriteFont cms;
        private SpriteFont arial;
        SpriteBatch spriteBatch;


        //Object variables
        Player redPlayer;
        Player bluePlayer;
        Ball ball;
        Settings st;

        //variables for the lives rectangles, this is just stretching a single pixel to be our rectangle
        Texture2D single_pixel;

        //variables for gameStates
        private bool player_turn;
        private bool has_moved;
        private bool running;

        //the actual gamestates
        enum GameStates
        {
            Menu,
            GameOver,
            Game,
            Settings
        }

        GameStates currentState;
        AllSettings selectedSetting;
        KeyboardState previousState;

        //random class
        readonly static Random rng = new Random();

        public Pong()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

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

            //create a new settings object
            st = new Settings();

            //create spritefont
            cms = Content.Load<SpriteFont>("ComicSans");
            arial = Content.Load<SpriteFont>("Menu");
            
            //set texturecolor
            single_pixel = new Texture2D(GraphicsDevice, 1, 1);
            single_pixel.SetData(new[] { Color.White });

            //set gameStates
            //true = blue | false = red
            player_turn = true;
            has_moved = false;

            currentState = GameStates.Menu;
            selectedSetting = 0;

            //initialize the ball's position and speed
            ResetBall();
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

            //set current keyboard state
            KeyboardState state = Keyboard.GetState();

            //Exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            //change what happens based on the current gamestate
            switch(currentState)
            {
                case GameStates.Menu:
                    //MAIN MENU
                    if (state.IsKeyDown(Keys.Space))
                    {
                        currentState = GameStates.Game;
                    }
                    if (state.IsKeyDown(Keys.S))
                    {
                        currentState = GameStates.Settings;
                    }
                    break;
                case GameStates.GameOver:
                    //GAME OVER
                    running = false;
                    if (state.IsKeyDown(Keys.Enter))
                    {
                        currentState = GameStates.Menu;
                    }
                    if (state.IsKeyDown(Keys.Space))
                    {
                        currentState = GameStates.Game;
                        ResetBall();
                    }
                    break;
                case GameStates.Game:
                    //GAME IS RUNNING
                    if(!running)
                    {
                        running = true;
                        bluePlayer.SetLives(st.lives);
                        redPlayer.SetLives(st.lives);
                    }
                    RunGame();
                    break;
                case GameStates.Settings:
                    if (state.IsKeyDown(Keys.Enter))
                    {
                        currentState = GameStates.Menu;
                    }

                    if (state.IsKeyDown(Keys.Left) && !previousState.IsKeyDown(Keys.Left))
                    {
                        st.ChangeSetting(false, selectedSetting);
                    }
                    if (state.IsKeyDown(Keys.Right) && !previousState.IsKeyDown(Keys.Right))
                    {
                        st.ChangeSetting(true, selectedSetting);
                    }
                    if (state.IsKeyDown(Keys.Up) && !previousState.IsKeyDown(Keys.Up))
                    {
                        if (selectedSetting > 0)
                        {
                            selectedSetting--;
                        }
                    }
                    if (state.IsKeyDown(Keys.Down) && !previousState.IsKeyDown(Keys.Down))
                    {
                        if ((int) selectedSetting < Enum.GetValues(typeof(AllSettings)).Length-1)
                        {
                            selectedSetting++;
                        }
                    }
                    break;
            }

            //set previous keyboard state, so we can make sure the settings only activate once per button press
            previousState = state;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // change background color
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            switch (currentState)
            {
                case GameStates.Menu:
                    //MAIN MENU
                    DrawCenteredString(cms, "PONG", new Vector2(450, 200), Color.White);

                    //Main menu text
                    DrawCenteredString(arial, "Press [Space] to start", new Vector2(450,340),Color.White);
                    DrawCenteredString(arial, "or press [S] to go to settings", new Vector2(450, 370), Color.White);
                    DrawCenteredString(arial, "Controls are {W,S} and {ArrowUp, ArrowDown} for the players", new Vector2(450,400), Color.Gray);

                    break;
                case GameStates.GameOver:
                    //GAME OVER
                    DrawCenteredString(cms, "PONG", new Vector2(450,200), Color.White);

                    //resulting text
                    DrawCenteredString(arial, "Game over, " + (player_turn ? "Red" : "Blue") + " wins!", new Vector2(450,340), Color.White);

                    //Help text
                    DrawCenteredString(arial, "Press [Enter] to return to the menu", new Vector2(450, 400), Color.Gray);
                    DrawCenteredString(arial, "or press [Space] to play again", new Vector2(450, 430), Color.Gray);
                    break;

                case GameStates.Settings:
                    //SETTINGS
                    DrawCenteredString(arial, "Settings", new Vector2(450, 60), Color.Gray);

                    //SELECTOR
                    DrawCenteredString(arial, "______________", new Vector2(450, 100 + 40 * (int)selectedSetting), Color.Gray);

                    //The settings
                    DrawCenteredString(arial, "Paddle speed <" + st.paddle_speed + ">", new Vector2(450, 100), Color.White);
                    DrawCenteredString(arial, "Speed multiplier on bounce <" + st.bounce_increase + ">", new Vector2(450, 140), Color.White);
                    DrawCenteredString(arial, "Starting speed of ball <" + st.ball_defaultspeed + ">", new Vector2(450, 180), Color.White);
                    DrawCenteredString(arial, "Amount of lives <" + st.lives + ">", new Vector2(450, 220), Color.White);

                    //Help text
                    DrawCenteredString(arial, "Use [Arrow keys] to change the values", new Vector2(450, 460), Color.Gray);
                    DrawCenteredString(arial, "Press [Enter] to return to the menu", new Vector2(450, 500), Color.Gray);

                    break;
                
                case GameStates.Game:
                    //GAME IS RUNNING
                    //draw middle_line
                    spriteBatch.Draw(single_pixel, new Rectangle(450, 0, 1, 600), Color.DarkGray);

                    //draw ball and players
                    spriteBatch.Draw(ball.GetImage(), new Rectangle((int)ball.GetPosition().X, (int)ball.GetPosition().Y, ball.Width, ball.Height), ball.GetColor());
                    spriteBatch.Draw(redPlayer.GetImage(), new Rectangle((int)redPlayer.GetPosition().X, (int)redPlayer.GetPosition().Y, redPlayer.Width, redPlayer.Height), redPlayer.GetColor());
                    spriteBatch.Draw(bluePlayer.GetImage(), new Rectangle((int)bluePlayer.GetPosition().X, (int)bluePlayer.GetPosition().Y, bluePlayer.Width, bluePlayer.Height), bluePlayer.GetColor());

                    //drawing red player's lives
                    for (int i = 0; i < redPlayer.GetLives(); i++)
                    {
                        //make sure we dont overflow on the paddle, and instead just draw the remaining lives on the top of the screen
                        if (i < 3)
                        {
                            spriteBatch.Draw(single_pixel, new Rectangle(4, (int)redPlayer.GetPosition().Y + 31 + i * 12, 8, 8), redPlayer.GetColor());
                        } else
                        {
                            spriteBatch.Draw(single_pixel, new Rectangle(20 + i * 12, 4, 8, 8), redPlayer.GetColor());
                        }
                    }

                    //drawing blue player's lives
                    for (int i = 0; i < bluePlayer.GetLives(); i++)
                    {
                        //make sure we dont overflow on the paddle, and instead just draw the remaining lives on the top of the screen
                        if (i < 3)
                        {
                            spriteBatch.Draw(single_pixel, new Rectangle((int)900 - bluePlayer.Width + 4, (int)bluePlayer.GetPosition().Y + 31 + i * 12, 8, 8), bluePlayer.GetColor());
                        } else
                        {
                            spriteBatch.Draw(single_pixel, new Rectangle(868 - i * 12, 4, 8, 8), bluePlayer.GetColor());
                        }
                    }

                    //create an indication where the ball will move if the game hasn't started yet
                    if (!has_moved)
                    {
                        for (int i = 1; i <= 5; i++)
                        {
                            Vector2 nextpos = Vector2.Add(ball.GetPosition(), Vector2.Multiply(ball.GetSpeed(), (float)st.ball_defaultspeed * st.bounce_speed * i * 5));
                            spriteBatch.Draw(ball.GetImage(), new Rectangle((int)nextpos.X + ball.Width / 2, (int)nextpos.Y + ball.Height / 2, ball.Width / 3, ball.Height / 3), ball.GetColor());
                        }
                    }
                    break;
            }
                      

            spriteBatch.End();
            base.Draw(gameTime);
        }

        //reset the ball's position and speed
        private void ResetBall(bool isRed = false)
        {
            has_moved = false;
            st.bounce_speed = 1;

            //give the ball to the player where the ball has passed
            if (isRed)
            {
                player_turn = false;
                ball.SetSpeed(new Vector2((float)-rng.NextDouble() * 2, (float)rng.NextDouble() * 2 - 1));
                if(ball.GetSpeed().X < 0.2)
                {
                    ball.SetSpeed(new Vector2(-0.2f, 0.8f));
                }
            }
            else
            {
                player_turn = true;
                ball.SetSpeed(new Vector2((float)rng.NextDouble() * 2, (float)rng.NextDouble() * 2 - 1));
                if (ball.GetSpeed().X < 0.2)
                {
                    ball.SetSpeed(new Vector2(0.2f, 0.8f));
                }
            }

            //set new position in the middle of the screen
            ball.SetPosition(new Vector2(450-ball.Width/2, 300-ball.Height/2));
            
        }

        //return coordinates for the center of a string, instead of the upper-left corner
        private Vector2 CenterString(SpriteFont font, String text)
        {
            return new Vector2((font.MeasureString(text).X / 2), (font.MeasureString(text).Y / 2));
        }

        //draw a string, centered on its position.
        private void DrawCenteredString(SpriteFont font, String text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, text, Vector2.Subtract(position, CenterString(font, text)), color);
        }

        //the function that runs every tick while the game is running
        private void RunGame()
        {
            //reset ball and remove a life when ball passes paddle on the left side
            if (ball.GetPosition().X < -20)
            {
                redPlayer.SetLives(redPlayer.GetLives() - 1);
                ResetBall(true);

                //if out of lives
                if(redPlayer.GetLives() < 1)
                {
                    currentState = GameStates.GameOver;
                    redPlayer.SetLives(3);
                    bluePlayer.SetLives(3);
                }
            }

            //reset ball and remove a life when ball passes paddle on the right side
            if (ball.GetPosition().X > 920)
            {
                bluePlayer.SetLives(bluePlayer.GetLives() - 1);
                ResetBall(false);

                //if out of lives
                if (bluePlayer.GetLives() < 1)
                {
                    currentState = GameStates.GameOver;
                    redPlayer.SetLives(3);
                    bluePlayer.SetLives(3);
                }
            }

            //set the current keyboard state
            KeyboardState state = Keyboard.GetState();

            //RED PLAYER UP
            if (state.IsKeyDown(Keys.W) && redPlayer.GetPosition().Y > 0)
            {
                redPlayer.SetPosition(new Vector2(0, (float)(redPlayer.GetPosition().Y - st.paddle_speed)));
                has_moved = true;
            }
            //RED PLAYER DOWN
            if (state.IsKeyDown(Keys.S) && redPlayer.GetPosition().Y < 600 - redPlayer.Height)
            {
                redPlayer.SetPosition(new Vector2(0, (float)(redPlayer.GetPosition().Y + st.paddle_speed)));
                has_moved = true;
            }
            //BLUE PLAYER UP
            if (state.IsKeyDown(Keys.Up) && bluePlayer.GetPosition().Y > 0)
            {
                bluePlayer.SetPosition(new Vector2(900 - bluePlayer.Width, (float)(bluePlayer.GetPosition().Y - st.paddle_speed)));
                has_moved = true;
            }
            //BLUE PLAYER DOWN
            if (state.IsKeyDown(Keys.Down) && bluePlayer.GetPosition().Y < 600 - bluePlayer.Height)
            {
                bluePlayer.SetPosition(new Vector2(900 - bluePlayer.Width, (float)(bluePlayer.GetPosition().Y + st.paddle_speed)));
                has_moved = true;
            }

            //change ball position
            if (has_moved)
            {
                ball.SetPosition(Vector2.Add(ball.GetPosition(), Vector2.Multiply(ball.GetSpeed(), (float)st.ball_defaultspeed * st.bounce_speed)));
            }

            //check boundaries of ball position vertically
            if (ball.GetPosition().Y >= 600 - ball.Height || ball.GetPosition().Y <= 0)
            {
                ball.InvertY();
            }

            //bounce on the red paddle
            if (player_turn == false && ball.GetPosition().X < redPlayer.Width && ball.GetPosition().Y > redPlayer.GetPosition().Y - ball.Height && ball.GetPosition().Y < redPlayer.GetPosition().Y + redPlayer.Height)
            {
                //calculate new direction
                double DistanceToMiddle = (redPlayer.GetPosition().Y + redPlayer.Height / 2) - (ball.GetPosition().Y + ball.Height / 2);
                double normalizedDistance = DistanceToMiddle / (redPlayer.Height / 2);

                //set new direction
                ball.SetSpeed(new Vector2((float)Math.Cos(normalizedDistance), (float)-Math.Sin(normalizedDistance)));

                //switch player turn
                player_turn = !player_turn;

                //speed up the ball on bounce
                st.bounce_speed *= st.bounce_increase;
            }

            //bounce on the blue paddle
            if (player_turn == true && ball.GetPosition().X > 900 - bluePlayer.Width - ball.Width && ball.GetPosition().X < 900 && ball.GetPosition().Y > bluePlayer.GetPosition().Y - ball.Height && ball.GetPosition().Y < bluePlayer.GetPosition().Y + bluePlayer.Height)
            {
                //calculate new direction
                double DistanceToMiddle = (bluePlayer.GetPosition().Y + bluePlayer.Height / 2) - (ball.GetPosition().Y + ball.Height / 2);
                double normalizedDistance = DistanceToMiddle / (bluePlayer.Height / 2);

                //set new direction
                ball.SetSpeed(new Vector2((float)-Math.Cos(normalizedDistance), (float)-Math.Sin(normalizedDistance)));

                //switch player turn
                player_turn = !player_turn;

                //speed up the ball on bounce
                st.bounce_speed *= st.bounce_increase;
            }
        }
    }
}
