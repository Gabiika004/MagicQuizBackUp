﻿#pragma checksum "..\..\..\..\..\View\Windows\QuestionWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A7FA4730E737374008C39B0D38AD5E13FC9A4AB0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FontAwesome.Sharp;
using MagicQuizDesktop.View.Windows;
using MagicQuizDesktop.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MagicQuizDesktop.View.Windows {
    
    
    /// <summary>
    /// QuestionWindow
    /// </summary>
    public partial class QuestionWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 78 "..\..\..\..\..\View\Windows\QuestionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMinimize;
        
        #line default
        #line hidden
        
        
        #line 110 "..\..\..\..\..\View\Windows\QuestionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClose;
        
        #line default
        #line hidden
        
        
        #line 186 "..\..\..\..\..\View\Windows\QuestionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtQuestion;
        
        #line default
        #line hidden
        
        
        #line 208 "..\..\..\..\..\View\Windows\QuestionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtTopic;
        
        #line default
        #line hidden
        
        
        #line 272 "..\..\..\..\..\View\Windows\QuestionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtAnswer1;
        
        #line default
        #line hidden
        
        
        #line 293 "..\..\..\..\..\View\Windows\QuestionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtAnswer2;
        
        #line default
        #line hidden
        
        
        #line 314 "..\..\..\..\..\View\Windows\QuestionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtAnswer3;
        
        #line default
        #line hidden
        
        
        #line 335 "..\..\..\..\..\View\Windows\QuestionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtAnswer4;
        
        #line default
        #line hidden
        
        
        #line 359 "..\..\..\..\..\View\Windows\QuestionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSubmit;
        
        #line default
        #line hidden
        
        
        #line 390 "..\..\..\..\..\View\Windows\QuestionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDelete;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.4.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MagicQuizDesktop;component/view/windows/questionwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\View\Windows\QuestionWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.4.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 15 "..\..\..\..\..\View\Windows\QuestionWindow.xaml"
            ((MagicQuizDesktop.View.Windows.QuestionWindow)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnMinimize = ((System.Windows.Controls.Button)(target));
            
            #line 86 "..\..\..\..\..\View\Windows\QuestionWindow.xaml"
            this.btnMinimize.Click += new System.Windows.RoutedEventHandler(this.btnMinimize_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnClose = ((System.Windows.Controls.Button)(target));
            
            #line 118 "..\..\..\..\..\View\Windows\QuestionWindow.xaml"
            this.btnClose.Click += new System.Windows.RoutedEventHandler(this.btnClose_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtQuestion = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.txtTopic = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.txtAnswer1 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.txtAnswer2 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.txtAnswer3 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.txtAnswer4 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.btnSubmit = ((System.Windows.Controls.Button)(target));
            return;
            case 11:
            this.btnDelete = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

