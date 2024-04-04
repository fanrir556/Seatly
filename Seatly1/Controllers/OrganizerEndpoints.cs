using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Seatly1.Models;
namespace Seatly1.Controllers;

public static class OrganizerEndpoints
{
    public static void MapOrganizerEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Organizer").WithTags(nameof(Organizer));

        group.MapGet("/", async (SeatlyContext db) =>
        {
            return await db.Organizers.ToListAsync();
        })
        .WithName("GetAllOrganizers")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Organizer>, NotFound>> (int organizerid, SeatlyContext db) =>
        {
            return await db.Organizers.AsNoTracking()
                .FirstOrDefaultAsync(model => model.OrganizerId == organizerid)
                is Organizer model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetOrganizerById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int organizerid, Organizer organizer, SeatlyContext db) =>
        {
            var affected = await db.Organizers
                .Where(model => model.OrganizerId == organizerid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.OrganizerId, organizer.OrganizerId)
                    .SetProperty(m => m.OrganizerAccount, organizer.OrganizerAccount)
                    .SetProperty(m => m.LoginPassword, organizer.LoginPassword)
                    .SetProperty(m => m.OrganizerName, organizer.OrganizerName)
                    .SetProperty(m => m.OrganizerCategory, organizer.OrganizerCategory)
                    .SetProperty(m => m.OrganizerPhoto, organizer.OrganizerPhoto)
                    .SetProperty(m => m.Menu, organizer.Menu)
                    .SetProperty(m => m.Address, organizer.Address)
                    .SetProperty(m => m.ReservationUrl, organizer.ReservationUrl)
                    .SetProperty(m => m.Hashtag, organizer.Hashtag)
                    .SetProperty(m => m.Email, organizer.Email)
                    .SetProperty(m => m.Phone, organizer.Phone)
                    .SetProperty(m => m.Validation, organizer.Validation)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateOrganizer")
        .WithOpenApi();

        group.MapPost("/", async (Organizer organizer, SeatlyContext db) =>
        {
            db.Organizers.Add(organizer);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Organizer/{organizer.OrganizerId}",organizer);
        })
        .WithName("CreateOrganizer")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int organizerid, SeatlyContext db) =>
        {
            var affected = await db.Organizers
                .Where(model => model.OrganizerId == organizerid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteOrganizer")
        .WithOpenApi();
    }
}
