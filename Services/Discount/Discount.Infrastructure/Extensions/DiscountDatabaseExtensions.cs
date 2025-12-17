using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure.Extensions;

public static class DiscountDatabaseExtensions
{
    /// <summary>
    /// Ensures the Discount database schema and seed data are created.
    /// Call this once at startup.
    /// </summary>
    public static IHost InitializeDiscountDatabase<TContext>(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        var configuration = services.GetRequiredService<IConfiguration>();
        var logger = services.GetRequiredService<ILogger<DiscountDatabaseMarker>>();

        try
        {
            logger.LogInformation("Starting Discount database initialization...");
            SetupSchemaAndSeedData(configuration, logger);
            logger.LogInformation("Discount database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the Discount database.");
            throw;
        }

        return host;
    }

    /// <summary>
    /// Creates the Coupon table (dropping it if it exists) and inserts initial seed data.
    /// Retries a few times to handle transient startup issues (dependency container not ready, etc.).
    /// </summary>
    private static void SetupSchemaAndSeedData(IConfiguration configuration, ILogger logger)
    {
        const int maxRetries = 5;
        var retry = 0;

        var connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");

        while (true)
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();

                using var cmd = connection.CreateCommand();

                cmd.CommandText = "DROP TABLE IF EXISTS Coupon";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
                    CREATE TABLE Coupon(
                        Id          SERIAL PRIMARY KEY,
                        ProductName VARCHAR(500) NOT NULL,
                        Description TEXT,
                        Amount      INT
                    )";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
                    INSERT INTO Coupon(ProductName, Description, Amount)
                    VALUES
                    ('Adidas Quick Force Indoor Badminton Shoes', 'Shoe Discount', 500),
                    ('Yonex VCORE Pro 100 A Tennis Racquet (270gm, Strung)', 'Racquet Discount', 700);";
                cmd.ExecuteNonQuery();

                // success
                break;
            }
            catch (Exception ex) when (retry < maxRetries)
            {
                retry++;

                logger.LogWarning(
                    ex,
                    "Error initializing Discount database (attempt {Attempt}/{MaxAttempts}). Retrying in 2 seconds...",
                    retry,
                    maxRetries);

                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
        }
    }

    /// <summary>
    /// Marker type so ILogger<T> does not depend on a random DbContext or domain type.
    /// </summary>
    private sealed class DiscountDatabaseMarker { }
}