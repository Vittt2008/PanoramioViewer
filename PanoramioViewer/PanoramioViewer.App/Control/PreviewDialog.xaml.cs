using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PanoramioViewer.App.ViewModels;
using PanoramioViewer.Logic.Service;

// Документацию по шаблону элемента диалогового окна содержимого см. в разделе http://go.microsoft.com/fwlink/?LinkId=234238

namespace PanoramioViewer.App.Control
{
	public sealed partial class PreviewDialog : ContentDialog
	{
		public PreviewDialog(PhotoViewModel photoViewModel, IPanoramioService panoramioService)
		{
			InitializeComponent();
			ViewModel = new PreviewPhotoViewModel(photoViewModel, panoramioService);
		}

		public PreviewPhotoViewModel ViewModel
		{
			get { return DataContext as PreviewPhotoViewModel; }
			set { DataContext = value; }
		}
	}
}
