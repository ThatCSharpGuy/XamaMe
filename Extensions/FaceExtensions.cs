using System.Drawing;

namespace XamaMe.Extensions
{
    public static class FaceExtensions
    {
        public static float[] GetCenter(this Rectangle face)
        {
            var midX = face.Left + (face.Width / 2);
            var midY = face.Top + (face.Height / 2);

            return new float[] { midY, midX };
        }

        public static Rectangle GetRectangle(this Microsoft.ProjectOxford.Face.Contract.Face face)
        {
            return new Rectangle(face.FaceRectangle.Left,
                face.FaceRectangle.Top,
                face.FaceRectangle.Width,
                face.FaceRectangle.Height);
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