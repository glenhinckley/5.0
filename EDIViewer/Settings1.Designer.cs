﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Manual_test_app {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.8.0.0")]
    internal sealed partial class Settings1 : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings1 defaultInstance = ((Settings1)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings1())));
        
        public static Settings1 Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Persist Security Info=False;User ID=al60_seton_lite_developer_user;Password=AYImy" +
            "vmKCd14tMw9timGAoTVdCmIKdP6yDSSKymY7t0um+lShaMStpGRtDcLlS7O;Initial Catalog=al60" +
            "_seton_lite_developer;Data Source=10.1.1.120;pooling=false;connection Timeout=18" +
            "0\r\n")]
        public string ConString {
            get {
                return ((string)(this["ConString"]));
            }
            set {
                this["ConString"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("usp_secure_check_valid_user_al")]
        public string ALLoginCommandString {
            get {
                return ((string)(this["ALLoginCommandString"]));
            }
            set {
                this["ALLoginCommandString"] = value;
            }
        }
    }
}
