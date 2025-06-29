﻿// <copyright file="RestApiResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Bookings.Api.Models.Responses;

public class RestApiResponse<T>
    where T : class
{
    public bool IsSuccess { get; private set; }

    public string Error { get; private set; }

    public T? Result { get; private set; }

    /// <summary>
    /// s.
    /// </summary>
    /// <param name="data">Data to wrap.</param>
    public static RestApiResponse<T> Success(T data)
    {
        return new RestApiResponse<T>()
        {
            IsSuccess = true,
            Result = data,
        };
    }

    public static RestApiResponse<T> NonSuccess(string error)
    {
        return new RestApiResponse<T>()
        {
            IsSuccess = false,
            Error = error,
            Result = null,
        };
    }
}
