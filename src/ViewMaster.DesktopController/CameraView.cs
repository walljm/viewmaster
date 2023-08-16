using Microsoft.Web.WebView2.Core;
using System.Reflection;

namespace ViewMaster.DesktopController;

public partial class CameraView : Form
{
    private const string UrlBase = "http://cameraview/";

    public CameraView()
    {
        InitializeComponent();

        webView.CoreWebView2InitializationCompleted += this.WebView_CoreWebView2InitializationCompleted;
        webView.EnsureCoreWebView2Async();
        webView.Source = new Uri($"{UrlBase}camera-view.html");
    }

    private void WebView_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
    {
        webView.CoreWebView2.AddWebResourceRequestedFilter($"{UrlBase}*", CoreWebView2WebResourceContext.All);
        webView.CoreWebView2.WebResourceRequested += (object? sender, CoreWebView2WebResourceRequestedEventArgs args) =>
        {
            try
            {
                string assetsFilePath = args.Request.Uri.Substring($"{UrlBase}*".Length - 1);
                var fs = ReadResource(assetsFilePath);
                string headers = assetsFilePath switch
                {
                    _ when assetsFilePath.EndsWith(".html") => "Content-Type: text/html",
                    _ when assetsFilePath.EndsWith(".jpg") => "Content-Type: image/jpeg",
                    _ when assetsFilePath.EndsWith(".png") => "Content-Type: image/png",
                    _ when assetsFilePath.EndsWith(".css") => "Content-Type: text/css",
                    _ when assetsFilePath.EndsWith(".js") => "Content-Type: application/javascript",
                    _ => throw new NotImplementedException()
                };

                args.Response = webView.CoreWebView2.Environment.CreateWebResourceResponse(fs, 200, "OK", headers);
            }
            catch (Exception)
            {
                args.Response = webView.CoreWebView2.Environment.CreateWebResourceResponse(null, 404, "Not found", "");
            }
        };
    }

    static ManagedStream ReadResource(string name)
    {
        // Determine path
        var assembly = Assembly.GetExecutingAssembly() ?? throw new InvalidOperationException();
        var resourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name)) ?? throw new ArgumentNullException(nameof(name), "Could not find file in manifest");
        var stream = assembly.GetManifestResourceStream(resourcePath);
        return stream is null ? throw new ArgumentNullException(nameof(name), "Could not find file in manifest") : new ManagedStream(stream);
    }
}

internal class ManagedStream : Stream
{
    public ManagedStream(Stream s)
    {
        s_ = s;
    }

    public override bool CanRead => s_.CanRead;

    public override bool CanSeek => s_.CanSeek;

    public override bool CanWrite => s_.CanWrite;

    public override long Length => s_.Length;

    public override long Position { get => s_.Position; set => s_.Position = value; }

    public override void Flush()
    {
        throw new NotImplementedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return s_.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        int read;
        try
        {
            read = s_.Read(buffer, offset, count);
            if (read == 0)
            {
                s_.Dispose();
            }
        }
        catch
        {
            s_.Dispose();
            throw;
        }
        return read;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }

    private Stream s_;
}
