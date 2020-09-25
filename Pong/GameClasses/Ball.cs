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
        private readonly Texture2D image;
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

        public void InvertY()
        {
            this.speed.Y = -this.speed.Y;
        }

        public void InvertX()
        {
            this.speed.X = -this.speed.X;
        }

        //SETTERS
        public void SetPosition(Vector2 newPosition)
        {
            this.position = newPosition;
        }
        public void SetSpeed(Vector2 newSpeed)
        {
            this.speed = newSpeed;
        }


        //GETTERS
        public Vector2 GetPosition()
        {
            return this.position;
        }

        public Vector2 GetSpeed() {
            this.speed.Normalize();
            return this.speed;
        }
        public Texture2D GetImage()
        {
            return this.image;
        }

        public Color GetColor()
        {
            return this.color;
        }
    }
}
