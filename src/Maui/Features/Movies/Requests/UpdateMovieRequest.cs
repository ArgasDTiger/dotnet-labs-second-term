﻿namespace Maui.Features.Movies.Requests;

public sealed record UpdateMovieRequest(
    string Title, 
    string Description,
    decimal CollateralValue,
    decimal PricePerDay);