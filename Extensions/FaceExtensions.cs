using System.Drawing;

namespace XamaMe.Extensions
{
    public static class FaceExtensions
    {
        public static float[] GetFaceCenter(this Microsoft.ProjectOxford.Face.Contract.Face face)
        {

            var midX = face.FaceRectangle.Left + (face.FaceRectangle.Width / 2);
            var midY = face.FaceRectangle.Top + (face.FaceRectangle.Height / 2);

            return new float[] { midX, midY };
        }
        public static Rectangle GetCroppingRectangle(this Microsoft.ProjectOxford.Face.Contract.Face face)
        {
            // Xamagon size:
            const int width = 300;
            const int height = 267;

            var midX = face.FaceRectangle.Left + (face.FaceRectangle.Width / 2);
            var midY = face.FaceRectangle.Top + (face.FaceRectangle.Height / 2);

            var x = midX - (width / 2) < 0 ? 0 : midX - (width / 2);
            var y = midY - (height / 2) < 0 ? 0 : midY - (height / 2);

            return new Rectangle(x, y, width, height);
        }
    }
}