using System;
using SharpDX.Windows;

namespace HelloTriangle
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var form = new RenderForm("DirectX 12 : CSharp")
            {
                Width = 640,
                Height = 480,
                WindowState = System.Windows.Forms.FormWindowState.Maximized
            };
            form.Show();

            using (var app = new HelloTriangle())
            {
                app.Initialize(form);

                using (var loop = new RenderLoop(form))
                {
                    while (loop.NextFrame())
                    {
                        app.Update();
                        app.Render();
                    }
                }
            }
        }
    }
}
