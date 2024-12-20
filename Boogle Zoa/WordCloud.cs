using Boogle_Zoa;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

/// <summary>
/// Classe <see cref="WordCloud"/> pour générer des nuages de mots visuels représentant les données d'un joueur.
/// </summary>
/// <remarks>
/// Cette classe crée une image bitmap où les mots trouvés par un joueur sont disposés selon une spirale.
/// </remarks>
public class WordCloud
{
    /// <summary>
    /// Largeur de l'image générée pour le nuage de mots, en pixels.
    /// </summary>
    private static readonly int imageWidth = 960;

    /// <summary>
    /// Hauteur de l'image générée pour le nuage de mots, en pixels.
    /// </summary>
    private static readonly int imageHeight = 600;

    /// <summary>
    /// Coordonnée X du centre de l'image.
    /// </summary>
    private static readonly int centerX = imageWidth / 2;

    /// <summary>
    /// Coordonnée Y du centre de l'image.
    /// </summary>
    private static readonly int centerY = imageHeight / 2;

    /// <summary>
    /// Police utilisée pour le titre du nuage de mots.
    /// </summary>
    private static readonly Font TitleFont = new Font(FontFamily.GenericSerif, 50f);

    /// <summary>
    /// Pinceau pour colorier le titre du nuage de mots.
    /// </summary>
    private static readonly Brush TitleBrush = new SolidBrush(Color.Chocolate);

    /// <summary>
    /// Police utilisée pour les textes secondaires (nom et score du joueur).
    /// </summary>
    private static readonly Font TextFont = new Font(FontFamily.GenericSerif, 30f);

    /// <summary>
    /// Pinceau pour colorier les textes secondaires (nom et score du joueur).
    /// </summary>
    private static readonly Brush TextBrush = new SolidBrush(Color.Beige);


    /// <summary>
    /// Nom du joueur pour lequel le nuage de mots est généré.
    /// </summary>
    private string name;

    /// <summary>
    /// Liste des mots trouvés par le joueur, triés par longueur.
    /// </summary>
    private string[] words;

    /// <summary>
    /// Score total du joueur.
    /// </summary>
    private int score;

    /// <summary>
    /// Bitmap représentant l'image générée pour le nuage de mots.
    /// </summary>
    private Bitmap bitmap;

    /// <summary>
    /// Objet graphique utilisé pour dessiner sur le bitmap.
    /// </summary>
    private Graphics graphics;



    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="WordCloud"/> avec les données d'un joueur.
    /// </summary>
    /// <param name="player">Joueur</param>
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



    /// <summary>
    /// Dispose les mots trouvés par le joueur en spirale sur l'image bitmap.
    /// </summary>
    /// <remarks>
    /// Chaque mot est dessiné avec une taille et une couleur personnalisées.
    /// La méthode utilise un algorithme pour éviter que les mots se chevauchent, en ajustant leur position sur une spirale centrée.
    /// </remarks>
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
                    currentAngle += Math.PI / 36;
                    currentRadius += 2;
                }
                attempts++;
            }
            angle += Math.PI / 20;
            radius += 5;
        }
    }


    /// <summary>
    /// Sauvegarde l'image générée sous forme de fichier PNG et l'ouvre automatiquement dans l'explorateur de fichiers.
    /// </summary>
    /// <remarks>
    /// Le fichier est enregistré dans le dossier "wordClouds" avec un nom basé sur le joueur.
    /// </remarks>
    private void DrawTemplate()
    {
        DrawCenteredText("BOOGLE ZOA", TitleFont, TitleBrush, imageWidth / 2, 50);

        graphics.DrawString(name.ToUpper(), TextFont, TextBrush, 50, imageHeight - 75);

        DrawCenteredText(score.ToString(), TextFont, TextBrush, 850, imageHeight - 50);
    }


    /// <summary>
    /// Dessine un texte centré aux coordonnées spécifiées.
    /// </summary>
    /// <param name="text">Texte à dessiner.</param>
    /// <param name="font">Police utilisée pour le texte.</param>
    /// <param name="brush">Pinceau pour la couleur du texte.</param>
    /// <param name="xSet">Coordonnée X où centrer le texte.</param>
    /// <param name="ySet">Coordonnée Y où centrer le texte.</param>
    private void DrawCenteredText(string text, Font font, Brush brush, int xSet, int ySet)
    {
        float width = graphics.MeasureString(text, font).Width;
        float heigth = font.GetHeight();

        float x = (xSet - width / 2);
        float y = ySet - heigth / 2;

        graphics.DrawString(text, font, brush, x, y);
    }


    /// <summary>
    /// Sauvegarde l'image générée sous forme de fichier PNG et l'ouvre automatiquement dans l'explorateur de fichiers.
    /// </summary>
    /// <remarks>
    /// Le fichier est enregistré dans le dossier "wordClouds" avec un nom basé sur le joueur.
    /// </remarks>
    public void SaveAndOpenImage()
    {
        string filePath = $"..\\..\\..\\wordClouds\\WordCloud_{name}.png";

        bitmap.Save(filePath, ImageFormat.Png);
        System.Diagnostics.Process.Start("explorer", filePath);
    }


    /// <summary>
    /// Compare deux chaînes de caractères en fonction de leur longueur.
    /// </summary>
    /// <param name="x">Première chaîne à comparer.</param>
    /// <param name="y">Deuxième chaîne à comparer.</param>
    /// <returns>
    /// Un entier négatif, zéro ou positif, indiquant si <paramref name="x"/> est plus court,
    /// de même longueur ou plus long que <paramref name="y"/>.
    /// </returns>
    public static int CompareBySize(string x, string y)
    {
        return x.Length.CompareTo(y.Length);
    }
}


/// <summary>
/// Classe d'extensions pour les pinceaux dégradés.
/// </summary>
public static class GradientExtensions
{
    /// <summary>
    /// Calcule une couleur intermédiaire dans un dégradé linéaire en fonction d'une position donnée.
    /// </summary>
    /// <param name="brush">Pinceau dégradé linéaire.</param>
    /// <param name="position">Position dans le dégradé (entre 0 et 1).</param>
    /// <returns>Couleur correspondant à la position spécifiée.</returns>
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

