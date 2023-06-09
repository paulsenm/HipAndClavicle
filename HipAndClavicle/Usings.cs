﻿global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Diagnostics;
global using System;

global using HipAndClavicle;
global using HipAndClavicle.Repositories;
global using HipAndClavicle.ViewModels;
global using HipAndClavicle.Controllers;
global using HipAndClavicle.Models;
global using HipAndClavicle.Models.JunctionTables;
global using HipAndClavicle.Models.Enums;
global using HipAndClavicle.Data;
global using Product = HipAndClavicle.Models.Product;
global using Review = HipAndClavicle.Models.Review;
global using Address = ShipEngineSDK.Common.Address;

global using AspNetCoreHero;
global using AspNetCoreHero.ToastNotification.Extensions;
global using AspNetCoreHero.ToastNotification.Abstractions;
global using AspNetCoreHero.ToastNotification;

global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Html;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Http.Features;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Identity.UI;
global using Microsoft.Extensions.Logging;
global using Microsoft.AspNetCore.Mvc.ModelBinding;
global using System.Runtime.InteropServices;
global using Microsoft.Extensions.DependencyInjection;

global using Pomelo.EntityFrameworkCore;
global using ShipEngineSDK;
global using ShipEngineSDK.Common;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
global using Microsoft.AspNetCore.Mvc.Rendering;
global using ShipEngineSDK.CreateLabelFromShipmentDetails;
global using ShipEngineSDK.Common.Enums;
global using Stripe;
global using StripeProduct = Stripe.Product;
global using StripeReview = Stripe.Review;
global using StripeAddress = Stripe.Address;

// TODO add messaging from admin orders
// TODO enable email functionality
// TODO add order history to user profile page
// TODO Fix image preview on edit order
// TODO Product creation needs to create a Stripe Product.
// TODO add shipping time to settings.
// TODO come back and look at printing invoices
// TODO calculate total when order is created
