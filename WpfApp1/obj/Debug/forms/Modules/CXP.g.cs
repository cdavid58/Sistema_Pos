﻿#pragma checksum "..\..\..\..\forms\Modules\CXP.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "AFD53E1BFC1DE01C5C05E81C942986FA791C4522"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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
using WpfApp1.forms;
using WpfApp1.forms.Modules;


namespace WpfApp1.forms.Modules {
    
    
    /// <summary>
    /// CXP
    /// </summary>
    public partial class CXP : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 76 "..\..\..\..\forms\Modules\CXP.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtCustomer;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\..\forms\Modules\CXP.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtNames;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\..\forms\Modules\CXP.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtAddress;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\..\forms\Modules\CXP.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtPhone;
        
        #line default
        #line hidden
        
        
        #line 126 "..\..\..\..\forms\Modules\CXP.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgInvoice;
        
        #line default
        #line hidden
        
        
        #line 261 "..\..\..\..\forms\Modules\CXP.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgResult;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfApp1;component/forms/modules/cxp.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\forms\Modules\CXP.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.txtCustomer = ((System.Windows.Controls.TextBox)(target));
            
            #line 77 "..\..\..\..\forms\Modules\CXP.xaml"
            this.txtCustomer.KeyDown += new System.Windows.Input.KeyEventHandler(this.TxtCustomer_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtNames = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.txtAddress = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.txtPhone = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.dgInvoice = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 6:
            this.dgResult = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

