using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class ExceptionExtensions
{
    /// <summary>
    /// Exception ve altındaki tüm InnerException'ların mesajlarını hiyerarşik bir JSON string olarak döner.
    /// </summary>
    public static string ToJson(this Exception exception, bool includeStackTrace = false)
    {
        if (exception == null) return "{}";

        var options = new JsonSerializerOptions
        {
            WriteIndented = true, // Okunabilir (Pretty Print) olması için
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Türkçe karakterleri bozmaması için
        };

        // Exception'ı recursive olarak sade bir modele çeviriyoruz
        var simplifiedError = CreateSimplifiedError(exception, includeStackTrace);

        return JsonSerializer.Serialize(simplifiedError, options);
    }

    private static object CreateSimplifiedError(Exception ex, bool includeStackTrace)
    {
        if (ex == null) return null;

        // AggregateException (Task/Parallel işlemler) birden fazla inner exception barındırabilir.
        // Onları tek tek yakalamak gerekir.
        if (ex is AggregateException aggEx)
        {
            return new
            {
                Type = ex.GetType().Name,
                Message = ex.Message,
                StackTrace = includeStackTrace ? ex.StackTrace : null,
                InnerExceptions = aggEx.InnerExceptions.Select(i => CreateSimplifiedError(i, includeStackTrace)).ToList()
            };
        }

        // Standart Exception
        return new
        {
            Type = ex.GetType().Name,
            Message = ex.Message,
            StackTrace = includeStackTrace ? ex.StackTrace : null,
            InnerException = CreateSimplifiedError(ex.InnerException, includeStackTrace)
        };
    }
}