using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using DevilDesireDevLib.Implementation.Controls;

namespace DevilDesireDevLib.Implementation.Extensions.PropertyExtensions
{
    public class RichTextBlockBbCode
    {
        public static string GetBbCode(RichTextBlock rtb)
        {
            return (string)rtb.GetValue(BbCodeProperty);
        }

        public static void SetBbCode(RichTextBlock rtb, string bbcode)
        {
            rtb.SetValue(BbCodeProperty, bbcode);
        }

        public static readonly DependencyProperty BbCodeProperty = DependencyProperty.RegisterAttached("BbCode", typeof(string), typeof(RichTextBlockBbCode), new PropertyMetadata(string.Empty, OnBbCodeChanged));

        private static void OnBbCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RichTextBlock rtb = d as RichTextBlock;
            string bcc = e.NewValue as string;
            bcc = Replacements(bcc);

            if (rtb != null)
            {
                rtb.Blocks.Clear();

                foreach (string item in bcc.Split(new[] { "[p]", "[/p]" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Paragraph p = new Paragraph();

                    List<string> formats = new List<string>();
                    foreach (string s in SplitInBbCodeTags(item.ToCharArray()))
                    {
                        if (s.Length > 0 && s[0] == '[')
                        {
                            if (s.Length > 2 && s[1] == '/')
                            {
                                string n = formats.FirstOrDefault(x => x == "[" + s.Substring(2) || x.StartsWith("[" + s.Substring(2, s.Length - 3) + "="));
                                if (n != null)
                                    formats.Remove(n);
                            }
                            else
                            {
                                formats.Add(s);
                            }
                        }
                        else
                        {
                            if (formats.Contains("[spoiler]"))
                            {
                                InlineUIContainer inlineUiContainer = CreateInlineUiContainer(formats, s);
                                p.Inlines.Add(inlineUiContainer);
                            }
                            else
                            {
                                Run run = CreateRun(formats);
                                run.Text = s;
                                p.Inlines.Add(run);
                            }
                        
                        }
                    }

                    rtb.Blocks.Add(p);
                }
            }
        }

        private static string Replacements(string text)
        {
            return text.Replace("SIZE=3", "b")
                       .Replace("SIZE=4", "b")
                       .Replace("SIZE=5", "b")
                       .Replace("SIZE=6", "b")
                       .Replace("SIZE=7", "b")
                       .Replace("[/SIZE]", "[/b]")
                       .Replace("[B]", "[b]")
                       .Replace("[/B]", "[/b]")
                       .Replace("[spoiler]", "\r\n[spoiler]")
                       .Replace("[/spoiler]", "[/spoiler]\r\n")
                       .Replace("[SPOILER]", "\r\n[spoiler]")
                       .Replace("[/SPOILER]", "[/spoiler]\r\n"); ;
        }

        static Run CreateRun(IEnumerable<string> formats)
        {
            Run run = new Run();
            foreach (string format in formats)
            {
                switch (format)
                {
                    case "[b]":
                        run.FontWeight = FontWeights.Bold;
                        break;
                    case "[i]":
                        run.FontStyle = FontStyle.Italic;
                        break;
                    default:
                        if (format.StartsWith("[color="))
                        {
                            try
                            {
                                Color color = StringToColor(format.Substring(7, format.Length - 8));
                                run.Foreground = new SolidColorBrush(color);
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        break;
                }
            }
            return run;
        }

        static InlineUIContainer CreateInlineUiContainer(IEnumerable<string> formats, string s)
        {
            TextBlock headerText = new TextBlock
            {
                Foreground = new SolidColorBrush(Colors.Cyan),
                Text = "Spoiler!"
            };

            Expander expander = new Expander { Header = headerText, TextOnly = true, HorizontalAlignment = HorizontalAlignment.Center, LeftLine = new SolidColorBrush(Colors.Cyan)};
            Run run = new Run();
            foreach (string format in formats)
            {
                switch (format)
                {
                    case "[b]":
                        run.FontWeight = FontWeights.Bold;
                        break;
                    case "[i]":
                        run.FontStyle = FontStyle.Italic;
                        break;
                    default:
                        if (format.StartsWith("[color="))
                        {
                            try
                            {
                                Color color = StringToColor(format.Substring(7, format.Length - 8));
                                run.Foreground = new SolidColorBrush(color);
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        break;
                }
            }

            run.Text = s;
            TextBlock contentText = new TextBlock {TextWrapping = TextWrapping.WrapWholeWords};
            contentText.Inlines.Add(run);
            expander.Content = contentText;
            
            return new InlineUIContainer { Child = expander };
        }

        static Color StringToColor(string colorName)
        {
            string xaml = "<Color xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' >" + colorName + "</Color>";
            return (Color)XamlReader.Load(xaml);
        }

        static IEnumerable<string> SplitInBbCodeTags(char[] s)
        {
            int start = 0;
            for (int i = 1; i < s.Length; ++i)
            {
                if (s[i] == '[')
                {
                    yield return new string(s, start, i - start);
                    start = i;
                }
                if (s[i] == ']')
                {
                    yield return new string(s, start, i - start + 1);
                    start = i + 1;
                }
            }
            yield return new string(s, start, s.Length - start);
        }
    }
}