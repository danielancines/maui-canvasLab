using Vision;

namespace Maui.CanvasLab;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnLineButtonClicked(object? sender, EventArgs e)
    {
        this.MyChart.MyChartScale += 10;
        this.MyChart.Invalidate();
    }

    private void OnCircleButtonClicked(object? sender, EventArgs e)
    {
        this.MyChart.MyChartScale -= 10;
        this.MyChart.Invalidate();
    }

    private void OnRectangleButtonClicked(object? sender, EventArgs e)
    {
        this.MyChart.Buy = !this.MyChart.Buy;
        this.MyChart.Invalidate();
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

    public int MyChartScale { get; set; } = 50;
    public bool Buy { get; set; }

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
        if (this._endPoint == null)
            return;

        canvas.Antialias = true;
        PathF path = new PathF();
        PointF clickPoint = new PointF((float)this._endPoint.Value.X, (float)this._endPoint.Value.Y);
        Color arrowColor = Colors.Green;

        if (Buy)
        {
            path.MoveTo(clickPoint);
            path.LineTo(clickPoint.X - this.MyChartScale, clickPoint.Y + this.MyChartScale * 2);
            path.LineTo(clickPoint.X - (this.MyChartScale / 2), clickPoint.Y + this.MyChartScale * 2);
            path.LineTo(clickPoint.X - (this.MyChartScale / 2), clickPoint.Y + this.MyChartScale * 3);
            path.LineTo(clickPoint.X + (this.MyChartScale / 2), clickPoint.Y + this.MyChartScale * 3);
            path.LineTo(clickPoint.X + (this.MyChartScale / 2), clickPoint.Y + this.MyChartScale * 2);
            path.LineTo(clickPoint.X + this.MyChartScale, clickPoint.Y + this.MyChartScale * 2);
            path.Close();
        }
        else
        {
            arrowColor = Colors.Red;
            path.MoveTo(clickPoint);
            path.LineTo(clickPoint.X - this.MyChartScale, clickPoint.Y - this.MyChartScale * 2);
            path.LineTo(clickPoint.X - (this.MyChartScale / 2), clickPoint.Y - this.MyChartScale * 2);
            path.LineTo(clickPoint.X - (this.MyChartScale / 2), clickPoint.Y - this.MyChartScale * 3);
            path.LineTo(clickPoint.X + (this.MyChartScale / 2), clickPoint.Y - this.MyChartScale * 3);
            path.LineTo(clickPoint.X + (this.MyChartScale / 2), clickPoint.Y - this.MyChartScale * 2);
            path.LineTo(clickPoint.X + this.MyChartScale, clickPoint.Y - this.MyChartScale * 2);
            path.Close();
        }

        canvas.StrokeSize = 2;
        canvas.FillColor = arrowColor;

        canvas.SaveState();
        canvas.Alpha = 0.2f;
        canvas.FillPath(path);
        canvas.RestoreState();

        canvas.StrokeColor = arrowColor;
        canvas.DrawPath(path);
    }
}
