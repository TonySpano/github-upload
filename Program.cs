using System;
using SplashKitSDK;

public class Program
{
    public static void Main()
    {
        
        Window gameWindow = new Window("Surf Game", 1250, 650);    
        SurfGame  _game = new SurfGame(gameWindow);
    
        while (!gameWindow.CloseRequested && _game.Quit == false)
        {
            SplashKit.ProcessEvents();
            _game.HandleInput();
            _game.Update();
            _game.Draw();

        }

        _game.FinalScore();
        SplashKit.Delay(5000);
        gameWindow.Close();
        gameWindow = null;


    }


}