using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace CattyBasics;

static class Program
{
    static void Main()
    {
        Raylib.InitWindow(800, 600, "Catty's Basics");
        Raylib.SetTargetFPS(60);
        
        // Load cat image (put "catty.png" in the same folder as your .exe)
        Texture2D catTexture = Raylib.LoadTexture("catty.png");
        
        Vector2 playerPos = new(400, 300);
        Vector2 catPos = new(100, 100);
        float catSpeed = 1f;
        int wrongAnswers = 0;
        
        List<Vector2> notebooks = new() { new(200, 200), new(600, 400), new(300, 500) };
        bool showMath = false;
        int currentNotebook = -1;
        
        while (!Raylib.WindowShouldClose())
        {
            float dt = Raylib.GetFrameTime();
            
            // Player movement (WASD)
            if (Raylib.IsKeyDown(KeyboardKey.W)) playerPos.Y -= 300 * dt;
            if (Raylib.IsKeyDown(KeyboardKey.S)) playerPos.Y += 300 * dt;
            if (Raylib.IsKeyDown(KeyboardKey.A)) playerPos.X -= 300 * dt;
            if (Raylib.IsKeyDown(KeyboardKey.D)) playerPos.X += 300 * dt;
            
            // Catty AI: chase player
            Vector2 dir = playerPos - catPos;
            if (dir.Length() > 0)
                catPos += dir / dir.Length() * 200 * catSpeed * dt;
            
            // Collision with Catty = game over
            if (Vector2.Distance(playerPos, catPos) < 30)
            {
                Raylib.CloseWindow();
                return;
            }
            
            // Notebook collision
            for (int i = 0; i < notebooks.Count; i++)
            {
                if (Vector2.Distance(playerPos, notebooks[i]) < 25 && !showMath)
                {
                    showMath = true;
                    currentNotebook = i;
                }
            }
            
            // Drawing
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.DARKGREEN);
            
            // Draw notebooks
            foreach (var n in notebooks)
                Raylib.DrawRectangle((int)n.X - 10, (int)n.Y - 10, 20, 20, Color.GOLD);
            
            // Draw Catty with your PNG
            Raylib.DrawTexture(catTexture, (int)catPos.X - 25, (int)catPos.Y - 25, Color.WHITE);
            
            // Draw player
            Raylib.DrawCircleV(playerPos, 15, Color.BLUE);
            
            // UI
            Raylib.DrawText($"Wrong: {wrongAnswers}", 10, 10, 20, Color.WHITE);
            Raylib.DrawText($"Catty Speed: x{catSpeed:F1}", 10, 35, 20, Color.WHITE);
            
            // Math problem popup
            if (showMath)
            {
                Raylib.DrawRectangle(200, 200, 400, 150, Color.DARKGRAY);
                Raylib.DrawText("2 + 2 = ?", 220, 230, 30, Color.WHITE);
                Raylib.DrawText("Press Y for 4, N for wrong", 220, 280, 20, Color.LIGHTGRAY);
                
                if (Raylib.IsKeyPressed(KeyboardKey.Y))
                {
                    showMath = false;
                    notebooks.RemoveAt(currentNotebook);
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.N))
                {
                    wrongAnswers++;
                    catSpeed = 1f + wrongAnswers * 0.3f;
                    showMath = false;
                    notebooks.RemoveAt(currentNotebook);
                }
            }
            
            Raylib.EndDrawing();
        }
        
        Raylib.UnloadTexture(catTexture);
        Raylib.CloseWindow();
    }
}
