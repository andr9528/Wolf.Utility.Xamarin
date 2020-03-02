using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Wolf.Utility.Main.Logging;
using Wolf.Utility.Main.Logging.Enum;
using Wolf.Utility.Xamarin.TriggerActions;
using Wolf.Utility.Xamarin.TriggerActions.Resources;
using Xamarin.Forms;

namespace Wolf.Utility.Xamarin.Elements
{
    [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
    public class PasswordEntry : StackLayout
    {
        private Entry PasswordHidden;
        private Entry PasswordShown;
        private Image PasswordButtonIcon;
        private Button PasswordButton;

        private Grid InnerGrid;
        private Grid OuterGrid;

        public string Text => PasswordShown.Text;

        private string ShownText => PasswordShown.Text;
        private string HiddenText => PasswordHidden.Text;
        
        public PasswordEntry()
        {
            CreateViewElements();

            BindingContext = this;

            var pathShown = nameof(ShownText);
            var pathHidden = nameof(HiddenText);

            Logging.Log(LogType.Information,
                $"{nameof(pathShown)} => {pathShown}; {nameof(pathHidden)} => {pathHidden}");

            if (pathShown == null || pathHidden == null)
                throw new ArgumentNullException($"{nameof(pathHidden)} or {nameof(pathShown)}",
                    $@"Getting the nameof either {nameof(ShownText)} or {nameof(HiddenText)} returned null");

            PasswordHidden.SetBinding(Entry.TextProperty, pathShown, BindingMode.TwoWay);
            PasswordShown.SetBinding(Entry.TextProperty, pathHidden, BindingMode.TwoWay);
            
            InnerGrid.Children.Add(PasswordButton, 0, 0);
            InnerGrid.Children.Add(PasswordButtonIcon, 0, 0);
            
            OuterGrid.Children.Add(PasswordHidden, 0, 0);
            OuterGrid.Children.Add(PasswordShown, 0, 0);
            OuterGrid.Children.Add(InnerGrid, 0, 0);
        }

        private void CreateViewElements()
        {
            PasswordHidden = new Entry()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                IsPassword = true,
            };

            PasswordShown = new Entry()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                IsPassword = false,
                IsVisible = false
            };

            PasswordButtonIcon = new Image()
            {
                HeightRequest = 25,
                HorizontalOptions = LayoutOptions.Fill,
                InputTransparent = true,
                Source = ImageSource.FromStream(() => new MemoryStream(Icons.showpasswordicon)),
                VerticalOptions = LayoutOptions.Fill,
                WidthRequest = 25
            };

            PasswordButton = new Button()
            {
                BackgroundColor = Color.Transparent,
                Margin = new Thickness(-4, -6, -4, -6),
                Triggers =
                {
                    new EventTrigger()
                    {
                        Event = "Clicked",
                        Actions =
                        {
                            new ShowPasswordTriggerAction()
                            {
                                EntryPasswordHidden = nameof(PasswordHidden),
                                EntryPasswordShown = nameof(PasswordShown), IconImageName = nameof(PasswordButtonIcon)
                            }
                        }
                    }
                }
            };

            InnerGrid = new Grid()
            {
                Padding = new Thickness(0, 0, 3, 0),
                WidthRequest = 35,
                HeightRequest = 27,
                IsClippedToBounds = true,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                RowDefinitions = new RowDefinitionCollection() { new RowDefinition() },
                ColumnDefinitions = new ColumnDefinitionCollection() { new ColumnDefinition() }
            };

            OuterGrid = new Grid()
            {
                RowDefinitions = new RowDefinitionCollection() { new RowDefinition() },
                ColumnDefinitions = new ColumnDefinitionCollection() { new ColumnDefinition() }
            };
        }

        public void SetPlaceholders(string placeholder)
        {
            PasswordHidden.Placeholder = placeholder;
            PasswordShown.Placeholder = placeholder;
        }
    }
}
