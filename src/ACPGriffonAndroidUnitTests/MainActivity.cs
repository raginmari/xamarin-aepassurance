﻿/*
    Copyright 2020 Adobe
    All Rights Reserved.
    NOTICE: Adobe permits you to use, modify, and distribute this file in
    accordance with the terms of the Adobe license agreement accompanying
    it. If you have received this file from a source other than Adobe,
    then your use, modification, or distribution of it requires the prior
    written permission of Adobe.
    This file has been modified from its original form. The original
    license can be viewed in the NOTICES.txt file.
*/

using System.Reflection;

using Android.App;
using Android.OS;
using Xamarin.Android.NUnitLite;
using System.Threading;
using Com.Adobe.Marketing.Mobile;

namespace ACPGriffonAndroidUnitTests
{
    [Activity(Label = "ACPGriffonAndroidUnitTests", MainLauncher = true)]
    public class MainActivity : TestSuiteActivity
    {
        static CountdownEvent latch = new CountdownEvent(1);
        protected override void OnCreate(Bundle bundle)
        {
            // tests can be inside the main assembly
            AddTest(Assembly.GetExecutingAssembly());
            // or in any reference assemblies
            // AddTest (typeof (Your.Library.TestClass).Assembly);

            // Once you called base.OnCreate(), you cannot add more assemblies.
            base.OnCreate(bundle);

            // setup for all tests
            ACPCore.Application = this.Application;
            ACPCore.SetWrapperType(WrapperType.Xamarin);
            ACPCore.LogLevel = LoggingMode.Verbose;
            ACPGriffon.RegisterExtension();

            // start core
            ACPCore.Start(new CoreStartCompletionCallback());
            latch.Wait();
            latch.Dispose();
        }

        class CoreStartCompletionCallback : Java.Lang.Object, IAdobeCallback
        {
            public void Call(Java.Lang.Object callback)
            {
                // set launch config
                ACPCore.ConfigureWithAppID("launch-ENf8ed5382efc84d5b81a9be8dcc231be1-development");
                latch.Signal();
            }
        }
    }
}
