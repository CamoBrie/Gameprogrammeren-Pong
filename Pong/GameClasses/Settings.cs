using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pong.GameClasses
{

    enum AllSettings
    {
        paddle_speed,
        bounce_increase,
        ball_defaultspeed,
        lives
    }
    class Settings
    {
        public double paddle_speed;
        public float bounce_increase;
        public float bounce_speed;
        public double ball_defaultspeed;
        public int lives;

        public Settings(double paddle_speed = 10.0, float bounce_increase = 1.05f, double ball_defaultspeed = 7.0, float bounce_speed = 1, int lives = 3)
        {
            this.paddle_speed = paddle_speed;
            this.bounce_increase = bounce_increase;
            this.bounce_speed = bounce_speed;
            this.ball_defaultspeed = ball_defaultspeed;
            this.lives = lives;
        }

        public void ChangeSetting(bool increase = true, AllSettings currentSetting = 0)
        {
            switch (currentSetting)
            {
                case AllSettings.paddle_speed: //paddle_speed
                    if(increase)
                    {
                        if(!(this.paddle_speed>49))
                        {
                            this.paddle_speed++;
                        }

                    } else
                    {
                        if (!(this.paddle_speed < 2))
                        {
                            this.paddle_speed--;
                        }
                    }
                    
                    break;
                case AllSettings.bounce_increase: //bounce_increase
                    if (increase)
                    {
                        if (!(this.bounce_increase > 1.45f)) 
                        { 
                            this.bounce_increase += 0.05f;
                        }
                            
                    }
                    else
                    {
                        if (!(this.bounce_increase <= 1.05f)) 
                        { 
                            this.bounce_increase -= 0.05f;
                        }
                    }
                    break;

                case AllSettings.ball_defaultspeed: //ball_defaultspeed
                    if (increase)
                    {
                        if (!(this.ball_defaultspeed > 19)) {
                            this.ball_defaultspeed++;
                        }
                    }
                    else
                    {
                        if(!(this.ball_defaultspeed < 2))
                        this.ball_defaultspeed--;
                    }
                    break;
                case AllSettings.lives: //lives
                    if (increase)
                    {
                        if (!(this.lives > 9))
                        {
                            this.lives++;
                        }
                    }
                    else
                    {
                        if (!(this.lives < 2))
                            this.lives --;
                    }
                    break;

            }
        }
    }
}
