namespace Maui.CanvasLab;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }
}


public class MyChart : GraphicsView, IDrawable
{
    public MyChart()
    {
        this.Drawable = this;

        var pointerGestureRecognizer = new PointerGestureRecognizer();
        pointerGestureRecognizer.PointerPressed += PointerGestureRecognizer_PointerPressed;
        pointerGestureRecognizer.PointerReleased += PointerGestureRecognizer_PointerReleased;
        pointerGestureRecognizer.PointerMoved += PointerGestureRecognizer_PointerMoved;
        this.GestureRecognizers.Add(pointerGestureRecognizer);
    }

    private void PointerGestureRecognizer_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_mousePressed)
            return;

        _endPoint = e.GetPosition(this);
        this.Invalidate();
    }

    private void PointerGestureRecognizer_PointerReleased(object? sender, PointerEventArgs e)
    {
        _mousePressed = false;
        _endPoint = e.GetPosition(this);

        this.Invalidate();
    }

    bool _mousePressed;
    Point? _startPoint;
    Point? _endPoint;
    private void PointerGestureRecognizer_PointerPressed(object? sender, PointerEventArgs e)
    {
        _mousePressed = true;
        _startPoint = e.GetPosition(this);
        this.Invalidate();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Colors.Black;
        canvas.FillRectangle(dirtyRect);

        canvas.StrokeColor = Colors.Green;
        canvas.StrokeSize = 2;

        if (this._startPoint == null)
            return;

        float startX = (float)this._startPoint?.X;
        float startY = (float)this._startPoint?.Y;

        if (_startPoint != null)
        {
            canvas.DrawCircle(this._startPoint.Value, 1d);
        }


        if (this._endPoint == null)
            return;

        float endX = (float)this._endPoint.Value.X;
        float endY = (float)this._endPoint.Value.Y;
        if (this._mousePressed)
        {
            float targetY = startY;
            canvas.DrawString("0%", startX + 4, targetY + 12, HorizontalAlignment.Left);
            canvas.DrawLine((float)this._startPoint.Value.X, (float)this._startPoint.Value.Y, (float)this._startPoint.Value.X + 300, (float)this._startPoint.Value.Y);

            targetY = startY + ((endY - startY) * 23.6f / 100);
            canvas.FontColor = Colors.Green;
            canvas.DrawString("23.6%", startX + 4, targetY + 12, HorizontalAlignment.Left);
            canvas.DrawLine((float)this._startPoint.Value.X, targetY, (float)this._startPoint.Value.X + 300, targetY);

            targetY = startY + ((endY - startY) * 38.5f / 100);
            canvas.DrawString("38.5%", startX + 4, targetY + 12, HorizontalAlignment.Left);
            canvas.DrawLine((float)this._startPoint.Value.X, targetY, (float)this._startPoint.Value.X + 300, targetY);

            targetY = startY + ((endY - startY) * 50 / 100);
            canvas.DrawString("50%", startX + 4, targetY + 12, HorizontalAlignment.Left);
            canvas.DrawLine((float)this._startPoint.Value.X, targetY, (float)this._startPoint.Value.X + 300, targetY);

            targetY = startY + ((endY - startY) * 61.8f / 100);
            canvas.DrawString("61.8%", startX + 4, targetY + 12, HorizontalAlignment.Left);
            canvas.DrawLine((float)this._startPoint.Value.X, targetY, (float)this._startPoint.Value.X + 300, targetY);

            targetY = endY;
            canvas.DrawString("100%", startX + 4, targetY + 12, HorizontalAlignment.Left);
            canvas.DrawLine((float)this._startPoint.Value.X, (float)this._endPoint.Value.Y, (float)this._startPoint.Value.X + 300, (float)this._endPoint.Value.Y);

            //canvas.DrawLine((float)this._startPoint.Value.X, (float)this._startPoint.Value.Y + 50, (float)this._endPoint.Value.X + 100, (float)this._endPoint.Value.Y);
        }

        if (_endPoint != null && _startPoint != null)
        {
            canvas.DrawCircle(this._endPoint.Value, 1d);
            //canvas.StrokeDashPattern = [2f];
            canvas.DrawLine((float)this._startPoint.Value.X, (float)this._startPoint.Value.Y, (float)this._endPoint.Value.X, (float)this._endPoint.Value.Y);
        }
    }
}
