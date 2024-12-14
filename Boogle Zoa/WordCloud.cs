using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace Boogle_Zoa
{
    public class WordCloud
    {
        // Dimensions de l'image
        private static readonly int imageWidth = 960;
        private static readonly int imageHeight = 600;

        private static int xGraphOrigin = -130;
        private static int yGraphOrigin = 50;

        // Couleurs et styles
        private static Font titleFont = new Font(FontFamily.GenericSerif, 34f, FontStyle.Bold);
        private static readonly Brush titlebrush = new SolidBrush(Color.SaddleBrown);
        private static readonly Brush backgroundBrush = new SolidBrush(Color.LightYellow);
        private static readonly Brush wordBrush = new SolidBrush(Color.OrangeRed);


        private static readonly Color CurveColor = Color.Red;


        // Bitmap et contexte graphique
        private Bitmap bitmap;
        private Graphics graphics;

        private string[] words;

        

        public WordCloud(string[] words)
        {
            bitmap = new Bitmap(imageWidth, imageHeight);
            graphics = Graphics.FromImage(bitmap);
            this.words = words;

            graphics.FillRectangle(backgroundBrush, new Rectangle(0, 0, imageWidth, imageHeight));

            graphics.DrawString("BOOGLE ZOA", titleFont, titlebrush, 340, 20);

            DrawParametricCurve(1, 4.15 * Math.PI, LogarithmicSpiral);
        }

        private void DrawParametricCurve(double tMin, double tMax, Func<double, (double x, double y)> parametricFunction)
        {
            int steps = words.Length; // Nombre de points à tracer
            Font font;

            for (int i = 0; i < steps; i++)
            {
                font = new Font(FontFamily.GenericSerif, 5 + 1.2f * i, FontStyle.Bold); // A VOIR

                double t = Map(i, 0, steps, tMin, tMax);
                var (x, y) = parametricFunction(t);

                // Convertit les coordonnées (x, y) en pixels
                int px = xGraphOrigin + (int)Map(x, -1.5, 1.5, 0, imageWidth);
                int py = yGraphOrigin + (int)Map(y, -1.0, 1.0, 0, imageHeight);

                if (px >= 0 && px < imageWidth && py >= 0 && py < imageHeight)
                {
                    //Bitmap.SetPixel(px, py, CurveColor);
                    graphics.DrawString(words[steps - i - 1].ToString(), font, wordBrush, px, py);
                }
            }
        }


        public void SaveAndOpenImage(string fileName)
        {
            string filePath = $"..\\..\\..\\wordClouds\\{fileName}";
            bitmap.Save(filePath, ImageFormat.Png);

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ouverture du fichier : {ex.Message}");
            }
        }

        private static (double x, double y) LogarithmicSpiral(double theta)
        {
            double a = 0.12; // Échelle initiale
            double b = 0.17; // Facteur de croissance

            // Calcul de r (distance à l'origine)
            double r = a * Math.Exp(b * theta);

            // Conversion en coordonnées cartésiennes
            double x = r * Math.Cos(theta);
            double y = r * Math.Sin(theta);

            return (x, y);
        }

        private static double Map(double value, double fromMin, double fromMax, double toMin, double toMax)
        {
            return toMin + ((value - fromMin) * (toMax - toMin)) / (fromMax - fromMin);
        }
    }
}
