﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApplication2.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WpfApplication2.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #include &lt;stdarg.h&gt;
        ///#include &lt;stddef.h&gt;
        ///#include &lt;setjmp.h&gt;
        ///#include &lt;string.h&gt;
        ///#include &lt;cmocka.h&gt;
        ///
        ///#include &quot;{{filename.h}}&quot;
        ///
        ///static int TestID;
        ///
        ///static void setup()
        ///{
        ///	switch(TestID)
        ///	{
        ///	{{#testcases}}
        ///		case : {{id}}
        ///		{
        ///			
        ///			break;
        ///		}
        ///	{{/testcases}}
        ///	}
        ///}
        ///static void teardown()
        ///{
        ///	switch(TestID)
        ///	{
        ///	{{#testcases}}
        ///		case : {{id}}
        ///		{
        ///			
        ///			break;
        ///		}
        ///	{{/testcases}}
        ///	}
        ///}
        ////*Scenario: Some determinable business situation
        ///     Given some precondition
        ///       And so [rest of string was truncated]&quot;;.
        /// </summary>
        public static string template {
            get {
                return ResourceManager.GetString("template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to static void test_case_{{function_name}}_{{id}}()
        ///{
        ///	//declare intput arguments
        ///	{{#Arguments}}
        ///	{{type}} {{ispointer}} {{name}} {{isarray}} = {{value}};
        ///	{{/Arguments}}
        ///	TestID = {{id}};
        ///	//call function
        ///
        ///	{{#returnchecker}}
        ///	{{function_type}} ret = {{function_name}}({{#Arguments}}{{name}}{{comma}}{{/Arguments}});
        ///	{{/returnchecker}}
        ///	{{^returnchecker}}
        ///	{{function_name}}({{#Arguments}}{{name}},{{/Arguments}});
        ///	{{/returnchecker}}
        ///
        ///	{{#returnchecker}}
        ///	{{#memcheck}}
        ///
        ///	{{/memcheck}}
        ///	{{#i [rest of string was truncated]&quot;;.
        /// </summary>
        public static string testcase {
            get {
                return ResourceManager.GetString("testcase", resourceCulture);
            }
        }
    }
}
