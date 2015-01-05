using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Regex
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		private System.Text.RegularExpressions.Regex regex;
		private static RegexOptions regexMode = RegexOptions.Singleline;
		private bool canUse = true;

		public MainWindow()
		{
			InitializeComponent();
			regex = new System.Text.RegularExpressions.Regex(regexTextBox.Text, regexMode);
			MatchStart();
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}

		private void closeButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void miniSizeButton_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		private void textRichTextBox_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				MessageBox.Show("嗨~再见~");
				Close();
			}
			if (canUse)
				MatchStart();
		}

		private void MatchStart()
		{
			this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
				(ThreadStart)delegate ()
				{
					MatchStrings();
				});
		}

		private void MatchStrings()
		{
			//获取richTextBox文本内容
			TextRange text = new TextRange(textRichTextBox.Document.ContentStart, textRichTextBox.Document.ContentEnd);
			text.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.White);
			string content;
			if (text.Text.Length >= 3)
				content = text.Text.Remove(text.Text.Length - 2, 1);
			else
				content = text.Text;

			//遍历每个匹配子字符串
			foreach (Match m in System.Text.RegularExpressions.Regex.Matches(content, regex.ToString()))
			{
				if (m.Success)
				{
					//获取子字符串起始点
					TextPointer mStart = GetPointerFromCharOffset(m.Index, textRichTextBox.Document.ContentStart, textRichTextBox.Document);
					//获取子字符串结束
					TextPointer mEnd = GetPointerFromCharOffset(m.Length - 1, mStart, textRichTextBox.Document);
					//获取子字符串位置
					TextRange mWord = new TextRange(mStart.GetInsertionPosition(LogicalDirection.Forward), mEnd);
					//应用样式
					mWord.ApplyPropertyValue(TextElement.ForegroundProperty, "#569CD6");
				}
			}
		}

		//根据匹配字符串的索引获取TextPointer
		public TextPointer GetPointerFromCharOffset(int charOffset, TextPointer startPointer, FlowDocument document)
		{
			TextPointer nextPointer = startPointer;
			if (charOffset == 0)
			{
				return nextPointer;
			}
			TextPointer targePointer = nextPointer;

			for (int i = 0; i <= charOffset && nextPointer != null; ++i)
			{
				if (nextPointer == document.ContentEnd)
					return nextPointer;

				targePointer = nextPointer;
				nextPointer = nextPointer.GetNextInsertionPosition(LogicalDirection.Forward);
			}

			if (nextPointer != null)
				return nextPointer;
			else
				return targePointer;
		}

		private void regexButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				regex = new System.Text.RegularExpressions.Regex(regexTextBox.Text, regexMode);
			}
			catch (ArgumentException)
			{
				MessageBox.Show("Your regex has some problems.");
				canUse = false;
				return;
			}
			canUse = true;
			MatchStart();
		}

		private void aboutButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("暂时只支持单行匹配，所以不要按回车键\n当然你按了我也阻止不了你不信你试试\nAutohr: LzxHahaha\nEmail: lzxglhf@live.com", "说明", MessageBoxButton.OK);
		}

		private void keywordsButton_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://zh.wikipedia.org/wiki/%E6%AD%A3%E5%88%99%E8%A1%A8%E8%BE%BE%E5%BC%8F");
		}
	}
}
