//
//  LightMapPointStyleKit.cs
//  ChristmasLightsFinder.IOS
//
//  Created by Jeff Miller on 2015-11-11.
//  Copyright (c) 2015 CompanyName. All rights reserved.
//
//  Generated by PaintCode (www.paintcodeapp.com)
//



using System;
using System.Drawing;
using Foundation;
using UIKit;
using CoreGraphics;

namespace ChristmasLightsFinder.IOS
{
    [Register ("LightMapPointStyleKit")]
    public class LightMapPointStyleKit : NSObject
    {

        //// Initialization

        static LightMapPointStyleKit()
        {
        }

        //// Drawing Methods

        public static void DrawLightMapPoint(float lightColorR, float lightColorG, float lightColorB, float lightColorA)
        {
            //// General Declarations
            var context = UIGraphics.GetCurrentContext();

            //// Color Declarations
            var lightBaseColor = UIColor.FromRGBA(0.009f, 0.471f, 0.009f, 1.000f);

            //// Shadow Declarations
            var shadow = new NSShadow();
            shadow.ShadowColor = UIColor.Black.ColorWithAlpha(0.62f);
            shadow.ShadowOffset = new CGSize(0.1f, -0.1f);
            shadow.ShadowBlurRadius = 5.0f;

            //// Variable Declarations
            var lightColor = UIColor.FromRGBA(lightColorR, lightColorG, lightColorB, lightColorA);

            //// LightIcon
            {
                //// Oval Drawing
                var ovalPath = UIBezierPath.FromOval(new CGRect(1.0f, 1.0f, 18.0f, 38.0f));
                lightColor.SetFill();
                ovalPath.Fill();

                ////// Oval Inner Shadow
                context.SaveState();
                context.ClipToRect(ovalPath.Bounds);
                context.SetShadow(new CGSize(0, 0), 0);
                context.SetAlpha(shadow.ShadowColor.CGColor.Alpha);
                context.BeginTransparencyLayer();
                {
                    var opaqueShadow = new CGColor(shadow.ShadowColor.CGColor, 1.0f);
                    context.SetShadow(shadow.ShadowOffset, shadow.ShadowBlurRadius, opaqueShadow);
                    context.SetBlendMode(CGBlendMode.SourceOut);
                    context.BeginTransparencyLayer();

                    context.SetFillColor(opaqueShadow);
                    ovalPath.Fill();

                    context.EndTransparencyLayer();
                }
                context.EndTransparencyLayer();
                context.RestoreState();



                //// Rectangle Drawing
                var rectanglePath = UIBezierPath.FromRect(new CGRect(3.0f, 34.0f, 14.0f, 12.0f));
                lightBaseColor.SetFill();
                rectanglePath.Fill();

                ////// Rectangle Inner Shadow
                context.SaveState();
                context.ClipToRect(rectanglePath.Bounds);
                context.SetShadow(new CGSize(0, 0), 0);
                context.SetAlpha(shadow.ShadowColor.CGColor.Alpha);
                context.BeginTransparencyLayer();
                {
                    var opaqueShadow = new CGColor(shadow.ShadowColor.CGColor, 1.0f);
                    context.SetShadow(shadow.ShadowOffset, shadow.ShadowBlurRadius, opaqueShadow);
                    context.SetBlendMode(CGBlendMode.SourceOut);
                    context.BeginTransparencyLayer();

                    context.SetFillColor(opaqueShadow);
                    rectanglePath.Fill();

                    context.EndTransparencyLayer();
                }
                context.EndTransparencyLayer();
                context.RestoreState();

            }
        }

        //// Generated Images

        public static UIImage ImageOfLightMapPoint(float lightColorR, float lightColorG, float lightColorB, float lightColorA)
        {
            UIGraphics.BeginImageContextWithOptions(new CGSize(20.0f, 47.0f), false, 0);
                        LightMapPointStyleKit.DrawLightMapPoint(lightColorR, lightColorG, lightColorB, lightColorA);

            var imageOfLightMapPoint = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return imageOfLightMapPoint;
        }

    }
}
