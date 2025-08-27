using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;

namespace MyIPHoneNonChang.ViewModels.Dialog
{
    public class DataViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Command ReturnCommand { get; set; }

        #region 图表相关
        public ISeries[] SeriesList { get; set; } = {
            new LineSeries<double>{
                Name="温度",
                Values=new double[]{ 63 , 88 , 17, 89 , 71,  18  },
                Fill=new SolidColorPaint(new SKColor(203,240,226,60)),
                Stroke=new SolidColorPaint(new SKColor(11,198,120),2),
                LineSmoothness=3,
                GeometryFill=null,
                GeometryStroke=null
            }
        };
        public Axis[] XAxes { get; set; } = {
            new Axis{
                MinStep=1,
                ForceStepToMin=true,
                TextSize=8,
                SeparatorsPaint=new SolidColorPaint(SKColors.Transparent)
            }
        };
        public Axis[] YAxes { get; set; } = {
            new Axis{
                MinLimit=0,
                MaxLimit=100,
                MinStep=25,
                ForceStepToMin=true,
                TextSize=8,
                SeparatorsPaint=new SolidColorPaint(new SKColor(230,230,230),1){
                    PathEffect=new DashEffect(new float[]{5,5 })
                }
            }
        };

        public RectangularSection[] Sections { get; set; } = {
            new RectangularSection{
                Yi=80,Yj=80,
                Stroke=new SolidColorPaint(SKColors.Red.WithAlpha(80),1)
            },
            new RectangularSection{
                Yi=10,Yj=10,
                Stroke=new SolidColorPaint(SKColors.Blue.WithAlpha(80),1)
            }
        };
        #endregion


        public DataViewModel(IPopupService popupService)
        {
            ReturnCommand = new Command(() =>
            {
                popupService.ClosePopup();
            });
        }
    }
}
