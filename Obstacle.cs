using System;
using SplashKitSDK;

abstract class Obstacle
{
    protected Bitmap _obstacleBitmap;
    public double X { get; private set; }
    public double Y { get; private set; }
    public Color Maincolor { get; private set; }
    private Vector2D Velocity { get; set; }
    private Vector2D WaveVelocity { get; set; }
    int falls = 0;
    private bool flyaway = false;
    public Circle CollisionCircle
    {
        get { return SplashKit.CircleAt((X + (Width / 2)), (Y + (Height / 2)), 20); }
    }


    public int Width
    {
        get { return 50; }
    }
    public int Height
    {
        get { return 50; }
    }

    public Obstacle(Window gameWindow, Player player)
    {
        if (SplashKit.Rnd() < 0.5)
        {
            X = SplashKit.Rnd(3 * (gameWindow.Width / 4));
            Y = gameWindow.Height;
        }
         else
        {
            Y = SplashKit.Rnd(gameWindow.Height);
            X = 0;
        }


        const int SPEED = 3;

        Point2D fromPt = new Point2D()
        {
            X = X,
            Y = Y
        };

        Point2D toPt = new Point2D()
        {
            X = player.X,
            Y = player.Y
        };

        Vector2D dir;
        dir = SplashKit.UnitVector(SplashKit.VectorPointToPoint(fromPt, toPt));
        Velocity = SplashKit.VectorMultiply(dir, SPEED);
    }

    public void WaveMotion(Wave wave)
    {
        
        if ((wave.lipX - X) < 10 && (Y - wave.lipY) < 10)
        {
            flyaway = true;
        }

        if (flyaway == false)
        {
            Point2D fromPt = new Point2D()
            {
                X = X,
                Y = Y
            };

            Point2D toPt = new Point2D()
            {
                X = wave.lipX,
                Y = wave.lipY
            };

            Vector2D dir;
            dir = SplashKit.UnitVector(SplashKit.VectorPointToPoint(fromPt, toPt));
            WaveVelocity = SplashKit.VectorMultiply(dir, (300 / ((wave.lipX - X) + (Y - wave.lipY)) + 2));
            X = X + WaveVelocity.X;
            Y = Y + WaveVelocity.Y;
        }
        else if (flyaway == true)
        {
            X += Math.Pow(10, falls / 5);
            Y += 30;
            falls ++;

        }

    }

    public void isHit(Window gameWindow)
    {
        _obstacleBitmap = new Bitmap("explosion", "explosion.png");
        gameWindow.DrawBitmap(_obstacleBitmap, X, Y);
        gameWindow.Refresh();
        SplashKit.Delay(100);
    }

    public bool IsOffScreen(Window screen)
    {
        return (X < -Width || X > screen.Width || Y < -Height || Y > screen.Height);

    }
    public void Update()
    {
        X = X + Velocity.X;

        Y = Y + Velocity.Y;

    }

    public abstract void Draw(Window gameWindow);


}

class Shark : Obstacle
{
    public Shark(Window gameWindow, Player player) : base(gameWindow, player)
    {
        _obstacleBitmap = new Bitmap("shark", "shark.png");
    }
    public override void Draw(Window gameWindow)
    {
        gameWindow.DrawBitmap(_obstacleBitmap, X, Y);

    }
}

class Turtle : Obstacle
{
    public Turtle(Window gameWindow, Player player) : base(gameWindow, player)
    {
        _obstacleBitmap = new Bitmap("turtle", "turtle.png");
    }
    public override void Draw(Window gameWindow)
    {
        gameWindow.DrawBitmap(_obstacleBitmap, X, Y);

    }
}

class BodyBoard : Obstacle
{
    public BodyBoard(Window gameWindow, Player player) : base(gameWindow, player)
    {
        _obstacleBitmap = new Bitmap("bodyboard", "bodyboard.png");
    }
    public override void Draw(Window gameWindow)
    {
        gameWindow.DrawBitmap(_obstacleBitmap, X, Y);

    }
}

class Waste : Obstacle
{
    public Waste(Window gameWindow, Player player) : base(gameWindow, player)
    {
        _obstacleBitmap = new Bitmap("waste", "waste.png");

    }
    public override void Draw(Window gameWindow)
    {
        gameWindow.DrawBitmap(_obstacleBitmap, X, Y);
    }
}