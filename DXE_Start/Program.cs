using SharpDX;
using SharpDX.Animation;
using SharpDX.D3DCompiler;
using SharpDX.Diagnostics;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DirectComposition;
using SharpDX.DirectInput;
using SharpDX.DirectManipulation;
using SharpDX.DirectSound;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.IO;
using SharpDX.Mathematics;
using SharpDX.Multimedia;
using SharpDX.RawInput;
using SharpDX.Text;
using SharpDX.WIC;
using SharpDX.Win32;
using SharpDX.Windows;
using SharpDX.X3DAudio;
using SharpDX.XAPO;
using SharpDX.XAudio2;
using System.Drawing;
using System.Runtime.InteropServices;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DXE_Start
{
    public class VertexPositionColor
    {
        private Vector3 vector3;
        private Color red;

        public VertexPositionColor(Vector3 vector3, Color red)
        {
            this.vector3 = vector3;
            this.red = red;
        }
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Это быстро написанный код. Тут все переменные для DX11.
            RenderForm renderForm = new RenderForm();
            SharpDX.Direct3D11.Device device;
            SwapChain swapChain;
            SwapChainDescription swapChainDescription;
            SharpDX.Viewport viewport;
            DeviceContext deviceContext;
            ModeDescription modeDescription;
            Texture2D texture2D;
            RenderTargetView renderTargetView;



            //Устанавливаем параметры в переменные.
            renderForm.Width = 640;
            renderForm.Height = 480;
            modeDescription = new ModeDescription(640, 480, new Rational(60, 1), Format.R8G8B8A8_UNorm);
            swapChainDescription = new SwapChainDescription()
            {
                BufferCount = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
                ModeDescription = modeDescription,
                IsWindowed = true,
                OutputHandle = renderForm.Handle
            };
            SharpDX.Direct3D11.Device.CreateWithSwapChain(
                DriverType.Hardware,
                SharpDX.Direct3D11.DeviceCreationFlags.None,
                swapChainDescription,
                out device,
                out swapChain);
            viewport = new SharpDX.Viewport(0, 0, 640, 480);
            deviceContext = device.ImmediateContext;
            deviceContext.Rasterizer.SetViewport(viewport);
            texture2D = swapChain.GetBackBuffer<Texture2D>(0);
            renderTargetView = new RenderTargetView(device, texture2D);



            //Шейдеры.
            ShaderSignature inputSignature;
            VertexShader vertexShader;
            PixelShader pixelShader;
            InputLayout inputLayout;
            Vector3[] vertices;
            SharpDX.Direct3D11.Buffer buffer;
            ShaderBytecode vertexShaderByteCode;
            ShaderBytecode pixelShaderByteCode;
            SharpDX.Direct3D11.InputElement[] inputElements;


            inputElements  = new SharpDX.Direct3D11.InputElement[] 
            {
                new SharpDX.Direct3D11.InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, SharpDX.Direct3D11.InputClassification.PerVertexData, 0),
                new SharpDX.Direct3D11.InputElement("COLOR", 0, Format.R32G32B32A32_Float, 12, 0, SharpDX.Direct3D11.InputClassification.PerVertexData, 0)
            };
            vertices = new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(0, 240, 0),
                new Vector3(320, 120, 0)
            };
            buffer = SharpDX.Direct3D11.Buffer.Create(device, SharpDX.Direct3D11.BindFlags.VertexBuffer, vertices);

            VertexPositionColor[] vrt = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0.0f), SharpDX.Color.Red),
                new VertexPositionColor(new Vector3(0.5f, 0.5f, 0.0f), SharpDX.Color.Green),
                new VertexPositionColor(new Vector3(0.0f, -0.5f, 0.0f), SharpDX.Color.Blue)
            };

            vertexShaderByteCode = ShaderBytecode.CompileFromFile("vertexShader.hlsl", "main", "vs_4_0", ShaderFlags.Debug);
            inputSignature = ShaderSignature.GetInputSignature(vertexShaderByteCode);
            vertexShader = new SharpDX.Direct3D11.VertexShader(device, vertexShaderByteCode);



            pixelShaderByteCode = ShaderBytecode.CompileFromFile("pixelShader.hlsl", "main", "ps_4_0", ShaderFlags.Debug);
            pixelShader = new SharpDX.Direct3D11.PixelShader(device, pixelShaderByteCode);



            deviceContext.VertexShader.Set(vertexShader);
            deviceContext.PixelShader.Set(pixelShader);
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            inputLayout = new SharpDX.Direct3D11.InputLayout(device, inputSignature, inputElements);
            deviceContext.InputAssembler.InputLayout = inputLayout;



            //Используем.
            deviceContext.OutputMerger.SetRenderTargets(renderTargetView);
            deviceContext.ClearRenderTargetView(renderTargetView, new SharpDX.Color(32, 103, 178));
            deviceContext.Draw(0, 0);
            swapChain.Present(1, PresentFlags.None);
            renderForm.Show();



            //Очистка всего.
            renderTargetView.Dispose();
            texture2D.Dispose();
            deviceContext.Dispose();
            swapChain.Dispose();
            device.Dispose();
            renderForm.Dispose();
        }
    }
}
