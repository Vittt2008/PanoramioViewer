﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PanoramioViewer.App.Control;
using PanoramioViewer.App.ViewModels;
using PanoramioViewer.Logic.ServiceImpl;

// Документацию по шаблону элемента "Пустая страница" см. по адресу http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PanoramioViewer.App
{
	/// <summary>
	/// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			InitializeComponent();
			DataContext = new MainViewModel(new PanoramioService());
		}

		public MainViewModel ViewModel => DataContext as MainViewModel;

		private void PreviewPopupOnLayoutUpdated(object sender, object e)
		{
			double actualHorizontalOffset = PreviewPopup.HorizontalOffset;
			double actualVerticalOffset = PreviewPopup.VerticalOffset;

			double newHorizontalOffset = (Window.Current.Bounds.Width - PreviewBorder.ActualWidth) / 2;
			double newVerticalOffset = (Window.Current.Bounds.Height - PreviewBorder.ActualHeight) / 2;

			if (Math.Abs(actualHorizontalOffset - newHorizontalOffset) > 10E-6 ||
				Math.Abs(actualVerticalOffset - newVerticalOffset) > 10E-6)
			{
				PreviewPopup.HorizontalOffset = newHorizontalOffset;
				PreviewPopup.VerticalOffset = newVerticalOffset;
			}
		}
	}
}
