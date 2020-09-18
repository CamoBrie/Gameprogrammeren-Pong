using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.GameClasses
{
    class Ball
    {
        private Vector2 position;
        private Vector2 speed;
        private Texture2D image;
        private Color color;

        public int Width;
        public int Height;
        public Ball(Vector2 startingPosition, Vector2 startingSpeed, Texture2D image, Color? color)
        {
            if (!color.HasValue)
            {
                this.color = Color.White;
            }
            else
            {
                this.color = (Color)color;
            }

            this.position = startingPosition;
            this.speed = startingSpeed;
            this.image = image;

            this.Width = this.image.Width;
            this.Height = this.image.Height;
        }

        public void invertY()
        {
            this.speed.Y = -this.speed.Y;
        }

        public void invertX()
        {
            this.speed.X = -this.speed.X;
        }

        //SETTERS
        public void setPosition(Vector2 newPosition)
        {
            this.position = newPosition;
        }
        public void setSpeed(Vector2 newSpeed)
        {
            this.position = newSpeed;
        }


        //GETTERS
        public Vector2 getPosition()
        {
            return this.position;
        }

        public Vector2 getSpeed() {
            this.speed.Normalize();
            return this.speed;
        }
        public Texture2D getImage()
        {
            return this.image;
        }

        public Color GetColor()
        {
            return this.color;
        }
    }
}
