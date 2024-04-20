Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Transactions
Imports System.Xml
Imports System.Xml.Linq
Imports System.Xml.XPath
Imports Microsoft.VisualBasic
Imports Syncfusion.SfSkinManager
Imports Syncfusion.Windows.Shared
Imports Syncfusion.Windows.Tools.Controls
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Runtime.InteropServices
Module Program
    <STAThread>
    Public Sub Main()
        Dim WindowTitleDesignName As String = "FEM UI Design 01 - DEV MODE"
        Dim thread = New Thread(Sub()
                                    SfSkinManager.ApplyStylesOnApplication = True
                                    Dim app = New Application()
                                    Dim window = New Window With {
                                        .Title = WindowTitleDesignName,
                                        .Width = 1920,
                                        .Height = 1080,
                                        .WindowStartupLocation = WindowStartupLocation.CenterScreen,
                                                                               .WindowState = WindowState.Minimized,
                                        .WindowStyle = WindowStyle.SingleBorderWindow,
                                        .Background = Brushes.Black
                                    }
                                    Dim grid = New Grid()
                                    SfSkinManager.SetTheme(grid, New Theme("Windows11Dark"))
                                    BuildGrid(grid)
                                    window.Content = grid
                                    app.Run(window)
                                End Sub)
        thread.SetApartmentState(ApartmentState.STA)
        thread.Start()
        Console.WriteLine(WindowTitleDesignName & " is ready...")
        Console.WriteLine(WindowTitleDesignName & "WindowState = WindowState.Minimized")
        Console.ReadLine()
    End Sub
    Private Sub BuildGrid(grid As Grid)
        grid.Background = Brushes.Black
        grid.RowDefinitions.Add(New RowDefinition With {.Height = New GridLength(100)})
        grid.RowDefinitions.Add(New RowDefinition With {.Height = New GridLength(5)})
        grid.RowDefinitions.Add(New RowDefinition With {.Height = New GridLength(1, GridUnitType.Star)})
        grid.RowDefinitions.Add(New RowDefinition With {.Height = New GridLength(5)})
        grid.RowDefinitions.Add(New RowDefinition With {.Height = New GridLength(100)})
        grid.ColumnDefinitions.Add(New ColumnDefinition With {.Width = New GridLength(3, GridUnitType.Star)})
        grid.ColumnDefinitions.Add(New ColumnDefinition With {.Width = New GridLength(5)})
        grid.ColumnDefinitions.Add(New ColumnDefinition With {.Width = New GridLength(2, GridUnitType.Star)})
        grid.ColumnDefinitions.Add(New ColumnDefinition With {.Width = New GridLength(5)})
        grid.ColumnDefinitions.Add(New ColumnDefinition With {.Width = New GridLength(5, GridUnitType.Star)})
        Dim headerControl = New UserControl()
        headerControl.Content = New Label With {.Content = "Header Area", .HorizontalAlignment = HorizontalAlignment.Center}
        Grid.SetRow(headerControl, 0)
        Grid.SetColumnSpan(headerControl, 5)
        grid.Children.Add(headerControl)
        Dim gridSplitterTop = New GridSplitter With {
            .Height = 5,
            .ResizeDirection = GridResizeDirection.Rows,
            .HorizontalAlignment = HorizontalAlignment.Stretch
        }
        Grid.SetRow(gridSplitterTop, 1)
        Grid.SetColumnSpan(gridSplitterTop, 5)
        grid.Children.Add(gridSplitterTop)
        Dim listBox = CreateListBox2()
        Grid.SetRow(listBox, 2)
        Grid.SetColumn(listBox, 0)
        grid.Children.Add(listBox)
        Dim gridSplitter1 = New GridSplitter With {
            .Width = 5,
            .ResizeBehavior = GridResizeBehavior.PreviousAndNext,
            .HorizontalAlignment = HorizontalAlignment.Stretch
        }
        Grid.SetRow(gridSplitter1, 2)
        Grid.SetColumn(gridSplitter1, 1)
        grid.Children.Add(gridSplitter1)
        Dim treeViewUserControl = New TreeViewUserControl()
        Grid.SetRow(treeViewUserControl, 2)
        Grid.SetColumn(treeViewUserControl, 2)
        grid.Children.Add(treeViewUserControl)
        Dim gridSplitter2 = New GridSplitter With {
            .Width = 5,
            .ResizeBehavior = GridResizeBehavior.PreviousAndNext,
            .HorizontalAlignment = HorizontalAlignment.Stretch
        }
        Grid.SetRow(gridSplitter2, 2)
        Grid.SetColumn(gridSplitter2, 3)
        grid.Children.Add(gridSplitter2)
        Dim details = New DetailUserControl()
        Grid.SetRow(details, 2)
        Grid.SetColumn(details, 4)
        grid.Children.Add(details)
        Dim gridSplitterBottom = New GridSplitter With {
            .Height = 5,
            .ResizeDirection = GridResizeDirection.Rows,
            .HorizontalAlignment = HorizontalAlignment.Stretch
        }
        Grid.SetRow(gridSplitterBottom, 3)
        Grid.SetColumnSpan(gridSplitterBottom, 5)
        grid.Children.Add(gridSplitterBottom)
        Dim footerControl = New UserControl()
        footerControl.Content = New Label With {.Content = "Footer Area", .HorizontalAlignment = HorizontalAlignment.Center}
        Grid.SetRow(footerControl, 4)
        Grid.SetColumnSpan(footerControl, 5)
        grid.Children.Add(footerControl)
    End Sub
    Function CreateListBox2() As ListBox
        Dim listBox = New ListBox With {
            .DisplayMemberPath = "Text",
            .Margin = New Thickness(10)
        }
        AddHandler listBox.SelectionChanged, AddressOf OnListBoxSelectionChanged
        LoadItemsAsync(listBox)
        Return listBox
    End Function
    Private Sub OnListBoxSelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim listBox As ListBox = DirectCast(sender, ListBox)
        Dim selectedItem As ListBoxItem = TryCast(listBox.SelectedItem, ListBoxItem)
        Dim detailUserControl = DirectCast(DirectCast(listBox.Parent, Grid).Children.OfType(Of DetailUserControl)().FirstOrDefault(), DetailUserControl)
        If detailUserControl IsNot Nothing Then
            detailUserControl.DataContext = selectedItem.Content
            detailUserControl.UpdateDetails()
        End If
        Dim treeViewUserControl = DirectCast(DirectCast(listBox.Parent, Grid).Children.OfType(Of TreeViewUserControl)().FirstOrDefault(), TreeViewUserControl)
        If treeViewUserControl IsNot Nothing AndAlso TypeOf selectedItem.Content Is StackPanel Then
            Dim stackPanel = DirectCast(selectedItem.Content, StackPanel)
            Dim model = TryCast(stackPanel.DataContext, ItemViewModel)
            If model IsNot Nothing Then
                treeViewUserControl.UpdateTreeView(model)
            End If
        End If
    End Sub
    'TODO: Implement a more realistic data loading mechanism
    Async Function LoadItemsAsync(listBox As ListBox) As Task
        Await Task.Delay(1000)
        Dim items = Enumerable.Range(1, 10).Select(Function(i) New ItemViewModel With {.Text = $"Item {i}", .ImagePath = $"C:\Users\steph\Desktop\asf_{i Mod 2}.png"}).ToList()
        listBox.Dispatcher.Invoke(Sub() PopulateListBox(items, listBox))
    End Function
    Private Sub PopulateListBox(items As List(Of ItemViewModel), listBox As ListBox)
        For Each item In items
            Dim stackPanel = New StackPanel With {
                .Orientation = Orientation.Horizontal,
                .DataContext = item
            }
            Dim image = New System.Windows.Controls.Image With {
                .Source = New BitmapImage(New Uri(item.ImagePath, UriKind.Absolute)),
                .Width = 50,
                .Height = 50
            }
            Dim textBlock = New TextBlock With {
                .Text = item.Text,
                .VerticalAlignment = VerticalAlignment.Center
            }
            stackPanel.Children.Add(image)
            stackPanel.Children.Add(textBlock)
            listBox.Items.Add(New ListBoxItem With {.Content = stackPanel})
        Next
    End Sub
    Public Class ItemViewModel
        Public Property Text As String
        Public Property ImagePath As String
    End Class
    Public Class DetailUserControl
        Inherits UserControl
        Public Sub New()
            AddHandler Me.Loaded, AddressOf OnDetailUserControlLoaded
        End Sub
        Private Sub OnDetailUserControlLoaded(sender As Object, e As RoutedEventArgs)
            UpdateDetails()
        End Sub
        Public Sub UpdateDetails()
            Me.Content = New Label With {
                .Content = "Details loading...",
                .Margin = New Thickness(20)
            }
            Task.Delay(500).ContinueWith(Sub(t) UpdateDetailsContinuation())
        End Sub
        Private Sub UpdateDetailsContinuation()
            Dispatcher.Invoke(Sub() DisplayDetailContent())
        End Sub
        Private Sub DisplayDetailContent()
            If TypeOf DataContext Is StackPanel Then
                Dim panel = DirectCast(DataContext, StackPanel)
                Dim text = DirectCast(panel.Children(1), TextBlock).Text
                DirectCast(Me.Content, Label).Content = $"Details loaded for {text}!"
            Else
                DirectCast(Me.Content, Label).Content = "No item selected."
            End If
        End Sub
    End Class
    Public Class TreeViewUserControl
        Inherits UserControl
        Private treeView As TreeView
        Public Sub New()
            InitializeTreeView()
        End Sub
        Private Sub InitializeTreeView()
            treeView = New TreeView()
            Me.Content = treeView
        End Sub
        Public Sub UpdateTreeView(selectedItem As ItemViewModel)
            treeView.Items.Clear()
            If selectedItem IsNot Nothing Then
                Dim rootNode = New TreeViewItem With {
                    .Header = $"Details for {selectedItem.Text}"
                }
                Dim childNode1 = New TreeViewItem With {
                    .Header = $"{selectedItem.Text} - Child 1"
                }
                Dim childNode2 = New TreeViewItem With {
                    .Header = $"{selectedItem.Text} - Child 2"
                }
                rootNode.Items.Add(childNode1)
                rootNode.Items.Add(childNode2)
                treeView.Items.Add(rootNode)
                rootNode.IsExpanded = True
            End If
        End Sub
    End Class
End Module
