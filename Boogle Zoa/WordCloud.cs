using Boogle_Zoa;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public class WordCloud
{
    private static readonly int imageWidth = 960;
    private static readonly int imageHeight = 600;
    private static readonly int centerX = imageWidth / 2;
    private static readonly int centerY = imageHeight / 2;

    private static readonly Font TitleFont = new Font(FontFamily.GenericSerif, 50f);
    private static readonly Brush TitleBrush = new SolidBrush(Color.Chocolate);
    private static readonly Font TextFont = new Font(FontFamily.GenericSerif, 30f);
    private static readonly Brush TextBrush = new SolidBrush(Color.Beige);

    private string name;
    private string[] words;
    private int score;

    private Bitmap bitmap;
    private Graphics graphics;



    public WordCloud(Player player)
    {
        name = player.Name;
        score = player.Score;
        words = player.WordsFound.ToArray();

        Array.Sort(words, CompareBySize);

        bitmap = new Bitmap(imageWidth, imageHeight);
        graphics = Graphics.FromImage(bitmap);

        graphics.Clear(Color.Black);

        DrawTemplate();
        DrawSpiralWords();
    }

    private void DrawSpiralWords()
    {
        int numWords = words.Length;
        double angle = 0;
        double radius = 10;
        Font font;
        Brush colorBrush;
        int maxFontSize = 40;
        int minFontSize = 10;

        Color startColor = Color.Chocolate;
        Color endColor = Color.Beige;
        LinearGradientBrush gradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(imageWidth, imageHeight), startColor, endColor);

        List<RectangleF> wordPositions = new List<RectangleF>();

        for (int i = 0; i < numWords; i++)
        {
            int fontSize = (int)(minFontSize + (i * (maxFontSize - minFontSize) / (double)numWords));
            font = new Font(FontFamily.GenericSerif, fontSize, FontStyle.Bold);
            colorBrush = new SolidBrush(gradientBrush.GetColorAtPosition((float)i / numWords));

            bool placed = false;
            int attempts = 0;
            const int maxAttempts = 1000;

            // Réinitialiser les positions initiales pour chaque mot
            double currentAngle = angle;
            double currentRadius = radius;

            while (!placed && attempts < maxAttempts)
            {
                int x = (int)(centerX + currentRadius * Math.Cos(currentAngle)) - fontSize / 2;
                int y = (int)(centerY + currentRadius * Math.Sin(currentAngle)) - fontSize / 2;

                RectangleF wordRectangle = new RectangleF(x, y, font.Size * words[i].Length, fontSize);

                bool overlaps = false;
                foreach (var existingWord in wordPositions)
                {
                    if (existingWord.IntersectsWith(wordRectangle))
                    {
                        overlaps = true;
                        break;
                    }
                }

                if (!overlaps)
                {
                    DrawCenteredText(words[i], font, colorBrush, x, y);
                    wordPositions.Add(wordRectangle);
                    placed = true;
                }
                else
                {
                    // Si chevauchement, ajuster l'angle et le rayon
                    currentAngle += Math.PI / 36;  // Petite augmentation de l'angle
                    currentRadius += 2;  // Augmentation du rayon pour éloigner le mot
                }

                attempts++;
            }

            angle += Math.PI / 20;  // Augmenter l'angle global pour la spirale
            radius += 5;  // Augmenter progressivement le rayon général
        }
    }


    private void DrawTemplate()
    {
        DrawCenteredText("BOOGLE ZOA", TitleFont, TitleBrush, imageWidth / 2, 50);

        graphics.DrawString(name.ToUpper(), TextFont, TextBrush, 50, imageHeight - 75);

        DrawCenteredText(score.ToString(), TextFont, TextBrush, 850, imageHeight - 50);
    }


    private void DrawCenteredText(string text, Font font, Brush brush, int xSet, int ySet)
    {
        float width = graphics.MeasureString(text, font).Width;
        float heigth = font.GetHeight();

        float x = (xSet - width / 2);
        float y = ySet - heigth / 2;

        graphics.DrawString(text, font, brush, x, y);
    }


    public void SaveAndOpenImage()
    {
        string filePath = $"..\\..\\..\\wordClouds\\WordCloud_{name}.png";

        bitmap.Save(filePath, ImageFormat.Png);
        System.Diagnostics.Process.Start("explorer", filePath);
    }



    public static int CompareBySize(string x, string y)
    {
        return x.Length.CompareTo(y.Length);
    }
}



public static class GradientExtensions
{
    public static Color GetColorAtPosition(this LinearGradientBrush brush, float position)
    {
        Color startColor = brush.LinearColors[0];
        Color endColor = brush.LinearColors[1];

        int r = (int)(startColor.R + (endColor.R - startColor.R) * position);
        int g = (int)(startColor.G + (endColor.G - startColor.G) * position);
        int b = (int)(startColor.B + (endColor.B - startColor.B) * position);
        int a = (int)(startColor.A + (endColor.A - startColor.A) * position);

        return Color.FromArgb(a, r, g, b);
    }
}

