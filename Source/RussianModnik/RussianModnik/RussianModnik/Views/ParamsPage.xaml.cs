using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;
using SkiaSharp.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RussianModnik.Animations;
using RussianModnik.ViewModels;
using SkiaSharp.Views.Forms;

namespace RussianModnik.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParamsPage : ContentPage
    {
        //Put settings here so they are easier to change in the future
        public readonly static Color GradientStart = Color.FromHex("#A6B1E1".ToUpper());
        public readonly static Color GradientEnd = Color.FromHex("#f4c95d".ToUpper());
        public readonly static TimeSpan StrokeAnimationDuration = TimeSpan.FromMilliseconds(900);
        public readonly static Easing StrokeAnimationEasing = Easing.CubicInOut;

        private readonly HighlightForm Highlighter;

        private ParamsViewModel ViewModel;

        public ParamsPage()
        {
            InitializeComponent();

            ViewModel = new ParamsViewModel();
            this.BindingContext = ViewModel;

            Highlighter = new HighlightForm
            (
                new HighlightSettings()
                {
                    StrokeWidth = 8,
                    StrokeStartColor = GradientStart,
                    StrokeEndColor = GradientEnd,
                    AnimationDuration = StrokeAnimationDuration,
                    AnimationEasing = StrokeAnimationEasing,
                }
            );
        }

        private void DownloadClicked(object sender, EventArgs e)
        {
            IsBusy = true;
            ActivityFrame.IsVisible = true;
            MainIndicator.IsRunning = true;

            Task.Factory.StartNew(() =>
            {
                ViewModel.PopulateParams();
                Task.Delay(3000).Wait(); //Show the user something is happening

                Device.BeginInvokeOnMainThread(() =>
                {
                    ShowParams();

                    UnBusy();
                });
            });
        }

        private void CalculateClicked(object sender, EventArgs e)
        {
            IsBusy = true;
            ActivityFrame.IsVisible = true;
            MainIndicator.IsRunning = true;

            Task.Factory.StartNew(() =>
            {
                string saveRes = SaveParams();
                if (!string.IsNullOrEmpty(saveRes))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("Неверно введены параметры", saveRes, "Ок");
                        UnBusy();
                    });
                    return;
                }

                ViewModel.ComputePredictions();
                Task.Delay(2000).Wait();

                Device.BeginInvokeOnMainThread(() =>
                {
                    UnBusy();
                });
            }
            );
        }

        private void UnBusy()
        {
            IsBusy = false;
            ActivityFrame.IsVisible = MainIndicator.IsRunning = false;
        }

        private void ShowParams()
        {
            HeightEntry.Text = ViewModel.ParamValues.Height.ToString();
            WeightEntry.Text = ViewModel.ParamValues.Weight.ToString();
            FeetLength.Text = ViewModel.ParamValues.FeetLength.ToString();
            if (ViewModel.ParamValues.GenderIsMan)
                GenderPicker.SelectedItem = "Мужчина";
            else
                GenderPicker.SelectedItem = "Женщина";
            BodyTypePicker.SelectedItem = ViewModel.ParamValues.BodyType;
        }

        private string SaveParams()
        {
            //NOTE: DisplayAlert Doesn't work in tasks

            double temp;
            if (double.TryParse(HeightEntry.Text, out temp))
                ViewModel.ParamValues.Height = temp;
            else
            {
                return $"Рост не может быть: {HeightEntry.Text}. Пожалуйста, введите число"; 
            }

            if (double.TryParse(WeightEntry.Text, out temp))
                ViewModel.ParamValues.Weight = temp;
            else
            {
                //DisplayAlert("Неверно введены параметры", $"Рост не может быть: {WeightEntry.Text}. Пожалуйста, введите число", "Ок");
                return $"Рост не может быть: {WeightEntry.Text}. Пожалуйста, введите число";
            }

            if (double.TryParse(FeetLength.Text, out temp))
                ViewModel.ParamValues.FeetLength = temp;
            else
            {
                //DisplayAlert("Неверно введены параметры", $"Длина стопы не может быть: {FeetLength.Text}. Пожалуйста, введите число", "Ок");
                return $"Длина стопы не может быть: {FeetLength.Text}. Пожалуйста, введите число";
            }

            if (string.IsNullOrEmpty(GenderPicker.SelectedItem.ToString()))
            {
                //DisplayAlert("Неверное введены параметры", "Выберите пол", "Ок");
                return "Выберите пол";
            }
            else 
                ViewModel.ParamValues.GenderIsMan = GenderPicker.SelectedItem.ToString() == "Мужчина";

            if (string.IsNullOrEmpty(BodyTypePicker.SelectedItem.ToString()))
            {
                //DisplayAlert("Неверное введены параметры", "Выберите тип фигуры", "Ок");
                return "Выберите тип фигуры";
            }
            else 
                ViewModel.ParamValues.BodyType = BodyTypePicker.SelectedItem.ToString();

            ViewModel.SaveParams();
            return string.Empty;
        }

        public ParamsPage(ParamsViewModel viewModel) : this()
        {
            ViewModel = viewModel;
            this.BindingContext = ViewModel;
        }

        #region Animations

        private void HighlightElement(View sender)
        {
            Highlighter.HighlightElement(sender, MainCanvasView, MainLayout);
        }

        private void EntryFocused(object sender, FocusEventArgs e)
        {
            HighlightElement((View)sender);
        }

        private void BoxViewFocused(object sender, FocusEventArgs e)
        {
            HighlightElement((View)sender);
        }

        private void PickerFocused(object sender, FocusEventArgs e)
        {
            HighlightElement((View)sender);
        }

        //Why doesn't this work?
        private void ChooseEyeColorGrid_Focused(object sender, FocusEventArgs e)
        {
            HighlightElement(ChooseEyeColorGrid);
        }

        private void MainCanvasViewPaintSurfaceRequested(object sender, SKPaintSurfaceEventArgs e)
        {
            Highlighter.Draw(MainCanvasView, e.Surface.Canvas);
        }

        #endregion

        private void FinishButton_Clicked(object sender, EventArgs e)
        {
            //Do some math stuff to calculate recommendations

            //HighlightElement((View)sender);
        }

        void UpdateBodyTypeGrids()
        {
            //MenBodyTypeGrid.IsVisible = ViewModel.GenderIsMan && BodyTypeSwitch.IsToggled;
            WomenBodyTypeGrid.IsVisible = ViewModel.GenderIsWoman && BodyTypeSwitch.IsToggled;
        }

        private void GenderPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            string gender = (sender as Picker).SelectedItem as string;
            if (gender == "Man")
                ViewModel.GenderIsMan = true;
            else
                ViewModel.GenderIsMan = false;

            UpdateBodyTypeGrids();
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            UpdateBodyTypeGrids();
        }
    }
}